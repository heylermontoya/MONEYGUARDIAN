namespace MONEY_GUARDIAN.Application.DTOs
{
    public class ReportChartDto
    {
        public string Name { get; set; } = default!;
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
    }
}
