using MassTransit;
using UsersMS.Domain.Entities;
using UsersMS.Infrastructure.DataBase;
using UsersMS.Commons.Events;

namespace UsersMS.Infrastructure.Messaging.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly MongoDbContext _mongo;

    public UserCreatedConsumer(MongoDbContext mongo)
    {
        _mongo = mongo;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var user = new UserReadModel
        {
            Id = context.Message.UserId,
            FullName = context.Message.FullName,
            Email = context.Message.Email,
            Role = context.Message.Role
        };

        await _mongo.Users.InsertOneAsync(user);
    }
}
