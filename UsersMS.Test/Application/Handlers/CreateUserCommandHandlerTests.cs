using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Commons.Dtos.Request; // <- importante para CreateUserDto
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;

public class CreateUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IKeycloakService> _keycloakServiceMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly CreateUserCommandHandler _handler;

    public CreateUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _keycloakServiceMock = new Mock<IKeycloakService>();
        _eventPublisherMock = new Mock<IEventPublisher>();

        _handler = new CreateUserCommandHandler(
            _userRepositoryMock.Object,
            _keycloakServiceMock.Object,
            _eventPublisherMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldCreateUser_WhenCommandIsValid()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Name = "Juan",
            LastName = "Pérez",
            Email = "juan@example.com",
            Password = "1234",
            Role = UserRole.Administrador
        };

        var command = new CreateUserCommand(dto);

        _userRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask); 
        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(r => r.AddAsync(It.Is<User>(
            u => u.Name == dto.Name && u.Email == dto.Email
        )), Times.Once);

        _eventPublisherMock.Verify(e => e.PublishUserCreatedAsync(It.IsAny<User>()), Times.Once);
    }  

}

