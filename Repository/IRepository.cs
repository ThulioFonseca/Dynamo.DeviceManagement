using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dynamo.DeviceManagement.Repository
{
    public interface IRepository<T>
    {
        Task<T?> GetByIdAsync(string id);
        Task<IEnumerable<T?>?> GetAllAsync();
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
