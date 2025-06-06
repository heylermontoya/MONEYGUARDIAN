using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Commands
{
    public record UpdateMonetaryFundCommand(
        int Id,
        string Name,
        string Type
    ) : IRequest<MonetaryFundDto>;
}
