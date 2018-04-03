using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Project.Kernel.NUnit;

namespace Math.Kernel.Algorithms.Transformations.Сomparisons
{
    public interface IPreceptiveHashMeasureСomparisonImagesByImage { }
    public class PreceptiveHashMeasureСomparisonImagesByImage : IMeasureСomparisonImages<IPreceptiveHashMeasureСomparisonImagesByImage>
    {
        public PreceptiveHashMeasureСomparisonImagesByImage(IOtcuBinarization otcuBinarization, IPreceptiveHash preceptiveHash, IHammingCode hammingCode)
        {
            OtcuBinarization = otcuBinarization;
            PreceptiveHash = preceptiveHash;
            HammingCode = hammingCode;
        }

        public double Compare(Color[,] imageFirst, Color[,] imageSecond)
        {
            var hashImageFirst = CalculatePreceptiveHash(imageFirst);
            var hashImageSecond = CalculatePreceptiveHash(imageSecond);
            var numberDifferentBits = HammingCode.Calc(hashImageFirst, hashImageSecond);
            return System.Math.Round(1 - numberDifferentBits / 64.0, 4);
        }

        public Dictionary<KeyValuePair<int, int>, double> Compare(List<Color[,]> imagesForComparison)
        {
            var reversibleKeys = new Dictionary<KeyValuePair<int, int>, bool>();
            var result =
                new Dictionary<KeyValuePair<int, int>, double>();
            var hashes = imagesForComparison.Select(CalculatePreceptiveHash).ToList();
            for (var i = 0; i < hashes.Count; i++)
                for (var j = 0; j < hashes.Count; j++)
                {
                    if (i == j) continue;
                    var verifyingKey = new KeyValuePair<int, int>(i, j);
                    if (reversibleKeys.ContainsKey(verifyingKey)) continue;
                    var numberDifferentBits = HammingCode.Calc(hashes[i], hashes[j]);
                    result.Add(new KeyValuePair<int, int>(i, j), System.Math.Round(1 - numberDifferentBits / 64.0, 4));
                    var reverseKey = new KeyValuePair<int, int>(j, i);
                    reversibleKeys[reverseKey] = true;
                }
            return result;
        }

        private ulong CalculatePreceptiveHash(Color[,] image)
        {
            var threshold = OtcuBinarization.CalcThreshold(image);
            var binarization = OtcuBinarization.Binarization(image, threshold);
            return PreceptiveHash.Get(binarization, threshold);
        }

        protected IOtcuBinarization OtcuBinarization { get; set; }
        protected IPreceptiveHash PreceptiveHash { get; set; }
        protected IHammingCode HammingCode { get; set; }
    }
}


namespace Tests
{
    public class PreceptiveHashMeasureСomparisonImagesByImage : BaseMeasureСomparisonImages<Math.Kernel.Algorithms.Transformations.Сomparisons.PreceptiveHashMeasureСomparisonImagesByImage>
    {

        [Test, TestCaseSource(nameof(GetDataForTestFirst))]
        public void CompareTestFirst(string pictureFileNameFirst, string pictureFileNameSecond, double threshold)
        {
            var result = CompareTestFirst(pictureFileNameFirst, pictureFileNameSecond, 8, 8);
            this.Verify(() => System.Math.Abs(result - threshold) < 0.0001, $"variable result is not valid, Expected: {threshold}, But was: {result}");
        }

        [Test, TestCaseSource(nameof(GetDataForTestSecond))]
        public string CompareTestSecond(List<string> pictureFileNames)
        {
            return CompareTestSecond(pictureFileNames, 8, 8);
        }
        public static IEnumerable GetDataForTestFirst()
        {
            return new List<TestCaseData>
            {
                new TestCaseData("figure1.bmp","figure1.bmp", 1.0),
                new TestCaseData("figure7.bmp","figure7.bmp", 1.0),
                new TestCaseData("picture1.bmp","picture1.bmp", 1.0),
                new TestCaseData("picture6.bmp","picture6.bmp", 1.0),
                new TestCaseData("figure1.bmp","figure6.bmp", 0.9375),
                new TestCaseData("figure1.bmp","figure7.bmp", 0.0625),
                new TestCaseData("picture1.bmp","picture6.bmp", 0.8281),
                new TestCaseData("picture1.bmp","picture7.bmp", 0.7031),
                new TestCaseData("picture6.bmp","picture7.bmp", 0.7812),
                new TestCaseData("figure1.bmp","picture1.bmp", 0.5312),
                new TestCaseData("figure1.bmp","picture6.bmp", 0.4844),
                new TestCaseData("figure7.bmp","picture6.bmp", 0.5156)
            };
        }

