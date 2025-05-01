using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Domain.Entities;

namespace UsersMS.Application.Handlers.Commands
{
    public class CreateAdministratorCommandHandler : IRequestHandler<CreateAdministratorCommand, string>
    {
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IKeycloakService _keycloakService;
        public IEventBus

        public CreateAdministratorCommandHandler(IAdministratorRepository administratorRepository, IKeycloakService keycloakService)
        {
            Console.WriteLine("CreateAdministratorCommandHandler instantiated");

            _administratorRepository = administratorRepository;
            _keycloakService = keycloakService;
        }

        public async Task<string> Handle(CreateAdministratorCommand request, CancellationToken cancellationToken)
        {

            // Crear el administrator en la base de datos primero
            var administrator = new Administrator(
                request._createAdministratorDto.Email!,
                request._createAdministratorDto.Password!,
                request._createAdministratorDto.Id!,
                request._createAdministratorDto.Name!,
                request._createAdministratorDto.LastName!,
                request._createAdministratorDto.Phone!,
                request._createAdministratorDto.Address!,
                request._createAdministratorDto.Role,
                request._createAdministratorDto.State
            );

            await _administratorRepository.AddAsync(administrator);

            // Obtener el token de administrator de Keycloak
            Console.WriteLine("Fetching admin token from Keycloak");
            var token = await _keycloakService.GetAdminTokenAsync();

            // Crear el DTO para Keycloak con la información del administrator
            var userDto = new
            {
                username = administrator.Email,
                firstName = administrator.Name,
                lastName = administrator.LastName,
                email = administrator.Email,
                enabled = true,
                credentials = new[]
                {
                    new { type = "password", value = administrator.Password, temporary = false }
                },
            };

            await _keycloakService.CreateUserAsync(userDto, token);

            // Asignar el rol al usuario en Keycloak
            await _keycloakService.AssignRoleAsync(administrator.Email, request._createAdministratorDto.Role.ToString(), token);

            eventbus.push
            return "Administrator sucessfully created.";
        }
    }
}
