using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Commands
{
    public record UpdateDepositCommand(
        int Id,
        DateTime Date,
        int MonetaryFundId,
        decimal Amount
    ) : IRequest<DepositDto>;
}
