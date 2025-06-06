using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Domain.Tests.DataBuilder
{
    public class ExpenseHeaderBuilder
    {
        private int _id = 1;
        private DateTime _date = DateTime.Today;
        private int _monetaryFundId = 1;
        private int _userId = 1;
        private string _observations = "Observaciones por defecto";
        private string _merchant = "Comercio por defecto";
        private string _documentType = "Factura";

        public ExpenseHeaderBuilder WithId(int id)
        {
            _id = id;
            return this;
        }

        public ExpenseHeaderBuilder WithDate(DateTime date)
        {
            _date = date;
            return this;
        }

        public ExpenseHeaderBuilder WithMonetaryFundId(int fundId)
        {
            _monetaryFundId = fundId;
            return this;
        }

        public ExpenseHeaderBuilder WithUserId(int userId)
        {
            _userId = userId;
            return this;
        }

        public ExpenseHeaderBuilder WithObservations(string observations)
        {
            _observations = observations;
            return this;
        }

        public ExpenseHeaderBuilder WithMerchant(string merchant)
        {
            _merchant = merchant;
            return this;
        }

        public ExpenseHeaderBuilder WithDocumentType(string docType)
        {
            _documentType = docType;
            return this;
        }

        public ExpenseHeader Build()
        {
            return new ExpenseHeader
            {
                Id = _id,
                Date = _date,
                MonetaryFundId = _monetaryFundId,
                UserId = _userId,
                Observations = _observations,
                Merchant = _merchant,
                DocumentType = _documentType
            };
        }
    }
}
