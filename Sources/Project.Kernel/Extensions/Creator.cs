using System;

namespace Project.Kernel.Extensions
{
    public class Creator<T> where T:class, new()
    {
        public static T Create() => new T();

        public static T Create<TArg1>(TArg1 arg1) => (T)Activator.CreateInstance(typeof(T), arg1);

        public static T Create<TArg1, TArg2>(TArg1 arg1, TArg2 arg2) => (T) Activator.CreateInstance(typeof(T), arg1, arg2);

        public static T Create<TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3) => (T) Activator.CreateInstance(typeof(T), arg1, arg2, arg3);
    }
}
