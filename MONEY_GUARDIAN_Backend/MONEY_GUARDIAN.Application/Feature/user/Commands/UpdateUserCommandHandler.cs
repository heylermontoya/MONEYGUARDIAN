using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.user;

namespace MONEY_GUARDIAN.Application.Feature.user.Commands
{
    public class UpdateUserCommandHandler(
        UserService service
    ) : IRequestHandler<UpdateUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(
            UpdateUserCommand command,
            CancellationToken cancellationToken
        )
        {
            User user = await service.UpdateUserAsync(
                command.Id,
                command.Name,
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
