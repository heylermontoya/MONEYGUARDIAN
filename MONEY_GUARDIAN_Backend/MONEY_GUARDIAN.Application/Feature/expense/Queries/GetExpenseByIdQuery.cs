using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.expense.Queries
{
    public record GetExpenseByIdQuery(
        int Id
    ) : IRequest<ExpenseDto>;
}
