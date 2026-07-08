using ECommerceManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceManagement.Application.Interfaces.Repositories
{
    public interface IBrandRepository : IRepositoryBase<Brand>
    {
        Task<bool> IsNameExistsAsync(string name, Guid? excludeBrandId = null);
    }
}
