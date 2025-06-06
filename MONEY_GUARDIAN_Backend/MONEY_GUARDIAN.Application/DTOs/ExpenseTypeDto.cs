namespace MONEY_GUARDIAN.Application.DTOs
{
    public class ExpenseTypeDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
    }
}
