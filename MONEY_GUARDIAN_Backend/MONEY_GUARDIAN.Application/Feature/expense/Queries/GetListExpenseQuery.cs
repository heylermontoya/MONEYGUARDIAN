using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.expense.Queries
{
    public record GetListExpenseQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<ExpenseDto>>;
}
