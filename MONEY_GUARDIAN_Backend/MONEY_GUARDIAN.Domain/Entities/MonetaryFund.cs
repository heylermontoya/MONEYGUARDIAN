namespace MONEY_GUARDIAN.Domain.Entities
{
    public class MonetaryFund
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;

        public ICollection<ExpenseHeader>? ExpenseHeaders { get; set; }
        public ICollection<Deposit>? Deposits { get; set; }
    }
}
