using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class MonetaryFundBuilder
    {
        private int _id = 1;
        private string _name = "Fondo Principal";
        private string _type = "Corriente";

        public MonetaryFundBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public MonetaryFundBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public MonetaryFundBuilder WithType(string type)
        {
            _type = type;
            return this;
        }        

        public MonetaryFund Build()
        {
            return new MonetaryFund
            {
                Id = _id,
                Name = _name,
                Type = _type
            };
        }
    }
}
