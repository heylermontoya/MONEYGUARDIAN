using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Services.deposit;
using MONEY_GUARDIAN.Domain.Entities;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Queries
{
    public class GetDepositByIdQueryHandler(
        DepositService service
    ) : IRequestHandler<GetDepositByIdQuery, DepositDto>
    {
        public async Task<DepositDto> Handle(
            GetDepositByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            Deposit Deposit = await service.GetDepositById(
                command.Id
            );

            return MapDepositToDepositDto(Deposit);
        }

        private static DepositDto MapDepositToDepositDto(Deposit Deposit)
        {
            return new DepositDto()
            {
                Id = Deposit.Id,
                Date = Deposit.Date,
                Amount = Deposit.Amount
            };
        }
    }
}
