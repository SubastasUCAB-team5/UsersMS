using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersMS.Application.Queries;
using UsersMS.Commons.Dtos.Response;
using UsersMS.Core.Repositories;
using UsersMS.Infrastructure.Exceptions;

namespace UsersMS.Application.Handlers.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<GetAllUsersDto>>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetAllUsersQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<List<GetAllUsersDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userReadRepository.GetAllAsync();
            if (users == null) throw new UserNotFoundException("Users not found.");

            return users.Select(user => new GetAllUsersDto
            {
                UserId = user.Id!,
                Email = user.Email!,
                DocumentId = user.DocumentId!,
                Name = user.Name!,
                LastName = user.LastName!,
                Phone = user.Phone!,
                Address = user.Address!,
                Role = user.Role,
                State = user.State,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            }).ToList();
        }
    }
}

