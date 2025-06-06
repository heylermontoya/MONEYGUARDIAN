using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.budget.Queries
{
    public class GetListBudgetQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListBudgetQuery, List<BudgetDto>>
    {
        public async Task<List<BudgetDto>> Handle(
            GetListBudgetQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<BudgetDto> properties =
                await queryWrapper
                    .QueryAsync<BudgetDto>(
                        ItemsMessageConstants.GetBudgets
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return properties.ToList();
        }
    }
}
