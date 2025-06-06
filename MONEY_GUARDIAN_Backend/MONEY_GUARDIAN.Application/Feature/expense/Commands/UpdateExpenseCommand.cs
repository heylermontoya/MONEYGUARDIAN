using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.expense.Commands
{
    public record UpdateExpenseCommand(
        int Id,
        List<DetailExpenseDto> Details,
        DateTime Date,
        int MonetaryFundId,
        int UserId,
        string Observation,
        string Merchant,
        string DocumentType
    ) : IRequest<ExpenseDto>;
}
