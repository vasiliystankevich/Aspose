namespace Project.Kernel
{
    public interface IRepository<TName, TRepository>
    {
        TRepository Repository { get; set; }
    }
}