        public static IEnumerable GetDataForTestSecond()
        {
            return new List<TestCaseData>
            {
                new TestCaseData(new List<string>
                {
                    "figure1.bmp",
                    "figure2.bmp",
                    "figure3.bmp",
                    "figure5.bmp",
                    "figure6.bmp",
                    "figure7.bmp",
                    "picture1.bmp",
                    "picture2.bmp",
                    "picture3.bmp",
                    "picture4.bmp",
                    "picture5.bmp",
                    "picture6.bmp",
                    "picture7.bmp"
                }).Returns("{\"[figure1.bmp, figure2.bmp]\":0.9844,\"[figure1.bmp, figure3.bmp]\":0.9375,\"[figure1.bmp, figure5.bmp]\":0.9844,\"[figure1.bmp, figure6.bmp]\":0.9375,\"[figure1.bmp, figure7.bmp]\":0.0625,\"[figure1.bmp, picture1.bmp]\":0.5312,\"[figure1.bmp, picture2.bmp]\":0.5156,\"[figure1.bmp, picture3.bmp]\":0.5625,\"[figure1.bmp, picture4.bmp]\":0.5156,\"[figure1.bmp, picture5.bmp]\":0.5469,\"[figure1.bmp, picture6.bmp]\":0.4844,\"[figure1.bmp, picture7.bmp]\":0.5781,\"[figure2.bmp, figure3.bmp]\":0.9219,\"[figure2.bmp, figure5.bmp]\":0.9688,\"[figure2.bmp, figure6.bmp]\":0.9531,\"[figure2.bmp, figure7.bmp]\":0.0469,\"[figure2.bmp, picture1.bmp]\":0.5469,\"[figure2.bmp, picture2.bmp]\":0.5312,\"[figure2.bmp, picture3.bmp]\":0.5781,\"[figure2.bmp, picture4.bmp]\":0.5312,\"[figure2.bmp, picture5.bmp]\":0.5625,\"[figure2.bmp, picture6.bmp]\":0.5,\"[figure2.bmp, picture7.bmp]\":0.5625,\"[figure3.bmp, figure5.bmp]\":0.9531,\"[figure3.bmp, figure6.bmp]\":0.875,\"[figure3.bmp, figure7.bmp]\":0.0938,\"[figure3.bmp, picture1.bmp]\":0.5312,\"[figure3.bmp, picture2.bmp]\":0.5156,\"[figure3.bmp, picture3.bmp]\":0.5625,\"[figure3.bmp, picture4.bmp]\":0.5156,\"[figure3.bmp, picture5.bmp]\":0.5469,\"[figure3.bmp, picture6.bmp]\":0.4844,\"[figure3.bmp, picture7.bmp]\":0.5781,\"[figure5.bmp, figure6.bmp]\":0.9219,\"[figure5.bmp, figure7.bmp]\":0.0781,\"[figure5.bmp, picture1.bmp]\":0.5469,\"[figure5.bmp, picture2.bmp]\":0.5312,\"[figure5.bmp, picture3.bmp]\":0.5781,\"[figure5.bmp, picture4.bmp]\":0.5312,\"[figure5.bmp, picture5.bmp]\":0.5625,\"[figure5.bmp, picture6.bmp]\":0.5,\"[figure5.bmp, picture7.bmp]\":0.5938,\"[figure6.bmp, figure7.bmp]\":0.0625,\"[figure6.bmp, picture1.bmp]\":0.5312,\"[figure6.bmp, picture2.bmp]\":0.5156,\"[figure6.bmp, picture3.bmp]\":0.5625,\"[figure6.bmp, picture4.bmp]\":0.5156,\"[figure6.bmp, picture5.bmp]\":0.5469,\"[figure6.bmp, picture6.bmp]\":0.4844,\"[figure6.bmp, picture7.bmp]\":0.5469,\"[figure7.bmp, picture1.bmp]\":0.4688,\"[figure7.bmp, picture2.bmp]\":0.4844,\"[figure7.bmp, picture3.bmp]\":0.4375,\"[figure7.bmp, picture4.bmp]\":0.4844,\"[figure7.bmp, picture5.bmp]\":0.4531,\"[figure7.bmp, picture6.bmp]\":0.5156,\"[figure7.bmp, picture7.bmp]\":0.4844,\"[picture1.bmp, picture2.bmp]\":0.9219,\"[picture1.bmp, picture3.bmp]\":0.9688,\"[picture1.bmp, picture4.bmp]\":0.9844,\"[picture1.bmp, picture5.bmp]\":0.9844,\"[picture1.bmp, picture6.bmp]\":0.8281,\"[picture1.bmp, picture7.bmp]\":0.7031,\"[picture2.bmp, picture3.bmp]\":0.9219,\"[picture2.bmp, picture4.bmp]\":0.9062,\"[picture2.bmp, picture5.bmp]\":0.9062,\"[picture2.bmp, picture6.bmp]\":0.9062,\"[picture2.bmp, picture7.bmp]\":0.75,\"[picture3.bmp, picture4.bmp]\":0.9531,\"[picture3.bmp, picture5.bmp]\":0.9844,\"[picture3.bmp, picture6.bmp]\":0.8281,\"[picture3.bmp, picture7.bmp]\":0.7031,\"[picture4.bmp, picture5.bmp]\":0.9688,\"[picture4.bmp, picture6.bmp]\":0.8125,\"[picture4.bmp, picture7.bmp]\":0.6875,\"[picture5.bmp, picture6.bmp]\":0.8125,\"[picture5.bmp, picture7.bmp]\":0.6875,\"[picture6.bmp, picture7.bmp]\":0.7812}")
            };
        }
    }

}


