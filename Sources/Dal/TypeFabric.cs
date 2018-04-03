using Project.Kernel;
using Unity;

namespace Dal
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterSingleton<IDalContext, LocalDalContext>();
        }
    }
}
