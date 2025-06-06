using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.deposit;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Commands
{
    public class UpdateDepositCommandHandler(
        DepositService service
    ) : IRequestHandler<UpdateDepositCommand, DepositDto>
    {
        public async Task<DepositDto> Handle(
            UpdateDepositCommand command,
            CancellationToken cancellationToken
        )
        {
            Deposit deposit = await service.UpdateDepositAsync(
                command.Id,
                command.Date,
                command.MonetaryFundId,
                command.Amount
            );

            return MapDepositToDepositDto(deposit);
        }

        private static DepositDto MapDepositToDepositDto(Deposit deposit)
        {
            return new DepositDto()
            {
                Id = deposit.Id,
                Date = deposit.Date,
                Amount = deposit.Amount,
                MonetaryFundId = deposit.MonetaryFundId,
            };
        }
    }
}
