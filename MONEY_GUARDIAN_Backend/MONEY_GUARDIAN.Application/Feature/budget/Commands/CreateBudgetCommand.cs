using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.budget.Commands
{
    public record CreateBudgetCommand(
        int ExpenseTypeId,
        int UserId,
        int Month,
        int Year,
        decimal Amount
    ) : IRequest<BudgetDto>;
}
