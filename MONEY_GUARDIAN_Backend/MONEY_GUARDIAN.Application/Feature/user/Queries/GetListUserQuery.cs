using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.user.Queries
{
    public record GetListUserQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<UserDto>>;
}
