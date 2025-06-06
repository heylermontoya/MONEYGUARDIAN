using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.user.Commands;
using MONEY_GUARDIAN.Application.Feature.user.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserCommand command)
        {
            UserDto userDto = await mediator.Send(command);

            return new CreatedResult($"User/{userDto.Id}", userDto);
        }

        [HttpPost("loginUser")]
        public async Task<IActionResult> LoginUserAsync(ValidLoginUserCommand command)
        {
            UserDto userDto = await mediator.Send(command);

            return new OkObjectResult(userDto);
        }

        [HttpPost("registerUserGoogle")]
        public async Task<IActionResult> RegisterUserGoogleAsync(RegisterUserGoogleCommand command)
        {
            UserDto userDto = await mediator.Send(command);

            return new OkObjectResult(userDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAsync(UpdateUserCommand command)
        {
            UserDto userDto = await mediator.Send(command);

            return new OkObjectResult(userDto);
        }

        [HttpGet("UserById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            UserDto userDto = await mediator.Send(
                 new GetUserByIdQuery(id)
             );

            return new OkObjectResult(userDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListUserAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<UserDto> listUserDto = await mediator.Send(
                new GetListUserQuery(fieldFilter)
            );

            return new OkObjectResult(listUserDto);
        }
    }
}
