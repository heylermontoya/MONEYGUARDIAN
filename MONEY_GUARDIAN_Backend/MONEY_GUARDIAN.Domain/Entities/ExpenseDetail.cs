namespace MONEY_GUARDIAN.Domain.Entities
{
    public class ExpenseDetail
    {
        public int Id { get; set; }
        public int ExpenseHeaderId { get; set; }
        public int ExpenseTypeId { get; set; }
        public decimal Amount { get; set; }

        public ExpenseHeader? ExpenseHeader { get; set; }
        public ExpenseType? ExpenseType { get; set; }
    }
}
