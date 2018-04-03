using System;
using log4net;
using NUnit.Framework.Internal;

namespace Project.Kernel
{
    public interface IExecutor
    {
        void ExecuteAction(Action action);
        void ExecuteAction<TArg1>(Action<TArg1> action, TArg1 arg1);
        void ExecuteAction<TArg1, TArg2>(Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2);
        void ExecuteAction<TArg1, TArg2, TArg3>(Action<TArg1, TArg2, TArg3> action, TArg1 arg1, TArg2 arg2, TArg3 arg3);
        TResult ExecuteFunc<TResult>(Func<TResult> functor);
        TResult ExecuteFunc<TArg1, TResult>(Func<TArg1, TResult> functor, TArg1 arg1);
        TResult ExecuteFunc<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> functor, TArg1 arg1, TArg2 arg2);
        TResult ExecuteFunc<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> functor, TArg1 arg1, TArg2 arg2, TArg3 arg3);
        event Action<object, Exception> OnException;
    }

    public class Executor: BaseRepository,IExecutor
    {
        public Executor(IWrapper<ILog> logger) : base(logger)
        {
        }

        public void ExecuteAction(Action action)
        {
            var nameAction = $"{action.Method.DeclaringType.Name}.{action.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            try
            {
                action();
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
        }

        public void ExecuteAction<TArg1>(Action<TArg1> action, TArg1 arg1)
        {
            var nameAction = $"{action.Method.DeclaringType.Name}.{action.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            try
            {
                action(arg1);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
        }

        public void ExecuteAction<TArg1, TArg2>(Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2)
        {
            var nameAction = $"{action.Method.DeclaringType.Name}.{action.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            try
            {
                action(arg1, arg2);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
        }

        public void ExecuteAction<TArg1, TArg2, TArg3>(Action<TArg1, TArg2, TArg3> action, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            var nameAction = $"{action.Method.DeclaringType.Name}.{action.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            try
            {
                action(arg1, arg2, arg3);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
        }

        public TResult ExecuteFunc<TResult>(Func<TResult> functor)
        {
            var nameAction = $"{functor.Method.DeclaringType.Name}.{functor.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            var result = default(TResult);
            try
            {
                result = functor();
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
            return result;
        }

        public TResult ExecuteFunc<TArg1, TResult>(Func<TArg1, TResult> functor, TArg1 arg1)
        {
            var nameAction = $"{functor.Method.DeclaringType.Name}.{functor.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            var result = default(TResult);
            try
            {
                result = functor(arg1);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
            return result;
        }

        public TResult ExecuteFunc<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> functor, TArg1 arg1, TArg2 arg2)
        {
            var nameAction = $"{functor.Method.DeclaringType.Name}.{functor.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            var result = default(TResult);
            try
            {
                result = functor(arg1, arg2);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
            return result;
        }

        public TResult ExecuteFunc<TArg1, TArg2, TArg3, TResult>(Func<TArg1, TArg2, TArg3, TResult> functor, TArg1 arg1, TArg2 arg2, TArg3 arg3)
        {
            var nameAction = $"{functor.Method.DeclaringType.Name}.{functor.Method.Name}";
            Logger.Instance.Info($"execute {nameAction} begin...");
            var result = default(TResult);
            try
            {
                result = functor(arg1, arg2, arg3);
            }
            catch (Exception e)
            {
                ScanningException(e);
                OnException(this, e);
            }
            Logger.Instance.Info($"execute {nameAction} end...");
            return result;
        }

        protected void ScanningException(Exception exception)
        {
            var logException = exception;
            while (logException != null)
            {
                Logger.Instance.Fatal(logException.Message);
                logException = logException.InnerException;
            }
        }

        public event Action<object, Exception> OnException = (sender, exception) => { };
    }
}
