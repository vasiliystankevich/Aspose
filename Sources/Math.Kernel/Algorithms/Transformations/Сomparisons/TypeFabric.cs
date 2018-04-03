using Project.Kernel;
using Unity;

namespace Math.Kernel.Algorithms.Transformations.Сomparisons
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IMeasureСomparisonImages<IPreceptiveHashMeasureСomparisonImagesByArea>, PreceptiveHashMeasureСomparisonImagesByArea>();
            container.RegisterType<IMeasureСomparisonImages<IPreceptiveHashMeasureСomparisonImagesByImage>, PreceptiveHashMeasureСomparisonImagesByImage>();
            container.RegisterType<IMeasureСomparisonImages<ILowHighMeasureComparasionImages>, LowHighMeasureComparasionImages>();
            container.RegisterType<IMeasureСomparisonImagesFactory, MeasureСomparisonImagesFactory>();
        }
    }
}
