using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;

namespace MONEY_GUARDIAN.Domain.Services.monetaryFund
{
    [DomainService]
    public class MonetaryFundService(
        IGenericRepository<MonetaryFund> monetaryFundRepository
    )
    {
        public async Task<MonetaryFund> CreateMonetaryFundAsync(
            string name,
            string type
        )
        {
            #region validations
            var existmonetaryFund = await monetaryFundRepository.GetAsync(x => x.Name == name && x.Type == type);

            if (existmonetaryFund.Any())
            {
                throw new AppException(MessagesExceptions.ExistMonetaryFund);
            }
            #endregion

            MonetaryFund monetaryFund = new()
            {
                Name = name,
                Type = type
            };

            monetaryFund = await monetaryFundRepository.AddAsync(monetaryFund);

            return monetaryFund;
        }

        public async Task<MonetaryFund> UpdateMonetaryFundAsync(
            int id,
            string name,
            string type
        )
        {
            #region validations
            var existmonetaryFund = await monetaryFundRepository.GetAsync(x => x.Name == name && x.Type == type && x.Id != id);

            if (existmonetaryFund.Any())
            {
                throw new AppException(MessagesExceptions.ExistMonetaryFund);
            }
            #endregion

            MonetaryFund monetaryFund = await GetMonetaryFundById(id);

            monetaryFund.Name = name;
            monetaryFund.Type = type;


            monetaryFund = await monetaryFundRepository.UpdateAsync(monetaryFund);

            return monetaryFund;
        }

        public async Task<MonetaryFund> GetMonetaryFundById(int id)
        {
            MonetaryFund? monetaryFund = await monetaryFundRepository.GetByIdAsync(id);

            return monetaryFund ?? throw new AppException(MessagesExceptions.NotExistMonetaryFund);
        }
    }
}
