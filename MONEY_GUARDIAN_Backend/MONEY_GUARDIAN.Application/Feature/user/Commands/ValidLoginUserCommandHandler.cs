using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.user;

namespace MONEY_GUARDIAN.Application.Feature.user.Commands
{
    public class ValidLoginUserCommandHandler(
        UserService service
    ) : IRequestHandler<ValidLoginUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(
            ValidLoginUserCommand command,
            CancellationToken cancellationToken
        )
        {
            User user = await service.ValidLoginUserAsync(
                command.UserName,
                command.Password
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
