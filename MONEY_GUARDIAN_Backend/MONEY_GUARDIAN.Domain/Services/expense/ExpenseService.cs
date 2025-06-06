using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;
using MONEY_GUARDIAN.Domain.Services.user;
using System.Transactions;

namespace MONEY_GUARDIAN.Domain.Services.expense
{
    [DomainService]
    public class ExpenseService(
        IGenericRepository<ExpenseDetail> expenseDetailRepository,
        IGenericRepository<ExpenseHeader> expenseHeaderRepository,
        IGenericRepository<Budget> budgetRepository,
        MonetaryFundService monetaryFundService,
        UserService userService
    )
    {
        public async Task<ExpenseHeader> CreateExpenseAsync(
            List<ExpenseDetail> listExpenseDetail,
            DateTime date,
            int monetaryFundId,
            int userId,
            string observation,
            string merchant,
            string documentType
        )
        {
            #region validations

            await ValidationInputParameters(date, monetaryFundId, userId);

            foreach (ExpenseDetail expenseDetail in listExpenseDetail)
            {
                Budget? budget = (
                    await budgetRepository.GetAsync
                    (
                        budget => budget.UserId == userId &&
                        budget.ExpenseTypeId == expenseDetail.ExpenseTypeId &&
                        budget.Year == date.Year &&
                        budget.Month == date.Month
                    )
                )
                .FirstOrDefault() ?? throw new AppException(MessagesExceptions.NotExistBudget);

                decimal valueBudget = budget.Amount;

                decimal amountExpense = listExpenseDetail.Where(x => x.ExpenseTypeId == expenseDetail.ExpenseTypeId).Sum(x => x.Amount);

                if (amountExpense > valueBudget)
                {
                    throw new AppException(MessagesExceptions.ExpenseExceedsBudget);
                }
            }
            #endregion

            #region operation in transaction

            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            ExpenseHeader expenseHeader = new()
            {
                Date = date,
                MonetaryFundId = monetaryFundId,
                UserId = userId,
                Observations = observation,
                Merchant = merchant,
                DocumentType = documentType
            };

            expenseHeader = await expenseHeaderRepository.AddAsync(expenseHeader);

            foreach (var item in listExpenseDetail)
            {

                ExpenseDetail expenseDetail = new()
                {
                    ExpenseHeaderId = expenseHeader.Id,
                    ExpenseTypeId = item.ExpenseTypeId,
                    Amount = item.Amount
                };

                await expenseDetailRepository.AddAsync(expenseDetail);
            }

            scope.Complete();

            #endregion 

            return expenseHeader;
        }

        public async Task<ExpenseHeader> UpdateExpenseAsync(
            int id,
            List<ExpenseDetail> listExpenseDetail,
            DateTime date,
            int monetaryFundId,
            int userId,
            string observation,
            string merchant,
            string documentType
        )
        {
            #region validations

            await ValidationInputParameters(date, monetaryFundId, userId);

            foreach (ExpenseDetail expenseDetail in listExpenseDetail)
            {
                Budget? budget = (
                    await budgetRepository.GetAsync
                    (
                        budget => budget.UserId == userId &&
                        budget.ExpenseTypeId == expenseDetail.ExpenseTypeId &&
                        budget.Year == date.Year &&
                        budget.Month == date.Month
                    )
                )
                .FirstOrDefault() ?? throw new AppException(MessagesExceptions.NotExistBudget);

                decimal valueBudget = budget.Amount;

                decimal amountExpense = listExpenseDetail.Where(x => x.ExpenseTypeId == expenseDetail.ExpenseTypeId).Sum(x => x.Amount);

                if (amountExpense > valueBudget)
                {
                    throw new AppException(MessagesExceptions.ExpenseExceedsBudget);
                }
            }

            #endregion

            #region operation in transaction

            var incomingIds = listExpenseDetail
                .Where(d => d.Id > 0)
                .Select(d => d.Id)
                .ToList();


            await expenseDetailRepository.DeleteAsync(
                x => x.ExpenseHeaderId == id && !incomingIds.Contains(x.Id)
            );

            using TransactionScope scope = new(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var detail in listExpenseDetail)
            {
                if (detail.Id == 0)
                {
                    ExpenseDetail expenseDetail = new()
                    {
                        ExpenseTypeId = detail.ExpenseTypeId,
                        ExpenseHeaderId = id,
                        Amount = detail.Amount,
                    };

                    await expenseDetailRepository.AddAsync(expenseDetail);
                }
                else
                {
                    ExpenseDetail expenseDetail = await GetExpenseDetailById(detail.Id);

                    expenseDetail.ExpenseTypeId = detail.ExpenseTypeId;
                    expenseDetail.Amount = detail.Amount;

                    await expenseDetailRepository.UpdateAsync(expenseDetail);
                }
            }

            ExpenseHeader expenseHeader = await GetExpenseHeaderById(id);

            expenseHeader.Date = date;
            expenseHeader.MonetaryFundId = monetaryFundId;
            expenseHeader.UserId = userId;
            expenseHeader.Observations = observation;
            expenseHeader.Merchant = merchant;
            expenseHeader.DocumentType = documentType;

            await expenseHeaderRepository.UpdateAsync(expenseHeader);

            scope.Complete();

            #endregion

            return expenseHeader;
        }

        public async Task<ExpenseDetail> GetExpenseDetailById(int id)
        {
            ExpenseDetail? expenseDetail = await expenseDetailRepository.GetByIdAsync(id);

            return expenseDetail ?? throw new AppException(MessagesExceptions.NotExistExpenseDetail);
        }

        public async Task<ExpenseHeader> GetExpenseHeaderById(int id)
        {
            ExpenseHeader? expenseHeader = await expenseHeaderRepository.GetByIdAsync(id);

            return expenseHeader ?? throw new AppException(MessagesExceptions.NotExistExpenseHeader);
        }

        private async Task ValidationInputParameters(DateTime date, int monetaryFundId, int userId)
        {
            if (date.Year < 2020)
            {
                throw new AppException(MessagesExceptions.YearInvalid);
            }

            await monetaryFundService.GetMonetaryFundById(monetaryFundId);
            await userService.GetUserById(userId);
        }
    }
}
