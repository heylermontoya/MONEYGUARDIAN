using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.user.Commands
{
    public record UpdateUserCommand(
        int Id,
        string Name,
        string UserName,
        string? Password
    ) : IRequest<UserDto>;
}
