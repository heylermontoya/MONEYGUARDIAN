namespace MONEY_GUARDIAN.Application.DTOs
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; } = default!;
        public int ExpenseTypeId { get; set; }
        public string? ExpenseTypeName { get; set; } = default!;
        public int Month { get; set; }
        public int Year { get; set; }
        public decimal Amount { get; set; }
    }
}
