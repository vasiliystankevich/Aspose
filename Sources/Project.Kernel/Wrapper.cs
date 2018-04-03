using System;

namespace Project.Kernel
{
    public interface IWrapper<T>:IDisposable
    {
        T Instance { get; set; }
    }

    public class Wrapper<T> : IWrapper<T>
    {
        public Wrapper()
        {
        }

        public Wrapper(T instance)
        {
            Instance = instance;
        }

        public void Dispose()
        {
            var disposableInstance = Instance as IDisposable;
            disposableInstance?.Dispose();
        }

        public T Instance { get; set; }
    }
}
