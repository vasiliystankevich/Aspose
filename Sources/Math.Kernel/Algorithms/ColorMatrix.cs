using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Math.Kernel.Algorithms;
using Math.Kernel.NUnit;
using NUnit.Framework;

namespace Math.Kernel.Algorithms
{
    public interface IColorsMatrix
    {
        Color[,] Get(Bitmap image);
        void Set(Bitmap image, Color[,] colorsMatrix);
    }
    public class ColorsMatrix: IColorsMatrix
    {
        public Color[,] Get(Bitmap image)
        {
            var width = image.Width;
            var height = image.Height;
            var colorMatrix = new Color[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                colorMatrix[x, y] = image.GetPixel(x, y);
            return colorMatrix;
        }

        public void Set(Bitmap image, Color[,] colorMatrix)
        {
            var width = colorMatrix.GetLength(0);
            var height = colorMatrix.GetLength(1);
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                image.SetPixel(x, y, colorMatrix[x, y]);
        }
    }
}

namespace Tests
{
    public class ColorsMatrix : BaseMathKernelInstance<IColorsMatrix>
    {
        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Get(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            {
                Instance.Get(baseImage);
            }
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Set(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var resultImage = new Bitmap(baseImage.Width, baseImage.Height))
            {
                var colorMatrix = Instance.Get(baseImage);
                Instance.Set(resultImage, colorMatrix);
            }
        }
    }

}
