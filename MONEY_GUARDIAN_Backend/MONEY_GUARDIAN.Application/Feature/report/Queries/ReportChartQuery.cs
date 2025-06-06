using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.report.Queries
{
    public record ReportChartQuery(
        IEnumerable<FieldFilter>? FieldFilter
    ) : IRequest<List<ReportChartDto>>;
}
