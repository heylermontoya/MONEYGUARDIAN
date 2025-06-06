namespace MONEY_GUARDIAN.Application.DTOs
{
    public class ExpenseDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MonetaryFundId { get; set; }
        public string? MonetaryFundName { get; set; } = default!;

        public string Observation { get; set; } = default!;
        public string Merchant { get; set; } = default!;
        public string DocumentType { get; set; } = default!;
        public int UserId { get; set; }
        public string? UserName { get; set; } = default!;

        public List<DetailExpenseDto> Details { get; set; } = [];
    }
}
