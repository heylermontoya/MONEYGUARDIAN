using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.expenseType.Commands;
using MONEY_GUARDIAN.Application.Feature.expenseType.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseTypeController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateExpenseTypeAsync(CreateExpenseTypeCommand command)
        {
            ExpenseTypeDto expenseTypeDto = await mediator.Send(command);

            return new CreatedResult($"ExpenseType/{expenseTypeDto.Id}", expenseTypeDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpenseTypeAsync(UpdateExpenseTypeCommand command)
        {
            ExpenseTypeDto expenseTypeDto = await mediator.Send(command);

            return new OkObjectResult(expenseTypeDto);
        }

        [HttpGet("ExpenseTypeById/{id}")]
        public async Task<IActionResult> GetExpenseTypeById(int id)
        {
            ExpenseTypeDto expenseTypeDto = await mediator.Send(
                 new GetExpenseTypeByIdQuery(id)
             );

            return new OkObjectResult(expenseTypeDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListExpenseTypeAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ExpenseTypeDto> listExpenseTypeDto = await mediator.Send(
                new GetListExpenseTypeQuery(fieldFilter)
            );

            return new OkObjectResult(listExpenseTypeDto);
        }
    }
}
