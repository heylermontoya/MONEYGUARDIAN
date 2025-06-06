using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.Services.expenseType;
using MONEY_GUARDIAN.Domain.Services.user;

namespace MONEY_GUARDIAN.Domain.Services.budget
{
    [DomainService]
    public class BudgetService(
        IGenericRepository<Budget> budgetRepository,
        IGenericRepository<Deposit> depositRepository,
        ExpenseTypeService expenseTypeService,
        UserService userService
    )
    {
        public async Task<Budget> CreateBudgetAsync(
            int expenseTypeId,
            int userId,
            int year,
            int month,
            decimal amount
        )
        {
            #region validations

            await ValidationInputParameters(expenseTypeId, userId, amount, month, year);

            decimal amountBudget = (await budgetRepository.GetAsync()).Sum(x => x.Amount);
            await ValidationBudgetExceedsDeposits(amount, amountBudget);

            IEnumerable<Budget> validationBudget = await budgetRepository.GetAsync(
                x => x.UserId == userId &&
                x.ExpenseTypeId == expenseTypeId &&
                x.Month == month &&
                x.Year == year
            );
            if (validationBudget.Any())
            {
                throw new AppException(MessagesExceptions.UniqueMonthlyBudget);
            }

            #endregion

            Budget budget = new()
            {
                ExpenseTypeId = expenseTypeId,
                UserId = userId,
                Year = year,
                Month = month,
                Amount = amount
            };

            budget = await budgetRepository.AddAsync(budget);

            return budget;
        }

        public async Task<Budget> UpdateBudgetAsync(
            int id,
            int expenseTypeId,
            int userId,
            int year,
            int month,
            decimal amount
        )
        {

            #region validations

            await ValidationInputParameters(expenseTypeId, userId, amount, month, year);

            decimal amountBudget = (await budgetRepository.GetAsync(x => x.Id != id)).Sum(x => x.Amount);
            await ValidationBudgetExceedsDeposits(amount, amountBudget);

            IEnumerable<Budget> validationBudget = await budgetRepository.GetAsync(
                x => x.UserId == userId &&
                x.ExpenseTypeId == expenseTypeId &&
                x.Month == month &&
                x.Year == year &&
                x.Id != id
            );
            if (validationBudget.Any())
            {
                throw new AppException(MessagesExceptions.UniqueMonthlyBudget);
            }

            #endregion

            Budget budget = await GetBudgetById(id);

            budget.ExpenseTypeId = expenseTypeId;
            budget.UserId = userId;
            budget.Year = year;
            budget.Month = month;
            budget.Amount = amount;

            budget = await budgetRepository.UpdateAsync(budget);

            return budget;
        }

        public async Task<Budget> GetBudgetById(int id)
        {
            Budget? budget = await budgetRepository.GetByIdAsync(id);

            return budget ?? throw new AppException(MessagesExceptions.NotExistBudget);
        }

        private async Task ValidationInputParameters(
            int expenseTypeId,
            int userId,
            decimal amount,
            int month,
            int year
        )
        {            
            if (amount < 0)
            {
                throw new AppException(MessagesExceptions.AmountGreatherThanZero);
            }

            if (month < 1 || month > 12)
            {
                throw new AppException(MessagesExceptions.MonthInvalid);
            }

            if (year < 2020)
            {
                throw new AppException(MessagesExceptions.YearInvalid);
            }

            await expenseTypeService.GetExpenseTypeById(expenseTypeId);

            await userService.GetUserById(userId);
        }

        private async Task ValidationBudgetExceedsDeposits(decimal amount, decimal amountBudget)
        {
            decimal totalAmountBudget = amountBudget + amount;
            decimal totalAmountDeposit = (await depositRepository.GetAsync()).Sum(x => x.Amount);
            if (totalAmountBudget > totalAmountDeposit)
            {
                throw new AppException(MessagesExceptions.BudgetExceedsDeposits);
            }
        }
    }
}
