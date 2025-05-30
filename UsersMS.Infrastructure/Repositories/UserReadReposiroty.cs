using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using UsersMS.Core.Repositories;
using UsersMS.Domain.Entities;

namespace UsersMS.Infrastructure.Repositories
{
    public class UserReadRepository : IUserReadRepository
    {
        private readonly IMongoCollection<UserReadModel> _collection;

        public UserReadRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<UserReadModel>("Users");
        }

        public async Task<UserReadModel?> GetByIdAsync(Guid id)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<UserReadModel>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }
    }
}

