using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Commons.Dtos.Request;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;

public class UpdateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IKeycloakService> _keycloakServiceMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly UpdateUserCommandHandler _handler;

    public UpdateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _keycloakServiceMock = new Mock<IKeycloakService>();
        _eventPublisherMock = new Mock<IEventPublisher>();

        _handler = new UpdateUserCommandHandler(
            _userRepositoryMock.Object,
            _keycloakServiceMock.Object,
            _eventPublisherMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldUpdateUser_WhenCommandIsValid()
    {
        // Arrange
        var dto = new UpdateUserDto
        {
            UserId = Guid.NewGuid(),
            Name = "Nuevo Nombre",
            LastName = "Actualizado",
            Email = "nuevo@email.com",
            Role = UserRole.Administrador,
            Phone = "04141234567",
            Address = "Nueva dirección"
        };

        var command = new UpdateUserCommand(dto);

        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(dto.UserId))
            .ReturnsAsync(new User { Id = dto.UserId });

        _userRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(r => r.UpdateAsync(It.Is<User>(
            u => u.Name == dto.Name && u.Email == dto.Email
        )), Times.Once);
    }

}
