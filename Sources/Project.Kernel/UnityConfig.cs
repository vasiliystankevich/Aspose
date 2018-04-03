using System;
using Unity;

namespace Project.Kernel
{
    public class UnityConfig:IDisposable
    {
        static UnityConfig() { }
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }

        public void Dispose()
        {
            if (Container.IsValueCreated)
                Container.Value.Dispose();
        }

        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() => new UnityContainer());
    }
}
