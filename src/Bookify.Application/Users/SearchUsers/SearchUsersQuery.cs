using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.SearchUsers;

public sealed record SearchUsersQuery(string? SearchTerm, string? ExactMatch) : IQuery<IReadOnlyList<UserResponse>>;
