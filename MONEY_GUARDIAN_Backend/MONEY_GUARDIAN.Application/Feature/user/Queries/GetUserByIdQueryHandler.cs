using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.user;

namespace MONEY_GUARDIAN.Application.Feature.user.Queries
{
    public class GetUserByIdQueryHandler(
        UserService service
    ) : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        public async Task<UserDto> Handle(
            GetUserByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            User user = await service.GetUserById(
                command.Id
            );

            return MapUserToUserDto(user);
        }

        private static UserDto MapUserToUserDto(User user)
        {
            return new UserDto()
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.Username
            };
        }
    }
}
