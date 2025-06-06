namespace MONEY_GUARDIAN.Domain.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ExpenseTypeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }

        public User? User { get; set; }
        public ExpenseType? ExpenseType { get; set; }
    }
}
