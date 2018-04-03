namespace Project.Kernel
{
    public interface IInit
    {
        void Init();
    }

    public interface IInit<in TArg>
    {
        void Init(TArg arg);
    }

    public interface IInit<in TArgFirst, in TArgSecond>
    {
        void Init(TArgFirst argFirst, TArgSecond argSecond);
    }

    public interface IInit<in TArgFirst, in TArgSecond, in TArgThird>
    {
        void Init(TArgFirst argFirst, TArgSecond argSecond, TArgThird argThird);
    }
}
