using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Services.user;

namespace MONEY_GUARDIAN.Domain.Services.deposit
{
    [DomainService]
    public class DepositService(
        IGenericRepository<Deposit> depositRepository,
        MonetaryFundService monetaryFundService,
        UserService userService
    )
    {
        public async Task<Deposit> CreateDepositAsync(
            DateTime date,
            int monetaryFundId,
            decimal amount,
            int userId
        )
        {
            #region validations

            await ValidationInputParameters(date, amount, monetaryFundId);

            await userService.GetUserById(userId);

            #endregion

            Deposit Deposit = new()
            {
                Date = date,
                MonetaryFundId = monetaryFundId,
                Amount = amount,
                UserId = userId
            };

            Deposit = await depositRepository.AddAsync(Deposit);

            return Deposit;
        }

        public async Task<Deposit> UpdateDepositAsync(
            int id,
            DateTime date,
            int monetaryFundId,
            decimal amount
        )
        {
            #region validations

            await ValidationInputParameters(date, amount, monetaryFundId);

            #endregion

            Deposit Deposit = await GetDepositById(id);

            Deposit.Date = date;
            Deposit.MonetaryFundId = monetaryFundId;
            Deposit.Amount = amount;

            Deposit = await depositRepository.UpdateAsync(Deposit);

            return Deposit;
        }

        public async Task<Deposit> GetDepositById(int id)
        {
            Deposit? Deposit = await depositRepository.GetByIdAsync(id);

            return Deposit ?? throw new AppException(MessagesExceptions.NotExistDeposit);
        }

        private async Task ValidationInputParameters(DateTime date, decimal amount, int monetaryFundId)
        {
            if (date.Year < 2020)
            {
                throw new AppException(MessagesExceptions.YearInvalid);
            }

            if (amount < 0)
            {
                throw new AppException(MessagesExceptions.AmountGreatherThanZero);
            }

            await monetaryFundService.GetMonetaryFundById(monetaryFundId);
        }
    }
}
