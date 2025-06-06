namespace MONEY_GUARDIAN.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string? Password { get; set; } = null!;
        public bool IsUserGoogle { get; set; }

        public ICollection<Budget>? Budgets { get; set; }
        public ICollection<Deposit>? Deposits { get; set; }
        public ICollection<ExpenseHeader>? ExpenseHeaders { get; set; }
    }
}
