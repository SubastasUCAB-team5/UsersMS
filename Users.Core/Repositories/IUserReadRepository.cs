using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UsersMS.Domain.Entities;

namespace UsersMS.Core.Repositories
{
    public interface IUserReadRepository
    {
        Task<UserReadModel?> GetByIdAsync(Guid id);
        Task<List<UserReadModel>> GetAllAsync();
    }
}
