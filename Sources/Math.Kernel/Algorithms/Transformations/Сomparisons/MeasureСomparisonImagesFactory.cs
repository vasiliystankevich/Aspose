using Math.Kernel.Algorithms.Transformations.Сomparisons;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Project.Kernel.NUnit;
using Unity;

namespace Math.Kernel.Algorithms.Transformations.Сomparisons
{
    public interface IMeasureСomparisonImagesFactory
    {
        IMeasureСomparisonImages GetMeasureСomparisonImages<TType>();
    }
    public class MeasureСomparisonImagesFactory: IMeasureСomparisonImagesFactory
    {
        public MeasureСomparisonImagesFactory(IUnityContainer container)
        {
            Container = container;
        }

        public IMeasureСomparisonImages GetMeasureСomparisonImages<TTypeName>() 
        {
            return Container.Resolve<IMeasureСomparisonImages<TTypeName>>();
        }

        protected IUnityContainer Container { get; set; }
    }
}

namespace Tests
{
    public class MeasureСomparisonImagesFactory:BaseMathKernelInstance<IMeasureСomparisonImagesFactory>
    {
        [Test]
        public void GetMeasureСomparisonImagesFirst()
        {
            var instanceMeasureСomparisonImages = Instance.GetMeasureСomparisonImages<IPreceptiveHashMeasureСomparisonImagesByArea>();
            this.IsNotNull(instanceMeasureСomparisonImages, "variable [instanceMeasureСomparisonImages] is null")
                .Verify(
                    () => instanceMeasureСomparisonImages.GetType() ==
                          typeof(Math.Kernel.Algorithms.Transformations.Сomparisons.PreceptiveHashMeasureСomparisonImagesByArea),
                    "type variable [instanceMeasureСomparisonImages] is not typeof(PreceptiveHashMeasureСomparisonImagesByArea)");
        }

        [Test]
        public void GetMeasureСomparisonImagesSecond()
        {
            var instanceMeasureСomparisonImages = Instance.GetMeasureСomparisonImages<IPreceptiveHashMeasureСomparisonImagesByImage>();
            this.IsNotNull(instanceMeasureСomparisonImages, "variable [instanceMeasureСomparisonImages] is null")
                .Verify(
                    () => instanceMeasureСomparisonImages.GetType() ==
                          typeof(Math.Kernel.Algorithms.Transformations.Сomparisons.PreceptiveHashMeasureСomparisonImagesByImage),
                    "type variable [instanceMeasureСomparisonImages] is not typeof(PreceptiveHashMeasureСomparisonImagesByImage)");
        }

        [Test]
        public void GetMeasureСomparisonImagesThird()
        {
            var instanceMeasureСomparisonImages = Instance.GetMeasureСomparisonImages<ILowHighMeasureComparasionImages>();
            this.IsNotNull(instanceMeasureСomparisonImages, "variable [instanceMeasureСomparisonImages] is null")
                .Verify(
                    () => instanceMeasureСomparisonImages.GetType() ==
                          typeof(Math.Kernel.Algorithms.Transformations.Сomparisons.LowHighMeasureComparasionImages),
                    "type variable [instanceMeasureСomparisonImages] is not typeof(LowHighMeasureComparasionImages)");
        }
    }
}
