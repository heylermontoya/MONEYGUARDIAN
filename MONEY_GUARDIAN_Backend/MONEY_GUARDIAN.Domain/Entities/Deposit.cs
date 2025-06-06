namespace MONEY_GUARDIAN.Domain.Entities
{
    public class Deposit
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int MonetaryFundId { get; set; }
        public decimal Amount { get; set; }
        public int UserId { get; set; }

        public User? User { get; set; }
        public MonetaryFund? MonetaryFund { get; set; }
    }
}
