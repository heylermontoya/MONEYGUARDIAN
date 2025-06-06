using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class BudgetBuilder
    {
        private int _id;
        private int _userId;
        private int _expenseTypeId;
        private int _month;
        private int _year;
        private decimal _amount;

        public BudgetBuilder()
        {
            _id = 1;
            _userId = 1;
            _expenseTypeId = 1;
            _month = DateTime.Now.Month;
            _year = DateTime.Now.Year;
            _amount = 1000.00m;
        }

        public Budget Build()
        {
            return new Budget
            {
                Id = _id,
                UserId = _userId,
                ExpenseTypeId = _expenseTypeId,
                Month = _month,
                Year = _year,
                Amount = _amount
            };
        }

        public BudgetBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public BudgetBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        public BudgetBuilder WithExpenseTypeId(int expenseTypeId)
        {
            _expenseTypeId = expenseTypeId;
            return this;
        }

        public BudgetBuilder WithMonth(int month)
        {
            _month = month;
            return this;
        }

        public BudgetBuilder WithYear(int year)
        {
            _year = year;
            return this;
        }

        public BudgetBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }
    }
}
