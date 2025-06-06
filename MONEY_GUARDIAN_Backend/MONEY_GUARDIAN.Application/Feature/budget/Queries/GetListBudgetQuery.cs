using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.budget.Queries
{
    public record GetListBudgetQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<BudgetDto>>;
}
