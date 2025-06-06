using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Commands
{
    public record CreateDepositCommand(
        DateTime Date,
        int MonetaryFundId,
        decimal Amount,
        int UserId
    ) : IRequest<DepositDto>;
}
