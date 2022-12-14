/* *********************************************************************
 * Date: 27 Apr 2012
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using Forge.Legacy;
using Forge.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Forge.Threading
{

    /// <summary>
    /// Reentrance lock with deadlock detection ability
    /// </summary>
    [DebuggerDisplay("[{GetType()}, LockId = {LockId}, Name = {Name}]")]
    public sealed class DeadlockSafeLock : MBRBase, IDisposable, IEquatable<DeadlockSafeLock>, ILock
    {

        #region Field(s)

        private static readonly Dictionary<DeadlockSafeLock, object> EXISTING_LOCKS = new Dictionary<DeadlockSafeLock, object>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string mLockId = Guid.NewGuid().ToString();

        private readonly Dictionary<int, string> mQueuedThreads = new Dictionary<int, string>();

        private readonly Dictionary<int, string> mOwnerThreads = new Dictionary<int, string>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string mName = string.Empty;

        private readonly Semaphore mSemaphore = new Semaphore(1, 1);

        private Thread mOwner = null;

        private int mOwnerCounter = 0;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private bool mDisposed = false;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="DeadlockSafeLock"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public DeadlockSafeLock(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ThrowHelper.ThrowArgumentNullException("name");
            }
            mName = name;
            lock (EXISTING_LOCKS)
            {
                EXISTING_LOCKS.Add(this, null);
            }
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DeadlockSafeLock"/> is reclaimed by garbage collection.
        /// </summary>
        ~DeadlockSafeLock()
        {
            Dispose(false);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DebuggerHidden]
        public string Name
        {
            get { return mName; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="DeadlockSafeLock"/> is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disposed; otherwise, <c>false</c>.
        /// </value>
        [DebuggerHidden]
        public bool IsDisposed
        {
            get { return mDisposed; }
        }

        /// <summary>
        /// Gets the lock id.
        /// </summary>
        /// <value>
        /// The lock id.
        /// </value>
        [DebuggerHidden]
        public string LockId
        {
            get { return mLockId; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is held by current thread.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is held by current thread; otherwise, <c>false</c>.
        /// </value>
        public bool IsHeldByCurrentThread
        {
            get
            {
                return Thread.CurrentThread.Equals(this.mOwner);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is locked.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is locked; otherwise, <c>false</c>.
        /// </value>
        public bool IsLocked
        {
            get
            {
                return this.mOwner != null;
            }
        }

        #endregion

        #region Public method(s)

        /// <summary>
        /// Locks this instance.
        /// </summary>
        /// <exception cref="DeadlockException">Occurs when deadlock detected.</exception>
        /// <exception cref="ObjectDisposedException">Occurs when this instance has disposed.</exception>
        public void Lock()
        {
            DoDisposeCheck();

            bool isHeldByCurrentThread = IsHeldByCurrentThread;
            if (!isHeldByCurrentThread)
            {
                AdminThread();
            }

            LockInner(Timeout.Infinite);

            if (!isHeldByCurrentThread)
            {
                lock (EXISTING_LOCKS)
                {
                    mQueuedThreads.Remove(Thread.CurrentThread.ManagedThreadId);
                    mOwnerThreads.Add(Thread.CurrentThread.ManagedThreadId, GetStackTrace(new StackTrace()));
                }
            }
        }

        /// <summary>
        /// Tries the lock.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <exception cref="DeadlockException">Occurs when deadlock detected.</exception>
        /// <exception cref="ObjectDisposedException">Occurs when this instance has disposed.</exception>
        /// <exception cref="ArgumentNullException">Occurs when the TimeSpan parameter is null.</exception>
        /// <returns>True, if the lock acquired successfuly, otherwise False.</returns>
        public bool TryLock(TimeSpan timeout)
        {
#if NET40
            if (timeout == null)
            {
                ThrowHelper.ThrowArgumentNullException("timeout");
            }
#endif
            return TryLock(Convert.ToInt32(timeout.TotalMilliseconds));
        }

        /// <summary>
        /// Tries the lock.
        /// </summary>
        /// <param name="millisecondsTimeout">The milliseconds timeout.</param>
        /// <exception cref="DeadlockException">Occurs when deadlock detected.</exception>
        /// <exception cref="ObjectDisposedException">Occurs when this instance has disposed.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Occurs when int value is lower then Timeout.Infinite (-1)</exception>
        /// <returns>True, if the lock acquired successfuly, otherwise False.</returns>
        public bool TryLock(int millisecondsTimeout)
        {
            DoDisposeCheck();
            if (millisecondsTimeout < Timeout.Infinite)
            {
                ThrowHelper.ThrowArgumentOutOfRangeException("millisecondsTimeout");
            }

            bool result = false;
            bool isHeldByCurrentThread = IsHeldByCurrentThread;
            if (!isHeldByCurrentThread)
            {
                AdminThread();
            }
            result = LockInner(millisecondsTimeout);

            if (!isHeldByCurrentThread)
            {
                lock (EXISTING_LOCKS)
                {
                    mQueuedThreads.Remove(Thread.CurrentThread.ManagedThreadId);
                    if (result)
                    {
                        mOwnerThreads.Add(Thread.CurrentThread.ManagedThreadId, GetStackTrace(new StackTrace()));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Unlocks this instance.
        /// </summary>
        /// <exception cref="ObjectDisposedException">Occurs when this instance has disposed.</exception>
        public void Unlock()
        {
            DoDisposeCheck();

            if (!Thread.CurrentThread.Equals(mOwner))
            {
                throw new InvalidOperationException("Current thread does not the owner of the lock.");
            }

            if (mOwnerCounter == 1)
            {
                lock (EXISTING_LOCKS)
                {
                    mOwnerThreads.Remove(Thread.CurrentThread.ManagedThreadId);
                    mOwner = null;
                    mOwnerCounter = 0;
                }
                mSemaphore.Release();
            }
            else
            {
                mOwnerCounter--;
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (!obj.GetType().Equals(GetType())) return false;

            DeadlockSafeLock other = (DeadlockSafeLock)obj;
            return other.mLockId == mLockId && other.mName == mName;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>True, if the other class is equals with this.</returns>
        public bool Equals(DeadlockSafeLock other)
        {
            return Equals((object)other);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private method(s)

        private bool LockInner(int millisecondsTimeout)
        {
            DoDisposeCheck();
            bool result = true;

            if (IsHeldByCurrentThread)
            {
                mOwnerCounter++;
            }
            else
            {
                result = mSemaphore.WaitOne(millisecondsTimeout); // request access
                if (result)
                {
                    mOwner = Thread.CurrentThread; // store owner
                    mOwnerCounter = 1;
                }
            }

            return result;
        }

        private void AdminThread()
        {
            lock (EXISTING_LOCKS) // global lock
            {
                DoDisposeCheck();
                string stackTrace = GetStackTrace(new StackTrace());
                if (EXISTING_LOCKS.Count > 1 && !IsHeldByCurrentThread && IsLocked)
                {
                    // it is locked and its not mine
                    Thread currentOwner = mOwner; // it is the owner
                    // check the locks where I am the owner and check if the owner waits in the queue
                    foreach (DeadlockSafeLock dsl in EXISTING_LOCKS.Keys)
                    {
                        // skip the current lock
                        if (!Equals(dsl))
                        {
                            // check: this checked lock has already assigned to me and the current owner is waiting for the entry
                            if (dsl.IsHeldByCurrentThread && dsl.mQueuedThreads.ContainsKey(currentOwner.ManagedThreadId))
                            {
                                // on an owned lock, this owner has already waiting, this is a deadlock
                                throw new DeadlockException(mName, dsl.Name, currentOwner, dsl.mQueuedThreads[currentOwner.ManagedThreadId]);
                            }
                        }
                    }
                }
                // I did not detected deadlock, I am not the owner, I add myself to the to subscribers list
                if (!IsHeldByCurrentThread)
                {
                    mQueuedThreads.Add(Thread.CurrentThread.ManagedThreadId, stackTrace);
                }
            }
        }

        private static string GetStackTrace(StackTrace st)
        {
            StringBuilder sb = new StringBuilder(st.ToString());
            sb.AppendLine();
            return sb.ToString();
        }

        private void DoDisposeCheck()
        {
            if (mDisposed)
            {
                throw new ObjectDisposedException(string.Format("{0}, Name: {1}", GetType().FullName, mName));
            }
        }

        private void Dispose(bool disposing)
        {
            mDisposed = true;
            lock (EXISTING_LOCKS)
            {
                EXISTING_LOCKS.Remove(this);
            }
            if (disposing)
            {
                mSemaphore.Close();
            }
        }

        #endregion

    }

}
