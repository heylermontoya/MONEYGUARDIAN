namespace MONEY_GUARDIAN.Application.DTOs
{
    public class ReportDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string TypeTransaction { get; set; } = default!;
        public decimal Amount { get; set; }

    }
}
