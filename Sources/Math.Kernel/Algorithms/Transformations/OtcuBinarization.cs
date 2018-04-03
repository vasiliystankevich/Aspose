using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Math.Kernel.Algorithms;
using Math.Kernel.Algorithms.Transformations;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Unity;

namespace Math.Kernel.Algorithms.Transformations
{
    public interface IOtcuBinarization
    {
        int CalcThreshold(Color[,] colorsMatrix);
        Color[,] Binarization(Color[,] colorsMatrix, int threshold);
    }

    public class OtcuBinarization: IOtcuBinarization
    {
        public int CalcThreshold(Color[,] colorsMatrix)
        {
            var histogramm = new int[256];
            var width = colorsMatrix.GetLength(0);
            var height = colorsMatrix.GetLength(1);
            float sum = 0;
            float sumB = 0;
            var wB = 0;
            var threshold = 0;
            var total = width * height;
            float totalMax = 0;

            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                histogramm[colorsMatrix[x, y].R]++;

            for (var i = 1; i < 256; i++)
                sum += i * histogramm[i];

            for (var i = 0; i < 256; i++)
            {
                wB += histogramm[i];
                if (wB == 0) continue;

                var wF = total - wB; 
                if (wF == 0) break;

                sumB += i * histogramm[i];
                var mD = sumB / wB - (sum - sumB) / wF;
                var currentMax = wB * wF * mD * mD;

                if (!(currentMax > totalMax)) continue;
                totalMax = currentMax;
                threshold = i; 
            }

            return threshold;
        }

        public Color[,] Binarization(Color[,] colorsMatrix, int threshold)
        {
            var width = colorsMatrix.GetLength(0);
            var height = colorsMatrix.GetLength(1);
            var result = new Color[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                result[x, y] = colorsMatrix[x, y].R < threshold ? Color.Black : Color.White;
            return result;
        }
    }
}

namespace Tests
{
    public class OtcuBinarization : BaseMathKernelInstance<IOtcuBinarization>
    {
        public override void Init()
        {
            base.Init();
            TransformImages = Container.Resolve<ITransformImages>();
            ColorsMatrix = Container.Resolve<IColorsMatrix>();
        }

        [Test, TestCaseSource(nameof(CalcThresholdTestData))]
        public int CalcThreshold(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var grayScale = TransformImages.GrayScale(baseImage, baseImage.Width, baseImage.Height))
            {
                var grayScaleColorMatrix = ColorsMatrix.Get(grayScale);
                return Instance.CalcThreshold(grayScaleColorMatrix);
            }
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Binarization(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var grayScale = TransformImages.GrayScale(baseImage, baseImage.Width, baseImage.Height))
            {
                var grayScaleColorMatrix = ColorsMatrix.Get(grayScale);
                var threshold = Instance.CalcThreshold(grayScaleColorMatrix);
                Instance.Binarization(grayScaleColorMatrix, threshold);
            }
        }

        public static IEnumerable CalcThresholdTestData()
        {
            return new List<TestCaseData>
            {
                new TestCaseData("figure7.bmp").Returns(2),
                new TestCaseData("picture6.bmp").Returns(110)
            };
        }

        protected ITransformImages TransformImages { get; set; }
        protected IColorsMatrix ColorsMatrix { get; set; }
    }

}
