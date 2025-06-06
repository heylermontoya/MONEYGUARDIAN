using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.deposit.Commands;
using MONEY_GUARDIAN.Application.Feature.deposit.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepositController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateDepositAsync(CreateDepositCommand command)
        {
            DepositDto depositDto = await mediator.Send(command);

            return new CreatedResult($"Deposit/{depositDto.Id}", depositDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepositAsync(UpdateDepositCommand command)
        {
            DepositDto depositDto = await mediator.Send(command);

            return new OkObjectResult(depositDto);
        }

        [HttpGet("DepositById/{id}")]
        public async Task<IActionResult> GetDepositById(int id)
        {
            DepositDto depositDto = await mediator.Send(
                 new GetDepositByIdQuery(id)
             );

            return new OkObjectResult(depositDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListDepositAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<DepositDto> listDepositDto = await mediator.Send(
                new GetListDepositQuery(fieldFilter)
            );

            return new OkObjectResult(listDepositDto);
        }
    }
}
