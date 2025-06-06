using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Queries
{
    public record GetDepositByIdQuery(
        int Id
    ) : IRequest<DepositDto>;
}
