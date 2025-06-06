using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Queries
{
    public class GetListDepositQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListDepositQuery, List<DepositDto>>
    {
        public async Task<List<DepositDto>> Handle(
            GetListDepositQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<DepositDto> properties =
                await queryWrapper
                    .QueryAsync<DepositDto>(
                        ItemsMessageConstants.GetDeposits
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return properties.ToList();
        }
    }
}
