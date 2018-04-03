using System.Collections.Generic;
using Math.Kernel.Algorithms;
using Math.Kernel.NUnit;
using NUnit.Framework;
using Project.Kernel;
using Project.Kernel.NUnit;

namespace Math.Kernel.Algorithms
{
    public interface IHammingCode
    {
        ulong Calc(ulong value1, ulong value2);
    }

    public class HammingCode: IHammingCode
    {
        public ulong Calc(ulong value1, ulong value2)
        {
            ulong result = 0;
            do
            {
                var b1 = value1 % 2;
                var b2 = value2 % 2;
                value1 /= 2;
                value2 /= 2;
                result += (b1 + b2) % 2;
            } while (!(value1 == 0 && value2 == 0));
            return result;
        }
    }
}

namespace Tests
{
    public class HammingCode : BaseMathKernelInstance<IHammingCode>
    {
        [Test, TestCaseSource(nameof(HammingCodeTestData))]
        public ulong Calc(ulong value1, ulong value2)
        {
            return Instance.Calc(value1, value2);
        }

        private static IEnumerable<TestCaseData> HammingCodeTestData()
        {
            return new List<TestCaseData>
            {
                new TestCaseData(0xFFUL, 0xFFUL).Returns(0),
                new TestCaseData(0xFFUL, 0x7FUL).Returns(1),
                new TestCaseData(0xFFUL, 0x3FUL).Returns(2),
                new TestCaseData(0xFFUL, 0x1FUL).Returns(3),
                new TestCaseData(0xFFUL, 0x0FUL).Returns(4),
                new TestCaseData(0xFFUL, 0x07UL).Returns(5),
                new TestCaseData(0xFFUL, 0x03UL).Returns(6),
                new TestCaseData(0xFFUL, 0x01UL).Returns(7),
                new TestCaseData(0xFFUL, 0x00UL).Returns(8),
                new TestCaseData(0xAA55UL,0x55AAUL).Returns(16),
                new TestCaseData(0x55AA55UL,0xAA55AAUL).Returns(24),
                new TestCaseData(0xAA55AA55UL,0x55AA55AAUL).Returns(32)
            };
        }
    }

}
