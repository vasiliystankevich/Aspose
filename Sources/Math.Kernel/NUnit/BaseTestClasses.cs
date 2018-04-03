using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dal;
using Math.Kernel.Algorithms;
using Math.Kernel.Algorithms.Transformations;
using Math.Kernel.Algorithms.Transformations.Filters;
using Math.Kernel.Algorithms.Transformations.Сomparisons;
using Newtonsoft.Json;
using NUnit.Framework;
using Project.Kernel;
using Project.Kernel.NUnit;
using Unity;
using Unity.Interception.Utilities;

namespace Math.Kernel.NUnit
{
    public abstract class BaseMathKernelInstance<T>: BaseTestInstance<T>
    {
        public override void RegisterTypes()
        {
            base.RegisterTypes();
            BaseTypeFabric.RegisterTypes<Dal.TypeFabric>(Container);
            BaseTypeFabric.RegisterTypes<TypeFabric>(Container);
        }

        public override void Init()
        {
            base.Init();
            DalContext = Container.Resolve<IDalContext>();
            DalContext.Scan();
        }

        public static IEnumerable GetTestPictures()
        {
            return new List<TestCaseData>
            {
                new TestCaseData("figure7.bmp"),
                new TestCaseData("picture6.bmp")
            };
        }

        protected IDalContext DalContext { get; set; }
    }

    public abstract class BaseFilterInstance<TName> : BaseMathKernelInstance<IFilter<TName>>
    {
        public override void Init()
        {
            base.Init();
            TransformImages = Container.Resolve<ITransformImages>();
            ColorsMatrix = Container.Resolve<IColorsMatrix>();
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void Apply(string pictureFileName)
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

    public abstract class BaseMeasureСomparisonImages<TName> : BaseMathKernelInstance<TName> where TName:IMeasureСomparisonImages
    {
        public override void Init()
        {
            base.Init();
            TransformImages = Container.Resolve<ITransformImages>();
            ColorsMatrix = Container.Resolve<IColorsMatrix>();
        }

        protected double CompareTestFirst(string pictureFileNameFirst, string pictureFileNameSecond, int newWidth, int newHeight)
        {
            var colorMatrixImageFirst = GetColorMatrix(pictureFileNameFirst, newWidth, newHeight);
            var colorMatrixImageSecond = GetColorMatrix(pictureFileNameSecond, newWidth, newHeight);
            return Instance.Compare(colorMatrixImageFirst, colorMatrixImageSecond);
        }

        protected string CompareTestSecond(List<string> pictureFileNames, int newWidth, int newHeight)
        {
            var testResult = new Dictionary<KeyValuePair<string, string>, double>();
            var imagesColorMatrixes = pictureFileNames.Select(pictureFileName => GetColorMatrix(pictureFileName, newWidth, newHeight)).ToList();
            Instance.Compare(imagesColorMatrixes).ForEach(element =>
            {
                var keyResult = new KeyValuePair<string, string>(pictureFileNames[element.Key.Key].ToLowerInvariant(),
                    pictureFileNames[element.Key.Value].ToLowerInvariant());
                testResult.Add(keyResult, element.Value);
            });
            return JsonConvert.SerializeObject(testResult);
        }
        protected Color[,] GetColorMatrix(string pictureFileName, int newWidth, int newHeight)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            using (var grayScale = TransformImages.GrayScale(baseImage, newWidth, newHeight))
                return ColorsMatrix.Get(grayScale);
        }
        protected ITransformImages TransformImages { get; set; }
        protected IColorsMatrix ColorsMatrix { get; set; }
    }
}
