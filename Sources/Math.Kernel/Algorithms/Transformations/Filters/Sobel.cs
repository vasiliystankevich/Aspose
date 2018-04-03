using System;
using System.Drawing;
using Dal;
using Math.Kernel.Algorithms;
using Math.Kernel.Algorithms.Transformations;
using Math.Kernel.Algorithms.Transformations.Filters;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Unity;

namespace Math.Kernel.Algorithms.Transformations.Filters
{
    public interface ISobel { }

    public class SobelFilter :BaseFilter<ISobel>
    {
        public SobelFilter(IDalContext dalContext)
        {
            DalContext = dalContext;
            PreCalculatedFilterValues = DalContext.GetSobelDataMatrix();
        }

        protected override Color TransformAction(Color[,] image, int x, int y, int width, int height)
        {
            var gx = ConvolutionByX(image, x, y, width, height);
            var gy = ConvolutionByY(image, x, y, width, height);
            var g = System.Math.Pow(gx * gx + gy * gy, 0.5);
            return GenerateResultColor(image[x,y], g);
        }

        protected double ConvolutionByX(Color[,] image, int x, int y, int width, int height)
        {
            return CalcConvolution(image, KernelConvolutionX, x, y, width, height);
        }

        protected double ConvolutionByY(Color[,] image, int x, int y, int width, int height)
        {
            return CalcConvolution(image, KernelConvolutionY, x, y, width, height);
        }

        protected double[,] KernelConvolutionX = {{-1, 0, 1}, {-2, 0, 2}, {-1, 0, 1}};
        protected double[,] KernelConvolutionY = {{-1, -2, -1}, {0, 0, 0}, {1, 2, 1}};
    }
}

namespace Tests
{
    public class SobelFilter : BaseMathKernelInstance<IFilter<ISobel>>
    {
        public override void Init()
        {
            base.Init();
            TransformImages = Container.Resolve<ITransformImages>();
            ColorsMatrix = Container.Resolve<IColorsMatrix>();
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Transform(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var grayScale = TransformImages.GrayScale(baseImage, baseImage.Width, baseImage.Height))
            {
                var grayScaleColorMatrix = ColorsMatrix.Get(grayScale);
                Instance.Transform(grayScaleColorMatrix);
            }
        }

        protected ITransformImages TransformImages { get; set; }
        protected IColorsMatrix ColorsMatrix { get; set; }
    }

}
