using System;
using System.Collections.Generic;
using System.Drawing;
using Dal;

namespace Math.Kernel.Algorithms.Transformations.Filters
{
    public interface ICalcConvolution
    {
        double CalcConvolution(Color[,] image, double[,] kernelConvolution, int x, int y, int width, int height);
    }
    public interface IFilterData
    {
        Dictionary<double, List<double>> PreCalculatedFilterValues { get; set; }
        IDalContext DalContext { get; set; }
    }
    public interface IFilter<TName>
    {
        Color[,] Transform(Color[,] image);
    }

    public abstract class BaseFilter<TName>: IFilter<TName>, ICalcConvolution, IFilterData
    {
        public Color[,] Transform(Color[,] image)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);
            var result = new Color[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                result[x, y] = TransformAction(image, x, y, width, height);
            return result;
        }

        public double CalcConvolution(Color[,] image, double[,] kernelConvolution, int x, int y, int width, int height)
        {
            var result = 0.0;
            for (var i = -1; i <= 1; i++)
            {
                if (x + i < 0) continue;
                if (x + i >= width) continue;
                for (var j = -1; j <= 1; j++)
                {
                    if (y + j < 0) continue;
                    if (y + j >= height) continue;
                    var index = kernelConvolution[i + 1, j + 1];
                    if (System.Math.Abs(index) < 0.01) continue;
                    var value = image[x + i, y + j].R;
                    result += PreCalculatedFilterValues[index][value];
                }
            }
            return result;
        }

        protected static Color GenerateResultColor(Color imagePointColor, double resultConvolution)
        {
            var convolutionColorComponent = Convert.ToInt32(System.Math.Round(resultConvolution));
            if (convolutionColorComponent > 255) convolutionColorComponent = imagePointColor.R;
            if (convolutionColorComponent < 0) convolutionColorComponent = imagePointColor.R;
            return Color.FromArgb(imagePointColor.A, convolutionColorComponent, convolutionColorComponent, convolutionColorComponent);
        }

        protected abstract Color TransformAction(Color[,] image, int x, int y, int width, int height);
        public Dictionary<double, List<double>> PreCalculatedFilterValues { get; set; }
        public IDalContext DalContext { get; set; }
    }
}
