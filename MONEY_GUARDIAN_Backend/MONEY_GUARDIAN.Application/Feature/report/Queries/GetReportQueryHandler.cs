using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.report.Queries
{
    public class GetReportQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetReportQuery, List<ReportDto>>
    {
        public async Task<List<ReportDto>> Handle(
            GetReportQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ReportDto> listMonetaryFund =
                await queryWrapper
                    .QueryAsync<ReportDto>(
                        ItemsMessageConstants.GetReport
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            return listMonetaryFund.ToList();
        }
    }
}
