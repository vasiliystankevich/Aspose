using Project.Kernel;
using Unity;

namespace Math.Kernel
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            RegisterTypes<Algorithms.TypeFabric>(container);
            RegisterTypes<Algorithms.Transformations.TypeFabric>(container);
            RegisterTypes<Algorithms.Transformations.Filters.TypeFabric>(container);
            RegisterTypes<Algorithms.Transformations.Сomparisons.TypeFabric>(container);
        }
    }
}