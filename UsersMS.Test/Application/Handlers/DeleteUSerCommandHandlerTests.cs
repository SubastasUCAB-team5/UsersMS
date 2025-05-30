using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using UsersMS.Application.Handlers.Commands;
using UsersMS.Application.Commands;
using UsersMS.Core.Repositories;
using UsersMS.Core.Service;
using UsersMS.Domain.Entities;
using UsersMS.Commons.Enums;
using UsersMS.Commons.Dtos.Request;

public class DeleteUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IKeycloakService> _keycloakServiceMock;
    private readonly Mock<IEventPublisher> _eventPublisherMock;
    private readonly DeleteUserCommandHandler _handler;

    public DeleteUserCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _keycloakServiceMock = new Mock<IKeycloakService>();
        _eventPublisherMock = new Mock<IEventPublisher>();

        _handler = new DeleteUserCommandHandler(
            _userRepositoryMock.Object,
            _keycloakServiceMock.Object,
            _eventPublisherMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldDeleteUserPhysically_AndPublishDeletedEvent()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Name = "Usuario",
            Email = "user@email.com",
            State = UserState.Active
        };

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(user);

        _userRepositoryMock
            .Setup(r => r.DeleteAsync(userId))
            .Returns(Task.CompletedTask);

        var dto = new DeleteUserDto { UserId = userId };

        // Act
        await _handler.Handle(new DeleteUserCommand(dto), CancellationToken.None);

        // Assert
        _userRepositoryMock.Verify(r => r.DeleteAsync(userId), Times.Once);
        _eventPublisherMock.Verify(e => e.PublishUserDeletedAsync(It.Is<User>(
            u => u.Id == userId && u.Email == user.Email
        )), Times.Once);
    }
}
