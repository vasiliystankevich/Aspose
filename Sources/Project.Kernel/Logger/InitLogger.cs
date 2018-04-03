using System.IO;
using log4net;
using log4net.Config;
using NUnit.Framework;
using Project.Kernel.NUnit;
using Unity;
using Unity.Injection;

namespace Project.Kernel.Logger
{
    public static class InitLogger
    {
        public static void RegisterTypes(IUnityContainer unityContainer, string logFileConfigPath)
        {
            unityContainer.RegisterType<IWrapper<ILog>>(new InjectionFactory(factory =>
            {
                XmlConfigurator.ConfigureAndWatch(new FileInfo(logFileConfigPath));
                var logger = LogManager.GetLogger(typeof(Wrapper<ILog>));
                var logInstance = new Wrapper<ILog>(logger);
                return logInstance;
            }));
        }

        public static void ConfigureAndWatch(string logFileConfigPath)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(logFileConfigPath));
        }

        internal class Test : BaseTest
        {
            [Test]
            public void LogTest()
            {
                Log.Instance.Info("NUinit.Base::BaseTest::LogTest");
            }
        }
    }
}
