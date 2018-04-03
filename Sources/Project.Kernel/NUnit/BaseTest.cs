using System;
using System.IO;
using log4net;
using NUnit.Framework;
using Project.Kernel.Logger;
using Unity;

namespace Project.Kernel.NUnit
{
    [TestFixture]
    public abstract class BaseTest
    {
        [SetUp]
        public virtual void Init()
        {
            Container = UnityConfig.GetConfiguredContainer();
            RegisterTypes();
            Log = Container.Resolve<IWrapper<ILog>>();
        }

        public virtual void RegisterTypes()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var logConfigPath = Path.Combine(basePath, "log4net.config");
            var logConfig = new FileInfo(logConfigPath);
            InitLogger.RegisterTypes(Container, logConfig.FullName);
        }

        protected IUnityContainer Container { get; set; }
        protected IWrapper<ILog> Log { get; set; } 
    }

    public abstract class BaseTestInstance<TInterface> : BaseTest
    {
        public override void Init()
        {
            base.Init();
            Instance = Container.Resolve<TInterface>();
        }
        protected TInterface Instance { get; set; }
    }

    public static class TestExtensions
    {
        public static BaseTest IsNotNull(this BaseTest sender, object data, string message)
        {
            return sender.Verify(() => data != null, message);
        }
        public static BaseTest Verify(this BaseTest sender, Func<bool> thatFunc, string message)
        {
            Assert.That(thatFunc, message);
            return sender;
        }
    }
}
