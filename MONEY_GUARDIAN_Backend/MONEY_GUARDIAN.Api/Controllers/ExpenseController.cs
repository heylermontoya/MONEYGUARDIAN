using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.expense.Commands;
using MONEY_GUARDIAN.Application.Feature.expense.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController(IMediator mediator)
    {
        [HttpPost]
        public async Task<IActionResult> CreateExpenseAsync(CreateExpenseCommand command)
        {
            ExpenseDto expenseDto = await mediator.Send(command);

            return new CreatedResult($"Expense/{expenseDto.Id}", expenseDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateExpenseAsync(UpdateExpenseCommand command)
        {
            ExpenseDto expenseDto = await mediator.Send(command);

            return new OkObjectResult(expenseDto);
        }

        [HttpGet("ExpenseById/{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            ExpenseDto expenseDto = await mediator.Send(
                 new GetExpenseByIdQuery(id)
             );

            return new OkObjectResult(expenseDto);
        }

        [HttpPost("list")]
        public async Task<IActionResult> ObtainListExpenseAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ExpenseDto> listExpenseDto = await mediator.Send(
                new GetListExpenseQuery(fieldFilter)
            );

            return new OkObjectResult(listExpenseDto);
        }
    }
}
