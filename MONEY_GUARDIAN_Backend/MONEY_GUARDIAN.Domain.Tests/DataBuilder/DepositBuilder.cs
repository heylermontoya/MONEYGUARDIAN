using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class DepositBuilder
    {
        private int _id;
        private DateTime _date;
        private int _monetaryFundId;
        private decimal _amount;
        private int _userId;

        public DepositBuilder()
        {
            _id = 1;
            _date = DateTime.Now;
            _monetaryFundId = 1;
            _amount = 100.00m;
            _userId = 1;
        }

        public Deposit Build()
        {
            return new Deposit
            {
                Id = _id,
                Date = _date,
                MonetaryFundId = _monetaryFundId,
                Amount = _amount,
                UserId = _userId
            };
        }

        public DepositBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public DepositBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public DepositBuilder WithMonetaryFundId(int monetaryFundId)
        {
            _monetaryFundId = monetaryFundId;
            return this;
        }

        public DepositBuilder WithAmount(decimal amount)
        {
            _amount = amount;
            return this;
        }

        public DepositBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }
    }
}
