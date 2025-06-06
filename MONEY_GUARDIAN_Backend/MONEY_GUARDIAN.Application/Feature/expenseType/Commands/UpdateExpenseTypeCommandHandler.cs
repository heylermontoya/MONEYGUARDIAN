using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expenseType;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Commands
{
    public class UpdateExpenseTypeCommandHandler(
        ExpenseTypeService service
    ) : IRequestHandler<UpdateExpenseTypeCommand, ExpenseTypeDto>
    {
        public async Task<ExpenseTypeDto> Handle(
            UpdateExpenseTypeCommand command,
            CancellationToken cancellationToken
        )
        {
            ExpenseType expenseType = await service.UpdateExpenseTypeAsync(
                command.Id,
                command.Name
            );

            return MapExpenseTypeToExpenseTypeDto(expenseType);
        }

        private static ExpenseTypeDto MapExpenseTypeToExpenseTypeDto(ExpenseType expenseType)
        {
            return new ExpenseTypeDto()
            {
                Id = expenseType.Id,
                Code = expenseType.Code,
                Name = expenseType.Name
            };
        }
    }
}
