using System;
using System.Drawing;
using System.Drawing.Imaging;
using Math.Kernel.Algorithms;
using Math.Kernel.NUnit;
using NUnit.Framework;

namespace Math.Kernel.Algorithms
{
    public interface ILockBitsImage
    {
        void Work(Bitmap image, Action<BitmapData> action);
        TResult Work<TResult>(Bitmap image, Func<BitmapData, TResult> functor);
    }

    public class LockBitsImage: ILockBitsImage
    {
        public void Work(Bitmap image, Action<BitmapData> action)
        {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var bitmapData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            action(bitmapData);
            image.UnlockBits(bitmapData);
        }

        public TResult Work<TResult>(Bitmap image, Func<BitmapData, TResult> functor)
        {
            var rect = new Rectangle(0, 0, image.Width, image.Height);
            var bitmapData = image.LockBits(rect, ImageLockMode.ReadWrite, image.PixelFormat);
            var result = functor(bitmapData);
            image.UnlockBits(bitmapData);
            return result;
        }
    }
}

namespace Tests
{
    public class LockBitsImage : BaseMathKernelInstance<ILockBitsImage>
    {
        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void WorkAction(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            {
                Instance.Work(baseImage, data => { });
            }
        }

        [Test, TestCaseSource(nameof(GetTestPictures))]
        public void WorkFunctor(string pictureFileName)
        {
            using (var baseImage = DalContext.FindPictureByFileName(pictureFileName))
            {
                var result = Instance.Work(baseImage, data => 42.0);
                Assert.AreEqual(42, result);
            }
        }

    }
}
