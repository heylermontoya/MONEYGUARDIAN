using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Queries
{
    public record GetMonetaryFundByIdQuery(
        int Id
    ) : IRequest<MonetaryFundDto>;
}
