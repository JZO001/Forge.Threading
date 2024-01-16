using System;
using Forge.Threading.Tasking;

namespace Forge.Threading
{

    public static class AsyncDelegateMethodExtensions
    {

        public static readonly TaskManager TaskManager = new TaskManager(ChaosTheoryEnum.Chaos);

        public static ITaskResult BeginInvoke(this Action action, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state);
        }

        public static ITaskResult BeginInvoke<T1>(this Action<T1> action, T1 p1, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1);
        }

        public static ITaskResult BeginInvoke<T1, T2>(this Action<T1, T2> action, T1 p1, T2 p2, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3, p4);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3, p4, p5);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3, p4, p5, p6);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(action, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        public static void EndInvoke(this Action action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1>(this Action<T1> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2>(this Action<T1, T2> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        public static void EndInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        private static void EndActionInvokePrivate(ITaskResult taskResult)
        {
            if (taskResult == null)
            {
                throw new ArgumentNullException("taskResult");
            }
            if (!taskResult.IsCompleted)
            {
                try
                {
                    taskResult.WaitHandle.WaitOne();
                }
                catch (Exception)
                {
                }
            }
            taskResult.WaitHandle.Dispose();
        }

        public static ITaskResult BeginInvoke<TResult>(this Func<TResult> func, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state);
        }

        public static ITaskResult BeginInvoke<T1, TResult>(this Func<T1, TResult> func, T1 p1, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1);
        }

        public static ITaskResult BeginInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 p1, T2 p2, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 p1, T2 p2, T3 p3, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3, p4);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3, p4, p5);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3, p4, p5, p6);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7);
        }

        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, ReturnCallback asyncCallback, object state)
        {
            return TaskManager.Execute(func, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        public static TResult EndInvoke<TResult>(this Func<TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, TResult>(this Func<T1, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        private static TResult EndFuncInvokePrivate<TResult>(ITaskResult taskResult)
        {
            if (taskResult == null)
            {
                throw new ArgumentNullException("taskResult");
            }
            if (!taskResult.IsCompleted)
            {
                try
                {
                    taskResult.WaitHandle.WaitOne();
                }
                catch (Exception)
                {
                }
            }
            taskResult.WaitHandle.Dispose();
            return (TResult)taskResult.Result;
        }

    }

}
