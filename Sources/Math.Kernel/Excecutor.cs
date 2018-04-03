using System;
using log4net;
using Project.Kernel;

namespace Libraries.Core.Backend.Common
{
    public interface IExecutor
    {
        void ExecuteAction(Action action);
        void ExecuteAction<TArg1>(Action<TArg1> action, TArg1 arg1);
        void ExecuteAction<TArg1, TArg2>(Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2);
        TResult ExecuteFunc<TResult>(Func<TResult> functor);
        TResult ExecuteFunc<TArg1, TResult>(Func<TArg1, TResult> functor, TArg1 arg1);
        TResult ExecuteFunc<TArg1, TArg2, TResult>(Func<TArg1, TArg2, TResult> functor, TArg1 arg1, TArg2 arg2);
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
            }
            Logger.Instance.Info($"execute {nameAction} end...");
            return result;
        }

        public void ScanningException(Exception exception)
        {
            var logException = exception;
            while (logException != null)
            {
                Logger.Instance.Fatal(logException.Message);
                logException = logException.InnerException;
            }
        }
    }
}
