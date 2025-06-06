using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.budget.Commands;
using MONEY_GUARDIAN.Application.Feature.budget.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateBudgetAsync(CreateBudgetCommand command)
        {
            BudgetDto budgetDto = await mediator.Send(command);

            return new CreatedResult($"Budget/{budgetDto.Id}", budgetDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBudgetAsync(UpdateBudgetCommand command)
        {
            BudgetDto budgetDto = await mediator.Send(command);

            return new OkObjectResult(budgetDto);
        }

        [HttpGet("BudgetById/{id}")]
        public async Task<IActionResult> GetBudgetById(int id)
        {
            BudgetDto budgetDto = await mediator.Send(
                 new GetBudgetByIdQuery(id)
             );

            return new OkObjectResult(budgetDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListBudgetAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<BudgetDto> listBudgetDto = await mediator.Send(
                new GetListBudgetQuery(fieldFilter)
            );

            return new OkObjectResult(listBudgetDto);
        }
    }
}
