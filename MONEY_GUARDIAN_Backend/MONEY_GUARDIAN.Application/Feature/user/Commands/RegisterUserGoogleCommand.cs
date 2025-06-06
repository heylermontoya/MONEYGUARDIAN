using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.user.Commands
{
    public record RegisterUserGoogleCommand(
        string UserName
    ) : IRequest<UserDto>;
}
