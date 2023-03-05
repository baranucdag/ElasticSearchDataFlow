using ElasticSearchDataFlow.Data.Entites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ElasticSearchDataFlow.Data.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        DbSet<T> Table { get; }
        IQueryable<T> GetAll(bool tracking = true);
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> expression, bool tracking = true);
        Task<T> GetByIdAsync(string id, bool tracking = true);
        Task<bool> AddAsync(T model);
        Task<bool> AddRangeAsync(List<T> datas);
        bool AddRange(List<T> datas);
        bool Remove(T model);
        bool RemoveRange(List<T> datas);
        Task<bool> RemoveAsync(string id);
        bool Update(T model);

        Task<int> SaveAsync();
    }
}
