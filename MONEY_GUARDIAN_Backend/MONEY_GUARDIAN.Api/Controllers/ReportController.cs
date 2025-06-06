using MediatR;
using Microsoft.AspNetCore.Mvc;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Application.Feature.report.Queries;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController(IMediator mediator)
    {
        [HttpPost("list")]
        public async Task<IActionResult> ObtainListReportAsync(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ReportDto> listReportDtoDto = await mediator.Send(
                           new GetReportQuery(fieldFilter)
                       );

            return new OkObjectResult(listReportDtoDto);
        }

        [HttpPost("infoChart")]
        public async Task<IActionResult> GetInfoChart(
            IEnumerable<FieldFilter>? fieldFilter
        )
        {
            List<ReportChartDto> listReportDtoDto = await mediator.Send(
                new ReportChartQuery(fieldFilter)
            );

            return new OkObjectResult(listReportDtoDto);
        }
    }
}
