using log4net;

namespace Project.Kernel
{
    public abstract class BaseRepository
    {
        protected BaseRepository(IWrapper<ILog> logger)
        {
            Logger = logger;
        }
        public IWrapper<ILog> Logger { get; set; }
    }
}