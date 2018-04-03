using System.Drawing;
using System.Drawing.Drawing2D;
using Math.Kernel.Algorithms.Models;
using Math.Kernel.Algorithms.Transformations;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Project.Kernel.NUnit;

namespace Math.Kernel.Algorithms.Transformations
{
    public interface ITransformImages
    {
        Bitmap Resize(Bitmap image, int width, int height);
        Bitmap GrayScale(Bitmap image, int width, int height);
    }

    public class TransformImages:ITransformImages
    {
        public TransformImages(ITransformImageAttributesFactory transformImageFactory)
        {
            TransformImageFactory = transformImageFactory;
        }

        public Bitmap Transform(Bitmap image, TransformDataPicture transformDataPicture)
        {
            var destRect = new Rectangle(0, 0, transformDataPicture.Width, transformDataPicture.Height);
            var destImage = new Bitmap(transformDataPicture.Width, transformDataPicture.Height, image.PixelFormat);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.High;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var imageAttributes = transformDataPicture.TransformImageAttributes.Get())
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttributes);
            }
            return destImage;
        }

        public Bitmap Resize(Bitmap image, int width, int height)
        {
            var transform = TransformImageFactory.CreateResizeTransformImageAttributes();
            var transformData = new TransformDataPicture(width, height, transform);
            return Transform(image, transformData);
        }

        public Bitmap GrayScale(Bitmap image, int width, int height)
        {
            var transform = TransformImageFactory.CreateGrayScaleTransformImageAttributes();
            var transformData = new TransformDataPicture(width, height, transform);
            return Transform(image, transformData);
        }

        public ITransformImageAttributesFactory TransformImageFactory { get; set; }
    }
}

namespace Tests
{
    public class TransformImages : BaseMathKernelInstance<ITransformImages>
    {
        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Resize(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var resizeImage = Instance.Resize(baseImage, 128, 128))
            {
                this.IsNotNull(resizeImage, "variable [resizeImage] is null")
                    .Verify(() => resizeImage.GetType() == typeof(Bitmap),
                        "type variable [resizeImage] is not typeof(Bitmap)")
                    .Verify(() => resizeImage.Width == 128, $"variable [resizeImage] is not width 128px")
                    .Verify(() => resizeImage.Height == 128, $"variable [resizeImage] is not height 128px");
            }
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void GrayScale(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var grayScaleImage = Instance.GrayScale(baseImage, 128, 128))
            {
                this.IsNotNull(grayScaleImage, "variable [resizeImage] is null")
                    .Verify(() => grayScaleImage.GetType() == typeof(Bitmap),
                        "type variable [grayScaleImage] is not typeof(Bitmap)")
                    .Verify(() => grayScaleImage.Width == 128, $"variable [grayScaleImage] is not width 128px")
                    .Verify(() => grayScaleImage.Height == 128, $"variable [grayScaleImage] is not height 128px");
            }
        }
    }
}
