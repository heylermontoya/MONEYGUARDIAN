using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.monetaryFund.Commands;
using MONEY_GUARDIAN.Application.Feature.monetaryFund.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MonetaryFundController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateMonetaryFundAsync(CreateMonetaryFundCommand command)
        {
            MonetaryFundDto monetaryFundDto = await mediator.Send(command);

            return new CreatedResult($"MonetaryFund/{monetaryFundDto.Id}", monetaryFundDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMonetaryFundAsync(UpdateMonetaryFundCommand command)
        {
            MonetaryFundDto monetaryFundDto = await mediator.Send(command);

            return new OkObjectResult(monetaryFundDto);
        }

        [HttpGet("MonetaryFundById/{id}")]
        public async Task<IActionResult> GetMonetaryFundById(int id)
        {
            MonetaryFundDto monetaryFundDto = await mediator.Send(
                 new GetMonetaryFundByIdQuery(id)
             );

            return new OkObjectResult(monetaryFundDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListMonetaryFundAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<MonetaryFundDto> listMonetaryFundDto = await mediator.Send(
                new GetListMonetaryFundQuery(fieldFilter)
            );

            return new OkObjectResult(listMonetaryFundDto);
        }
    }
}
