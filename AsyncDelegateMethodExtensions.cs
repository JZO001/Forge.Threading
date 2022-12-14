using Forge.Threading.Tasking;
using System;

namespace Forge.Threading
{

    /// <summary>Extension methods</summary>
    public static class AsyncDelegateMethodExtensions
    {

        /// <summary>The task manager</summary>
        public static readonly TaskManager TaskManager = new TaskManager(ChaosTheoryEnum.Chaos);

#if NET6_0_OR_GREATER
#nullable enable
#endif

        #region Actions

        /// <summary>Begins the invoke.</summary>
        /// <param name="action">The action.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        public static ITaskResult BeginInvoke(this Action action, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute(action, asyncCallback, state);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 0.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p0.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1>(this Action<T1> action, T1 p1, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1>(action, asyncCallback, state, p1);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 0.</typeparam>
        /// <typeparam name="T2">The type of the 1.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p0.</param>
        /// <param name="p2">The p1.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2>(this Action<T1, T2> action, T1 p1, T2 p2, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2>(action, asyncCallback, state, p1, p2);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 0.</typeparam>
        /// <typeparam name="T2">The type of the 1.</typeparam>
        /// <typeparam name="T3">The type of the 2.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p0.</param>
        /// <param name="p2">The p1.</param>
        /// <param name="p3">The p2.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 p1, T2 p2, T3 p3, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3>(action, asyncCallback, state, p1, p2, p3);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 0.</typeparam>
        /// <typeparam name="T2">The type of the 1.</typeparam>
        /// <typeparam name="T3">The type of the 2.</typeparam>
        /// <typeparam name="T4">The type of the 3.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p0.</param>
        /// <param name="p2">The p1.</param>
        /// <param name="p3">The p2.</param>
        /// <param name="p4">The p3.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 p1, T2 p2, T3 p3, T4 p4, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4>(action, asyncCallback, state, p1, p2, p3, p4);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 0.</typeparam>
        /// <typeparam name="T2">The type of the 1.</typeparam>
        /// <typeparam name="T3">The type of the 2.</typeparam>
        /// <typeparam name="T4">The type of the 3.</typeparam>
        /// <typeparam name="T5">The type of the 4.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p0.</param>
        /// <param name="p2">The p1.</param>
        /// <param name="p3">The p2.</param>
        /// <param name="p4">The p3.</param>
        /// <param name="p5">The p4.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5>(action, asyncCallback, state, p1, p2, p3, p4, p5);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6>(action, asyncCallback, state, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6, T7>(action, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, ReturnCallback
#if NET6_0_OR_GREATER
            ? 
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ? 
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6, T7, T8>(action, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>Ends the invoke.</summary>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke(this Action action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1>(this Action<T1> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2>(this Action<T1, T2> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <param name="action">The action.</param>
        /// <param name="taskResult">The task result.</param>
        public static void EndInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> action, ITaskResult taskResult)
        {
            EndActionInvokePrivate(taskResult);
        }

        private static void EndActionInvokePrivate(ITaskResult taskResult)
        {
            if (taskResult == null) throw new ArgumentNullException(nameof(taskResult));

            if (!taskResult.IsCompleted)
            {
                try
                {
                    taskResult.WaitHandle.WaitOne();
                }
                catch (Exception) { }
            }
            taskResult.WaitHandle.Dispose();
        }

        #endregion

        #region Func(s)

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<TResult>(this Func<TResult> func, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<TResult>(func, asyncCallback, state);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, TResult>(this Func<T1, TResult> func, T1 p1, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, TResult>(func, asyncCallback, state, p1);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, T1 p1, T2 p2, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, TResult>(func, asyncCallback, state, p1, p2);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, T1 p1, T2 p2, T3 p3, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, TResult>(func, asyncCallback, state, p1, p2, p3);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, TResult>(func, asyncCallback, state, p1, p2, p3, p4);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, TResult>(func, asyncCallback, state, p1, p2, p3, p4, p5);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6, TResult>(func, asyncCallback, state, p1, p2, p3, p4, p5, p6);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6, T7, TResult>(func, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7);
        }

        /// <summary>Begins the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="p1">The p1.</param>
        /// <param name="p2">The p2.</param>
        /// <param name="p3">The p3.</param>
        /// <param name="p4">The p4.</param>
        /// <param name="p5">The p5.</param>
        /// <param name="p6">The p6.</param>
        /// <param name="p7">The p7.</param>
        /// <param name="p8">The p8.</param>
        /// <param name="asyncCallback">The asynchronous callback.</param>
        /// <param name="state">The state.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static ITaskResult BeginInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, T1 p1, T2 p2, T3 p3, T4 p4, T5 p5, T6 p6, T7 p7, T8 p8, ReturnCallback
#if NET6_0_OR_GREATER
            ?
#endif
            asyncCallback, object
#if NET6_0_OR_GREATER
            ?
#endif
            state)
        {
            return TaskManager.Execute<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(func, asyncCallback, state, p1, p2, p3, p4, p5, p6, p7, p8);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<TResult>(this Func<TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, TResult>(this Func<T1, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, TResult>(this Func<T1, T2, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, TResult>(this Func<T1, T2, T3, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, T4, TResult>(this Func<T1, T2, T3, T4, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, T4, T5, TResult>(this Func<T1, T2, T3, T4, T5, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, TResult>(this Func<T1, T2, T3, T4, T5, T6, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, T7, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        /// <summary>Ends the invoke.</summary>
        /// <typeparam name="T1">The type of the 1.</typeparam>
        /// <typeparam name="T2">The type of the 2.</typeparam>
        /// <typeparam name="T3">The type of the 3.</typeparam>
        /// <typeparam name="T4">The type of the 4.</typeparam>
        /// <typeparam name="T5">The type of the 5.</typeparam>
        /// <typeparam name="T6">The type of the 6.</typeparam>
        /// <typeparam name="T7">The type of the 7.</typeparam>
        /// <typeparam name="T8">The type of the 8.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="taskResult">The task result.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static TResult EndInvoke<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> func, ITaskResult taskResult)
        {
            return EndFuncInvokePrivate<TResult>(taskResult);
        }

        private static TResult EndFuncInvokePrivate<TResult>(ITaskResult taskResult)
        {
            if (taskResult == null) throw new ArgumentNullException(nameof(taskResult));

            if (!taskResult.IsCompleted)
            {
                try
                {
                    taskResult.WaitHandle.WaitOne();
                }
                catch (Exception) { }
            }
            taskResult.WaitHandle.Dispose();
            return (TResult)taskResult.Result;
        }

        #endregion

#if NET6_0_OR_GREATER
#nullable disable
#endif

    }

}
