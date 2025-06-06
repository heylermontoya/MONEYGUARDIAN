using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Queries
{
    public class GetListMonetaryFundQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListMonetaryFundQuery, List<MonetaryFundDto>>
    {
        public async Task<List<MonetaryFundDto>> Handle(
            GetListMonetaryFundQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<MonetaryFund> listMonetaryFund =
                await queryWrapper
                    .QueryAsync<MonetaryFund>(
                        ItemsMessageConstants.GetMonetaryFund
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return MapListMonetaryFundToListMonetaryFundDto(listMonetaryFund.ToList());
        }

        private static List<MonetaryFundDto> MapListMonetaryFundToListMonetaryFundDto(List<MonetaryFund> listMonetaryFund)
        {
            return listMonetaryFund.Select(monetaryFund =>
                new MonetaryFundDto()
                {
                    Id = monetaryFund.Id,
                    Name = monetaryFund.Name,
                    Type = monetaryFund.Type
                }
            ).ToList();
        }
    }
}
