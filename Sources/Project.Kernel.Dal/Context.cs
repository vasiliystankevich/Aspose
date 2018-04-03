using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Kernel.Dal
{
    public interface IDbContext
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        void ExecuteTransaction(Action action);
    }

    public interface IExtendedFunctionDbContext
    {
        EnumValueModel FindEnumValue(string type, string name);
    }
}
