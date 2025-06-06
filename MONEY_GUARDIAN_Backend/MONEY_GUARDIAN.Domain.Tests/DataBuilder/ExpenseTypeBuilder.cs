using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class ExpenseTypeBuilder
    {
        private int _id;
        private string _code;
        private string _name;

        public ExpenseTypeBuilder()
        {
            _id = 1;
            _code = "DEFAULT_CODE";
            _name = "Default Expense Type";
        }

        public ExpenseType Build()
        {
            return new ExpenseType
            {
                Id = _id,
                Code = _code,
                Name = _name
            };
        }

        public ExpenseTypeBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ExpenseTypeBuilder WithCode(string code)
        {
            _code = code;
            return this;
        }

        public ExpenseTypeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
    }
}
