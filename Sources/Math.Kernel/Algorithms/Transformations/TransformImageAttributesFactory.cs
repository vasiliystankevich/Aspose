using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Unity;

namespace Math.Kernel.Algorithms.Transformations
{
    public interface ITransformImageAttributes
    {
        ImageAttributes Get();
    }

    public class ResizeTransformImageAttributes : ITransformImageAttributes
    {
        public ImageAttributes Get()
        {
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
            return imageAttributes;
        }
    }

    public class GrayScaleTransformImageAttributes : ITransformImageAttributes
    {
        public ImageAttributes Get()
        {
            var imageAttributes = new ImageAttributes();
            imageAttributes.SetWrapMode(WrapMode.TileFlipXY);
            imageAttributes.SetColorMatrix(GreyColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttributes;
        }

        public static readonly ColorMatrix GreyColorMatrix = new ColorMatrix(new[]
        {
            new[] {.3f, .3f, .3f, 0, 0},
            new[] {.59f, .59f, .59f, 0, 0},
            new[] {.11f, .11f, .11f, 0, 0},
            new[] {0f, 0f, 0f, 1f, 0f},
            new[] {0f, 0f, 0f, 0f, 1f}
        });
    }

    public interface ITransformImageAttributesFactory
    {
        ITransformImageAttributes CreateResizeTransformImageAttributes();
        ITransformImageAttributes CreateGrayScaleTransformImageAttributes();
    }
    public class TransformImageAttributesFactory : ITransformImageAttributesFactory
    {
        public TransformImageAttributesFactory(IUnityContainer container)
        {
            Container = container;
        }

        protected ITransformImageAttributes CreateTransform<T>() where T:class, ITransformImageAttributes, new()
        {
            return Container.Resolve<T>();
        }

        public ITransformImageAttributes CreateResizeTransformImageAttributes()
        {
            return CreateTransform<ResizeTransformImageAttributes>();
        }

        public ITransformImageAttributes CreateGrayScaleTransformImageAttributes()
        {
            return CreateTransform<GrayScaleTransformImageAttributes>();
        }

        protected IUnityContainer Container { get; set; }
    }
}
