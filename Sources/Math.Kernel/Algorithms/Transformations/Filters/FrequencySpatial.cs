using System;
using System.Drawing;
using Dal;
using Math.Kernel.Algorithms.Transformations.Filters;
using Math.Kernel.NUnit;

namespace Math.Kernel.Algorithms.Transformations.Filters
{
    public interface IHighFrequencySpatial { }
    public interface ILowFrequencySpatial { }
    public interface IMaskedImageTransform { }
    public abstract class BaseFrequencySpatialFilter<TName> : BaseFilter<TName>
    {
        protected BaseFrequencySpatialFilter(IDalContext dalContext)
        {
            DalContext = dalContext;
        }

        protected override Color TransformAction(Color[,] image, int x, int y, int width, int height)
        {
            var resultConvolution = CalcConvolution(image, KernelConvolution, x, y, width, height);
            return GenerateResultColor(image[x, y], resultConvolution);
        }

        protected double[,] KernelConvolution;
    }

    public class HighFrequencySpatialFilter: BaseFrequencySpatialFilter<IHighFrequencySpatial>
    {
        public HighFrequencySpatialFilter(IDalContext dalContext):base(dalContext)
        {
            KernelConvolution = new double[,] {{1, -2, 1}, {-2, 5, -2}, {1, -2, 1}};       ;
            PreCalculatedFilterValues = DalContext.GetHighFrequencySpatialDataMatrix();
        }
    }

    public class LowFrequencySpatialFilter : BaseFrequencySpatialFilter<ILowFrequencySpatial>
    {
        public LowFrequencySpatialFilter(IDalContext dalContext):base(dalContext)
        {
            KernelConvolution = new double[,] {{1, 2, 1}, {2, 4, 2}, {1, 2, 1}};
            PreCalculatedFilterValues = DalContext.GetLowFrequencySpatialDataMatrix();
        }
    }

    public class MaskedImageTransform: IFilter<IMaskedImageTransform>
    {
        public MaskedImageTransform(IFilter<ILowFrequencySpatial> lowFrequencySpatialFilter, IFilter<IHighFrequencySpatial> highFrequencySpatial)
        {
            LowFrequencySpatialFilter = lowFrequencySpatialFilter;
            HighFrequencySpatial = highFrequencySpatial;
        }

        public Color[,] Transform(Color[,] image)
        {
            var width = image.GetLength(0);
            var height = image.GetLength(1);
            var result = new Color[width, height];
            var lowImage = LowFrequencySpatialFilter.Transform(image);
            var highImage = LowFrequencySpatialFilter.Transform(image);
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
            {
                var highImageColorComponent = highImage[x, y].R;
                var lowImageColorComponent = lowImage[x, y].R;
                var maskedColorComponent = HighMaskedCooficent * highImageColorComponent -
                                           LowMaskedCooficent * lowImageColorComponent;
                result[x, y] = GenerateResultColor(image[x, y], maskedColorComponent);
            }
            return result;
        }

        protected Color GenerateResultColor(Color imagePointColor, double resultConvolution)
        {
            var convolutionColorComponent = Convert.ToInt32(System.Math.Round(resultConvolution));
            if (convolutionColorComponent > 255) convolutionColorComponent = imagePointColor.R;
            if (convolutionColorComponent < 0) convolutionColorComponent = imagePointColor.R;
            return Color.FromArgb(imagePointColor.A, convolutionColorComponent, convolutionColorComponent, convolutionColorComponent);
        }

        protected static double HighMaskedCooficent = .742;
        protected static double LowMaskedCooficent = 1 - HighMaskedCooficent;
        protected IFilter<ILowFrequencySpatial> LowFrequencySpatialFilter { get; }
        protected IFilter<IHighFrequencySpatial> HighFrequencySpatial { get; }
    }
}

namespace Tests
{
    public class HighFrequencySpatialFilter : BaseFilterInstance<IHighFrequencySpatial>
    {
    }

    public class LowFrequencySpatialFilter : BaseFilterInstance<ILowFrequencySpatial>
    {
    }

    public class MaskedImageTransform : BaseFilterInstance<IMaskedImageTransform>
    {
    }
}
