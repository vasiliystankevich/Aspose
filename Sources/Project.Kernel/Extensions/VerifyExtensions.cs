using System;
using System.Security;

namespace Project.Kernel.Extensions
{
    public static class VerifyExtensions
    {
        public static void IsNull(this object sender, string message)
        {
            sender.Verify(() => sender == null, message);
        }

        public static void IsNotNull(this object sender, Action action)
        {
            if (sender != null) action();
        }

        public static void Verify(this object sender, Func<bool> verifyFunctor, string message)
        {
            if (verifyFunctor()) throw new VerificationException(message);
        }

        public static void Verify<TVerifyObject>(this object sender, TVerifyObject verifyObject, Func<TVerifyObject, bool> verifyFunctor, string message)
        {
            if (verifyFunctor(verifyObject)) throw new VerificationException(message);
        }

        public static void Verify<TVerifyObjectFirst, TVerifyObjectSecond>(this object sender, TVerifyObjectFirst verifyObjectFirst, TVerifyObjectSecond verifyObjectSecond, Func<TVerifyObjectFirst, TVerifyObjectSecond, bool> verifyFunctor, string message)
        {
            if (verifyFunctor(verifyObjectFirst, verifyObjectSecond)) throw new VerificationException(message);
        }

        public static void Verify<TVerifyObjectFirst, TVerifyObjectSecond, TVerifyObjectThird>(this object sender, TVerifyObjectFirst verifyObjectFirst, TVerifyObjectSecond verifyObjectSecond, TVerifyObjectThird verifyObjectThird, Func<TVerifyObjectFirst, TVerifyObjectSecond, TVerifyObjectThird, bool> verifyFunctor, string message)
        {
            if (verifyFunctor(verifyObjectFirst, verifyObjectSecond, verifyObjectThird)) throw new VerificationException(message);
        }
    }
}
