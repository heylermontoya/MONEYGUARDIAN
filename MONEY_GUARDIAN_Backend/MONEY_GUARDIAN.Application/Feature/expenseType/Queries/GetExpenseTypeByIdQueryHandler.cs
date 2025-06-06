using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expenseType;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Queries
{
    public class GetExpenseTypeByIdQueryHandler(
        ExpenseTypeService service
    ) : IRequestHandler<GetExpenseTypeByIdQuery, ExpenseTypeDto>
    {
        public async Task<ExpenseTypeDto> Handle(
            GetExpenseTypeByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            ExpenseType expenseType = await service.GetExpenseTypeById(
                command.Id
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
