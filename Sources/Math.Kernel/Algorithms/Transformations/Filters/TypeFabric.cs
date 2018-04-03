using Project.Kernel;
using Unity;

namespace Math.Kernel.Algorithms.Transformations.Filters
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IFilter<ISobel>, SobelFilter>();
            container.RegisterType<IFilter<ILowFrequencySpatial>, LowFrequencySpatialFilter>();
            container.RegisterType<IFilter<IHighFrequencySpatial>, HighFrequencySpatialFilter>();
            container.RegisterType<IFilter<IMaskedImageTransform>, MaskedImageTransform>();
        }
    }
}
