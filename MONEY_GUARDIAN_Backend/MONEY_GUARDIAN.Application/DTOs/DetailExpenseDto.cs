namespace MONEY_GUARDIAN.Application.DTOs
{
    public class DetailExpenseDto
    {
        public int? ExpenseDetailId { get; set; }
        public int ExpenseTypeId { get; set; }
        public string? ExpenseTypeName { get; set; } = default!;
        public decimal Amount { get; set; }
    }
}
