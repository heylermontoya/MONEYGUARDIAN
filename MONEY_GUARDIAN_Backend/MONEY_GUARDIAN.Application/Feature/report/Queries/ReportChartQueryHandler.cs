using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.report.Queries
{
    public class ReportChartQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<ReportChartQuery, List<ReportChartDto>>
    {
        public async Task<List<ReportChartDto>> Handle(
            ReportChartQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ReportChartDto> listMonetaryFund =
                await queryWrapper
                    .QueryAsync<ReportChartDto>(
                        ItemsMessageConstants.GetReportChart
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return listMonetaryFund.ToList();
        }
    }
}
