using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.expense;

namespace MONEY_GUARDIAN.Application.Feature.expense.Commands
{
    public class UpdateExpenseCommandHandler(
        ExpenseService service
    ) : IRequestHandler<UpdateExpenseCommand, ExpenseDto>
    {
        public async Task<ExpenseDto> Handle(
            UpdateExpenseCommand command,
            CancellationToken cancellationToken
        )
        {
            List<ExpenseDetail> listExpenseDetail = [];
            foreach (var detail in command.Details)
            {
                if (detail.ExpenseDetailId == null)
                {
                    ExpenseDetail expenseDetail = new()
                    {
                        ExpenseTypeId = detail.ExpenseTypeId,
                        Amount = detail.Amount,
                    };

                    listExpenseDetail.Add(expenseDetail);
                }
                else
                {
                    ExpenseDetail expenseDetail = new()
                    {
                        Id = detail.ExpenseDetailId.Value,
                        ExpenseTypeId = detail.ExpenseTypeId,
                        Amount = detail.Amount,
                    };

                    listExpenseDetail.Add(expenseDetail);
                }
            }

            ExpenseHeader expenseHeader = await service.UpdateExpenseAsync(
                command.Id,
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
