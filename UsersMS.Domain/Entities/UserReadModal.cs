using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UsersMS.Domain.Entities;

public class UserReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string FullName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Role { get; set; } = default!;
}
