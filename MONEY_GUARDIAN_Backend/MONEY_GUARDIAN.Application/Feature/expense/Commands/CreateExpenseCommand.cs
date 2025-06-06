using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.expense.Commands
{
    public record CreateExpenseCommand(
        List<DetailExpenseDto> Details,
        DateTime Date,
        string DocumentType,
        string Merchant,
        int MonetaryFundId,
        string Observation,
        int UserId
    ) : IRequest<ExpenseDto>;
}
