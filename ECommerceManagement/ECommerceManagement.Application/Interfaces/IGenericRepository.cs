using ECommerceManagement.Domain.Common;
using System.Linq.Expressions;

namespace ECommerceManagement.Application.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetQueryable();

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
