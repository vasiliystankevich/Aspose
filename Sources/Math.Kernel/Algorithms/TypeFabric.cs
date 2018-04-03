using Project.Kernel;
using Unity;

namespace Math.Kernel.Algorithms
{
    public class TypeFabric:BaseTypeFabric
    {
        public override void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<IHammingCode, HammingCode>();
            container.RegisterType<ILockBitsImage, LockBitsImage>();
            container.RegisterType<IColorsMatrix, ColorsMatrix>();
            container.RegisterType<IPreceptiveHash, PreceptiveHash>();
        }
    }
}
