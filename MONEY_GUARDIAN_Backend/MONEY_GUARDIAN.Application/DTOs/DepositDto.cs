namespace MONEY_GUARDIAN.Application.DTOs
{
    public class DepositDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int MonetaryFundId { get; set; } = default!;
        public string? MonetaryFundName { get; set; } = default!;
    }
}
