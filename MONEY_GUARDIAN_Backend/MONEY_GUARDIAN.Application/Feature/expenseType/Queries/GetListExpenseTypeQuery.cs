using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Queries
{
    public record GetListExpenseTypeQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<ExpenseTypeDto>>;
}
