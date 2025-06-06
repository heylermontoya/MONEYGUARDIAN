using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Commands
{
    public record CreateExpenseTypeCommand(
        string Name
    ) : IRequest<ExpenseTypeDto>;
}
