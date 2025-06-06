namespace MONEY_GUARDIAN.Domain.Entities
{
    public class ExpenseType
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;

        public ICollection<ExpenseDetail>? ExpenseDetails { get; set; }
        public ICollection<Budget>? Budgets { get; set; }
    }
}
