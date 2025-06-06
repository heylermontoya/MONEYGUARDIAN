using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Exceptions;
using MONEY_GUARDIAN.Domain.Ports;

namespace MONEY_GUARDIAN.Domain.Services.expenseType
{
    [DomainService]
    public class ExpenseTypeService(
        IGenericRepository<ExpenseType> expenseTypeRepository
    )
    {
        public async Task<ExpenseType> CreateExpenseTypeAsync(
            string name,
            string code
        )
        {
            #region validations
            var existExpenseTypeWithName = await expenseTypeRepository.GetAsync(x => x.Name == name);

            if (existExpenseTypeWithName.Any())
            {
                throw new AppException(MessagesExceptions.ExistExpenseType);
            }
            #endregion

            ExpenseType expenseType = new()
            {
                Name = name,
                Code = code
            };

            expenseType = await expenseTypeRepository.AddAsync(expenseType);

            expenseType.Code = $"{expenseType.Id}_{expenseType.Name}_{expenseType.Id}";

            expenseType = await expenseTypeRepository.UpdateAsync(expenseType);

            return expenseType;
        }

        public async Task<ExpenseType> UpdateExpenseTypeAsync(
            int id,
            string name
        )
        {
            #region validations
            var existExpenseTypeWithName = await expenseTypeRepository.GetAsync(x => x.Name == name && x.Id != id);

            if (existExpenseTypeWithName.Any())
            {
                throw new AppException(MessagesExceptions.ExistExpenseType);
            }
            #endregion

            ExpenseType expenseType = await GetExpenseTypeById(id);

            expenseType.Name = name;
            expenseType.Code = $"{expenseType.Id}_{expenseType.Name}_{expenseType.Id}";

            expenseType = await expenseTypeRepository.UpdateAsync(expenseType);

            return expenseType;
        }

        public async Task<ExpenseType> GetExpenseTypeById(int id)
        {
            ExpenseType? expenseType = await expenseTypeRepository.GetByIdAsync(id);

            return expenseType ?? throw new AppException(MessagesExceptions.NotExistExpenseType);
        }
    }
}
