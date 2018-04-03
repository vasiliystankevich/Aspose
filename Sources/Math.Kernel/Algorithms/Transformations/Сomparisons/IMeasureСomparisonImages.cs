using System.Collections.Generic;
using System.Drawing;

namespace Math.Kernel.Algorithms.Transformations.Сomparisons
{
    public interface IMeasureСomparisonImages
    {
        double Compare(Color[,] imageFirst, Color[,] imageSecond);
        Dictionary<KeyValuePair<int, int>, double> Compare(List<Color[,]> imagesForComparison);
    }

    public interface IMeasureСomparisonImages<TName> : IMeasureСomparisonImages { }
}
