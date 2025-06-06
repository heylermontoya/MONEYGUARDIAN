using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Commands
{
    public record CreateMonetaryFundCommand(
        string Name,
        string Type
    ) : IRequest<MonetaryFundDto>;
}
