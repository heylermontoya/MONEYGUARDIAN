using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.user.Queries
{
    public record GetUserByIdQuery(
        int Id
    ) : IRequest<UserDto>;
}
