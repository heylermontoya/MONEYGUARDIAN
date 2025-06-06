using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class ExpenseDetailBuilder
    {
        private int _id = 1;
        private int _expenseHeaderId = 1;
        private int _expenseTypeId = 1;
        private decimal _amount = 1000;

        public ExpenseDetailBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ExpenseDetailBuilder WithExpenseHeaderId(int headerId)
        {
            _expenseHeaderId = headerId;
            return this;
        }

        public ExpenseDetailBuilder WithExpenseTypeId(int typeId)
        {
            _expenseTypeId = typeId;
            return this;
        }

        public ExpenseDetailBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public ExpenseDetail Build()
        {
            return new ExpenseDetail
            {
                Id = _id,
                ExpenseHeaderId = _expenseHeaderId,
                ExpenseTypeId = _expenseTypeId,
                Amount = _amount
            };
        }
    }
}
