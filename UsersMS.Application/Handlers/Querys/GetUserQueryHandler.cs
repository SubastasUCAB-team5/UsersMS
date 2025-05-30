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
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<GetUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetByIdAsync(request.UserId);

            if (user == null)
                throw new UserNotFoundException("User not found.");

            return new GetUserDto
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
            };
        }
    }
}

