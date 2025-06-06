using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Queries
{
    public record GetListMonetaryFundQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<MonetaryFundDto>>;
}
