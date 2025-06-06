using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expense;

namespace MONEY_GUARDIAN.Application.Feature.expense.Commands
{
    public class CreateExpenseCommandHandler(
        ExpenseService service
    ) : IRequestHandler<CreateExpenseCommand, ExpenseDto>
    {
        public async Task<ExpenseDto> Handle(
            CreateExpenseCommand command,
            CancellationToken cancellationToken
        )
        {

            List<ExpenseDetail> listExpenseDetail = command.Details.Select(detail =>
                new ExpenseDetail()
                {
                    ExpenseTypeId = detail.ExpenseTypeId,
                    Amount = detail.Amount
                }
            ).ToList();

            ExpenseHeader expenseHeader = await service.CreateExpenseAsync(
                listExpenseDetail,
                command.Date,
                command.MonetaryFundId,
                command.UserId,
                command.Observation,
                command.Merchant,
                command.DocumentType
            );

            return MapExpenseHeaderToExpenseDto(expenseHeader);
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
