using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Queries
{
    public record GetListDepositQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<DepositDto>>;
}
