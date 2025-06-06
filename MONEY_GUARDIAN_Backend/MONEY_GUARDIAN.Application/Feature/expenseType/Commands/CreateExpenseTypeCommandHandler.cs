using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expenseType;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Commands
{
    public class CreateExpenseTypeCommandHandler(
        ExpenseTypeService service
    ) : IRequestHandler<CreateExpenseTypeCommand, ExpenseTypeDto>
    {
        public async Task<ExpenseTypeDto> Handle(
            CreateExpenseTypeCommand command,
            CancellationToken cancellationToken
        )
        {
            ExpenseType expenseType = await service.CreateExpenseTypeAsync(
                command.Name,
                ""
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
