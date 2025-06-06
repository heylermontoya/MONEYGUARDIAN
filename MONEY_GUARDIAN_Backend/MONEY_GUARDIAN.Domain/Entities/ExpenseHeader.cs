namespace MONEY_GUARDIAN.Domain.Entities
{
    public class ExpenseHeader
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MonetaryFundId { get; set; }
        public int UserId { get; set; }
        public string Observations { get; set; } = null!;
        public string Merchant { get; set; } = null!;
        public string DocumentType { get; set; } = null!;

        public MonetaryFund? MonetaryFund { get; set; }
        public User? User { get; set; }
        public ICollection<ExpenseDetail>? Details { get; set; }
    }
}
