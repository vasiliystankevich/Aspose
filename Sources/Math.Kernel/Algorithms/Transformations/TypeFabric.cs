using Project.Kernel;
using Unity;

namespace Math.Kernel.Algorithms.Transformations
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<ResizeTransformImageAttributes, ResizeTransformImageAttributes>();
            container.RegisterType<GrayScaleTransformImageAttributes, GrayScaleTransformImageAttributes>();
            container.RegisterType<ITransformImageAttributesFactory, TransformImageAttributesFactory>();
            container.RegisterType<ITransformImages, TransformImages>();
            container.RegisterType<IOtcuBinarization, OtcuBinarization>();
        }
    }
}
