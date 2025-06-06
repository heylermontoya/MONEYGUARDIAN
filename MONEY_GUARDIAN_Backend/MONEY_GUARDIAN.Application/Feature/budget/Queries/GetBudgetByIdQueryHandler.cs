using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Services.budget;
using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Application.Feature.budget.Queries
{
    public class GetBudgetByIdQueryHandler(
        BudgetService service
    ) : IRequestHandler<GetBudgetByIdQuery, BudgetDto>
    {
        public async Task<BudgetDto> Handle(
            GetBudgetByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            Budget budget = await service.GetBudgetById(
                command.Id
            );

            return MapBudgetToBudgetDto(budget);
        }

        private static BudgetDto MapBudgetToBudgetDto(Budget budget)
        {
            return new BudgetDto()
            {
                Id = budget.Id,
                ExpenseTypeId = budget.ExpenseTypeId,
                Year = budget.Year,
                Month = budget.Month,
                Amount = budget.Amount
            };
        }
    }
}
