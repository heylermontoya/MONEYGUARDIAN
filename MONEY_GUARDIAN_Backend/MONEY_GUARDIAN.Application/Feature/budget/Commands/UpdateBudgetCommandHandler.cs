using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.budget;

namespace MONEY_GUARDIAN.Application.Feature.budget.Commands
{
    public class UpdateBudgetCommandHandler(
        BudgetService service
    ) : IRequestHandler<UpdateBudgetCommand, BudgetDto>
    {
        public async Task<BudgetDto> Handle(
            UpdateBudgetCommand command,
            CancellationToken cancellationToken
        )
        {
            Budget budget = await service.UpdateBudgetAsync(
                command.Id,
                command.ExpenseTypeId,
                command.UserId,
                command.Year,
                command.Month,
                command.Amount
            );

            return MapBudgetToBudgetDto(budget);
        }

        private static BudgetDto MapBudgetToBudgetDto(Budget budget)
        {
            return new BudgetDto()
            {
                Id = budget.Id,
                UserId = budget.UserId,
                ExpenseTypeId = budget.ExpenseTypeId,
                Month = budget.Month,
                Year = budget.Year,
                Amount = budget.Amount
            };
        }
    }
}
