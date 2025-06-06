using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Queries
{
    public record GetExpenseTypeByIdQuery(
        int Id
    ) : IRequest<ExpenseTypeDto>;
}
