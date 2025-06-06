using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expense;

namespace MONEY_GUARDIAN.Application.Feature.expense.Queries
{
    public class GetExpenseByIdQueryHandler(
        ExpenseService service
    ) : IRequestHandler<GetExpenseByIdQuery, ExpenseDto>
    {
        public async Task<ExpenseDto> Handle(
            GetExpenseByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            ExpenseHeader Expense = await service.GetExpenseHeaderById(
                command.Id
            );

            return MapExpenseHeaderToExpenseDto(Expense);
        }

        private static ExpenseDto MapExpenseHeaderToExpenseDto(ExpenseHeader expenseHeader)
        {
            return new ExpenseDto()
            {
                Id = expenseHeader.Id,
                Date = expenseHeader.Date,
                MonetaryFundId = expenseHeader.MonetaryFundId,
                Observation = expenseHeader.Observations,
                Merchant = expenseHeader.Merchant,
                DocumentType = expenseHeader.DocumentType,
                UserId = expenseHeader.UserId,
                Details = []
            };
        }
    }
}
