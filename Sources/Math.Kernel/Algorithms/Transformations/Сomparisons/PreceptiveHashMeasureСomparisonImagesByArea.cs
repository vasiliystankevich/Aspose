using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Math.Kernel.Algorithms.Transformations.Filters;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Project.Kernel.NUnit;

namespace Math.Kernel.Algorithms.Transformations.Сomparisons
{
    public interface IPreceptiveHashMeasureСomparisonImagesByArea { }
    public interface ILowHighMeasureComparasionImages { }
    public abstract class BasePreceptiveHashMeasureСomparisonImagesByArea<TName> : IMeasureСomparisonImages<TName>
    {
        protected BasePreceptiveHashMeasureСomparisonImagesByArea(IHammingCode hammingCode)
        {
            HammingCode = hammingCode;
        }

        public double Compare(Color[,] imageFirst, Color[,] imageSecond)
        {
            var firstImageHashes = CalculatePreceptiveHashesByImage(imageFirst);
            var secondImageHashes = CalculatePreceptiveHashesByImage(imageSecond);
            return CalculateTheMeasureCoincidence(firstImageHashes, secondImageHashes);
        }

        public Dictionary<KeyValuePair<int, int>, double> Compare(List<Color[,]> imagesForComparison)
        {
            var reversibleKeys = new Dictionary<KeyValuePair<int, int>, bool>();
            var result = new Dictionary<KeyValuePair<int, int>, double>();
            var hashes = imagesForComparison.Select(CalculatePreceptiveHashesByImage).ToList();
            for (var i = 0; i < hashes.Count; i++)
            for (var j = 0; j < hashes.Count; j++)
            {
                if (i == j) continue;
                var verifyingKey = new KeyValuePair<int, int>(i, j);
                if (reversibleKeys.ContainsKey(verifyingKey)) continue;
                var measure = CalculateTheMeasureCoincidence(hashes[i], hashes[j]);
                result.Add(new KeyValuePair<int, int>(i, j), measure);
                var reverseKey = new KeyValuePair<int, int>(j, i);
                reversibleKeys[reverseKey] = true;
            }
            return result;
        }

        private double CalculateTheMeasureCoincidence(IReadOnlyList<ulong> firstImageHashes, IReadOnlyList<ulong> secondImageHashes)
        {
            var summaMeasures = 0.0;
            for (var i = 0; i < firstImageHashes.Count; i++)
                summaMeasures += HammingCode.Calc(firstImageHashes[i], secondImageHashes[i]);
            return System.Math.Round(1 - summaMeasures / (firstImageHashes.Count * 64.0), 4);
        }

        protected abstract List<ulong> CalculatePreceptiveHashesByImage(Color[,] image);
        protected IHammingCode HammingCode { get; set; }

    }
    public class PreceptiveHashMeasureСomparisonImagesByArea: BasePreceptiveHashMeasureСomparisonImagesByArea<IPreceptiveHashMeasureСomparisonImagesByArea>
    {
        public PreceptiveHashMeasureСomparisonImagesByArea(IOtcuBinarization otcuBinarization, IFilter<ISobel> sobelFilter, IPreceptiveHash preceptiveHash, IHammingCode hammingCode):base(hammingCode)
        {
            OtcuBinarization = otcuBinarization;
            SobelFilter = sobelFilter;
            PreceptiveHash = preceptiveHash;
        }


        protected override List<ulong> CalculatePreceptiveHashesByImage(Color[,] image)
        {
            var imageWithSuperimposedSobelFilter = SobelFilter.Transform(image);
            var threshold = OtcuBinarization.CalcThreshold(imageWithSuperimposedSobelFilter);
            return PreceptiveHash.ByImage(imageWithSuperimposedSobelFilter, threshold);
        }

        protected IOtcuBinarization OtcuBinarization { get; set; }
        protected IFilter<ISobel> SobelFilter { get; set; }
        protected IPreceptiveHash PreceptiveHash { get; set; }
    }

    public class LowHighMeasureComparasionImages : BasePreceptiveHashMeasureСomparisonImagesByArea<ILowHighMeasureComparasionImages>
    {
        public LowHighMeasureComparasionImages(IOtcuBinarization otcuBinarization, IFilter<IMaskedImageTransform> maskedImageTransform, IPreceptiveHash preceptiveHash, IHammingCode hammingCode) : base(hammingCode)
        {
            OtcuBinarization = otcuBinarization;
            MaskedImageTransform = maskedImageTransform;
            PreceptiveHash = preceptiveHash;
        }

        protected override List<ulong> CalculatePreceptiveHashesByImage(Color[,] image)
        {
            var maskedImage = MaskedImageTransform.Transform(image);
            var threshold = OtcuBinarization.CalcThreshold(maskedImage);
            return PreceptiveHash.ByImage(maskedImage, threshold);
        }

        protected IOtcuBinarization OtcuBinarization { get; set; }
        protected IFilter<IMaskedImageTransform> MaskedImageTransform { get; set; }
        protected IPreceptiveHash PreceptiveHash { get; set; }
    }
}

namespace Tests
{
    public class PreceptiveHashMeasureСomparisonImagesByArea : BaseMeasureСomparisonImages<Math.Kernel.Algorithms.Transformations.Сomparisons.PreceptiveHashMeasureСomparisonImagesByArea>
    {
        [Test, TestCaseSource(nameof(GetDataForTestFirst))]
        public void CompareTestFirst(string pictureFileNameFirst, string pictureFileNameSecond, double threshold)
        {
            var result=CompareTestFirst(pictureFileNameFirst, pictureFileNameSecond, 128, 128);
            this.Verify(() => System.Math.Abs(result - threshold) < 0.0001, $"variable result is not valid, Expected: {threshold}, But was: {result}");
        }

        [Test, TestCaseSource(nameof(GetDataForTestSecond))]
        public string CompareTestSecond(List<string> pictureFileNames)
        {
            return CompareTestSecond(pictureFileNames, 128, 128);
        }

        public static IEnumerable GetDataForTestFirst()
        {
            return new List<TestCaseData>
            {
                new TestCaseData("figure1.bmp","figure1.bmp", 1.0),
                new TestCaseData("figure7.bmp","figure7.bmp", 1.0),
                new TestCaseData("picture1.bmp","picture1.bmp", 1.0),
                new TestCaseData("picture6.bmp","picture6.bmp", 1.0),
                new TestCaseData("figure1.bmp","figure6.bmp", 0.9530),
                new TestCaseData("figure1.bmp","figure7.bmp", 0.9067),
                new TestCaseData("picture1.bmp","picture6.bmp", 0.9714),
                new TestCaseData("picture1.bmp","picture7.bmp", 0.7211),
                new TestCaseData("picture6.bmp","picture7.bmp", 0.7211),
                new TestCaseData("figure1.bmp","picture1.bmp", 0.7430),
                new TestCaseData("figure1.bmp","picture6.bmp", 0.7417),
                new TestCaseData("figure7.bmp","picture6.bmp", 0.7201)
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
                }).Returns("{\"[figure1.bmp, figure2.bmp]\":0.9907,\"[figure1.bmp, figure3.bmp]\":0.9164,\"[figure1.bmp, figure5.bmp]\":0.9688,\"[figure1.bmp, figure6.bmp]\":0.953,\"[figure1.bmp, figure7.bmp]\":0.9067,\"[figure1.bmp, picture1.bmp]\":0.743,\"[figure1.bmp, picture2.bmp]\":0.7476,\"[figure1.bmp, picture3.bmp]\":0.741,\"[figure1.bmp, picture4.bmp]\":0.744,\"[figure1.bmp, picture5.bmp]\":0.7419,\"[figure1.bmp, picture6.bmp]\":0.7418,\"[figure1.bmp, picture7.bmp]\":0.7766,\"[figure2.bmp, figure3.bmp]\":0.9161,\"[figure2.bmp, figure5.bmp]\":0.9668,\"[figure2.bmp, figure6.bmp]\":0.9522,\"[figure2.bmp, figure7.bmp]\":0.9035,\"[figure2.bmp, picture1.bmp]\":0.7429,\"[figure2.bmp, picture2.bmp]\":0.746,\"[figure2.bmp, picture3.bmp]\":0.7404,\"[figure2.bmp, picture4.bmp]\":0.7434,\"[figure2.bmp, picture5.bmp]\":0.7418,\"[figure2.bmp, picture6.bmp]\":0.7418,\"[figure2.bmp, picture7.bmp]\":0.776,\"[figure3.bmp, figure5.bmp]\":0.9271,\"[figure3.bmp, figure6.bmp]\":0.9099,\"[figure3.bmp, figure7.bmp]\":0.8901,\"[figure3.bmp, picture1.bmp]\":0.7369,\"[figure3.bmp, picture2.bmp]\":0.7398,\"[figure3.bmp, picture3.bmp]\":0.7355,\"[figure3.bmp, picture4.bmp]\":0.7372,\"[figure3.bmp, picture5.bmp]\":0.736,\"[figure3.bmp, picture6.bmp]\":0.7341,\"[figure3.bmp, picture7.bmp]\":0.7676,\"[figure5.bmp, figure6.bmp]\":0.949,\"[figure5.bmp, figure7.bmp]\":0.9052,\"[figure5.bmp, picture1.bmp]\":0.7416,\"[figure5.bmp, picture2.bmp]\":0.7471,\"[figure5.bmp, picture3.bmp]\":0.7397,\"[figure5.bmp, picture4.bmp]\":0.7419,\"[figure5.bmp, picture5.bmp]\":0.7403,\"[figure5.bmp, picture6.bmp]\":0.7404,\"[figure5.bmp, picture7.bmp]\":0.7759,\"[figure6.bmp, figure7.bmp]\":0.9103,\"[figure6.bmp, picture1.bmp]\":0.7382,\"[figure6.bmp, picture2.bmp]\":0.7449,\"[figure6.bmp, picture3.bmp]\":0.7353,\"[figure6.bmp, picture4.bmp]\":0.7383,\"[figure6.bmp, picture5.bmp]\":0.7366,\"[figure6.bmp, picture6.bmp]\":0.7365,\"[figure6.bmp, picture7.bmp]\":0.7736,\"[figure7.bmp, picture1.bmp]\":0.7217,\"[figure7.bmp, picture2.bmp]\":0.7256,\"[figure7.bmp, picture3.bmp]\":0.7167,\"[figure7.bmp, picture4.bmp]\":0.7226,\"[figure7.bmp, picture5.bmp]\":0.7201,\"[figure7.bmp, picture6.bmp]\":0.7202,\"[figure7.bmp, picture7.bmp]\":0.7573,\"[picture1.bmp, picture2.bmp]\":0.8253,\"[picture1.bmp, picture3.bmp]\":0.9112,\"[picture1.bmp, picture4.bmp]\":0.9722,\"[picture1.bmp, picture5.bmp]\":0.994,\"[picture1.bmp, picture6.bmp]\":0.9715,\"[picture1.bmp, picture7.bmp]\":0.7211,\"[picture2.bmp, picture3.bmp]\":0.8212,\"[picture2.bmp, picture4.bmp]\":0.8245,\"[picture2.bmp, picture5.bmp]\":0.8221,\"[picture2.bmp, picture6.bmp]\":0.827,\"[picture2.bmp, picture7.bmp]\":0.7244,\"[picture3.bmp, picture4.bmp]\":0.9108,\"[picture3.bmp, picture5.bmp]\":0.9097,\"[picture3.bmp, picture6.bmp]\":0.9106,\"[picture3.bmp, picture7.bmp]\":0.7211,\"[picture4.bmp, picture5.bmp]\":0.9666,\"[picture4.bmp, picture6.bmp]\":0.9589,\"[picture4.bmp, picture7.bmp]\":0.7196,\"[picture5.bmp, picture6.bmp]\":0.9662,\"[picture5.bmp, picture7.bmp]\":0.7194,\"[picture6.bmp, picture7.bmp]\":0.7212}")
            };
        }
    }

    public class LowHighMeasureComparasionImages: BaseMeasureСomparisonImages<Math.Kernel.Algorithms.Transformations.Сomparisons.LowHighMeasureComparasionImages>
    {
        [Test, TestCaseSource(nameof(GetDataForTestFirst))]
        public void CompareTestFirst(string pictureFileNameFirst, string pictureFileNameSecond, double threshold)
        {
            var result = CompareTestFirst(pictureFileNameFirst, pictureFileNameSecond, 128, 128);
            this.Verify(() => System.Math.Abs(result - threshold) < 0.0001, $"variable result is not valid, Expected: {threshold}, But was: {result}");
        }

        [Test, TestCaseSource(nameof(GetDataForTestSecond))]
        public string CompareTestSecond(List<string> pictureFileNames)
        {
            return CompareTestSecond(pictureFileNames, 128, 128);
        }

        public static IEnumerable GetDataForTestFirst()
        {
            return new List<TestCaseData>
            {
                new TestCaseData("figure1.bmp","figure1.bmp", 1.0),
                new TestCaseData("figure7.bmp","figure7.bmp", 1.0),
                new TestCaseData("picture1.bmp","picture1.bmp", 1.0),
                new TestCaseData("picture6.bmp","picture6.bmp", 1.0),
                new TestCaseData("figure1.bmp","figure6.bmp", 0.9701),
                new TestCaseData("figure1.bmp","figure7.bmp", 0.035),
                new TestCaseData("picture1.bmp","picture6.bmp", 0.9581),
                new TestCaseData("picture1.bmp","picture7.bmp", 0.7617),
                new TestCaseData("picture6.bmp","picture7.bmp", 0.7504),
                new TestCaseData("figure1.bmp","picture1.bmp", 0.7982),
                new TestCaseData("figure1.bmp","picture6.bmp", 0.7802),
                new TestCaseData("figure7.bmp","picture6.bmp", 0.2491)
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
                }).Returns("{\"[figure1.bmp, figure2.bmp]\":0.9979,\"[figure1.bmp, figure3.bmp]\":0.9276,\"[figure1.bmp, figure5.bmp]\":0.9852,\"[figure1.bmp, figure6.bmp]\":0.9701,\"[figure1.bmp, figure7.bmp]\":0.035,\"[figure1.bmp, picture1.bmp]\":0.7982,\"[figure1.bmp, picture2.bmp]\":0.8053,\"[figure1.bmp, picture3.bmp]\":0.7962,\"[figure1.bmp, picture4.bmp]\":0.7992,\"[figure1.bmp, picture5.bmp]\":0.7968,\"[figure1.bmp, picture6.bmp]\":0.7802,\"[figure1.bmp, picture7.bmp]\":0.8241,\"[figure2.bmp, figure3.bmp]\":0.9271,\"[figure2.bmp, figure5.bmp]\":0.9845,\"[figure2.bmp, figure6.bmp]\":0.97,\"[figure2.bmp, figure7.bmp]\":0.0346,\"[figure2.bmp, picture1.bmp]\":0.799,\"[figure2.bmp, picture2.bmp]\":0.8059,\"[figure2.bmp, picture3.bmp]\":0.7968,\"[figure2.bmp, picture4.bmp]\":0.7999,\"[figure2.bmp, picture5.bmp]\":0.7975,\"[figure2.bmp, picture6.bmp]\":0.7809,\"[figure2.bmp, picture7.bmp]\":0.8245,\"[figure3.bmp, figure5.bmp]\":0.9369,\"[figure3.bmp, figure6.bmp]\":0.9192,\"[figure3.bmp, figure7.bmp]\":0.1021,\"[figure3.bmp, picture1.bmp]\":0.7919,\"[figure3.bmp, picture2.bmp]\":0.799,\"[figure3.bmp, picture3.bmp]\":0.7897,\"[figure3.bmp, picture4.bmp]\":0.7927,\"[figure3.bmp, picture5.bmp]\":0.7903,\"[figure3.bmp, picture6.bmp]\":0.7739,\"[figure3.bmp, picture7.bmp]\":0.8229,\"[figure5.bmp, figure6.bmp]\":0.9674,\"[figure5.bmp, figure7.bmp]\":0.0471,\"[figure5.bmp, picture1.bmp]\":0.7977,\"[figure5.bmp, picture2.bmp]\":0.8032,\"[figure5.bmp, picture3.bmp]\":0.7961,\"[figure5.bmp, picture4.bmp]\":0.7987,\"[figure5.bmp, picture5.bmp]\":0.7963,\"[figure5.bmp, picture6.bmp]\":0.7797,\"[figure5.bmp, picture7.bmp]\":0.8234,\"[figure6.bmp, figure7.bmp]\":0.0598,\"[figure6.bmp, picture1.bmp]\":0.7994,\"[figure6.bmp, picture2.bmp]\":0.8068,\"[figure6.bmp, picture3.bmp]\":0.7974,\"[figure6.bmp, picture4.bmp]\":0.8003,\"[figure6.bmp, picture5.bmp]\":0.7983,\"[figure6.bmp, picture6.bmp]\":0.7812,\"[figure6.bmp, picture7.bmp]\":0.8267,\"[figure7.bmp, picture1.bmp]\":0.2308,\"[figure7.bmp, picture2.bmp]\":0.2239,\"[figure7.bmp, picture3.bmp]\":0.2329,\"[figure7.bmp, picture4.bmp]\":0.2298,\"[figure7.bmp, picture5.bmp]\":0.2322,\"[figure7.bmp, picture6.bmp]\":0.2491,\"[figure7.bmp, picture7.bmp]\":0.2039,\"[picture1.bmp, picture2.bmp]\":0.8472,\"[picture1.bmp, picture3.bmp]\":0.959,\"[picture1.bmp, picture4.bmp]\":0.9917,\"[picture1.bmp, picture5.bmp]\":0.9964,\"[picture1.bmp, picture6.bmp]\":0.9581,\"[picture1.bmp, picture7.bmp]\":0.7617,\"[picture2.bmp, picture3.bmp]\":0.8475,\"[picture2.bmp, picture4.bmp]\":0.8453,\"[picture2.bmp, picture5.bmp]\":0.8453,\"[picture2.bmp, picture6.bmp]\":0.8253,\"[picture2.bmp, picture7.bmp]\":0.7694,\"[picture3.bmp, picture4.bmp]\":0.9576,\"[picture3.bmp, picture5.bmp]\":0.9587,\"[picture3.bmp, picture6.bmp]\":0.928,\"[picture3.bmp, picture7.bmp]\":0.7619,\"[picture4.bmp, picture5.bmp]\":0.9883,\"[picture4.bmp, picture6.bmp]\":0.9557,\"[picture4.bmp, picture7.bmp]\":0.7615,\"[picture5.bmp, picture6.bmp]\":0.9548,\"[picture5.bmp, picture7.bmp]\":0.7601,\"[picture6.bmp, picture7.bmp]\":0.7504}")
            };
        }
    }
}