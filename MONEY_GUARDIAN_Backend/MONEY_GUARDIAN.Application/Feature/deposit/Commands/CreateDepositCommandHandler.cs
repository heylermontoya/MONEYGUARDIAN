using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.deposit;

namespace MONEY_GUARDIAN.Application.Feature.deposit.Commands
{
    public class CreateDepositCommandHandler(
        DepositService service
    ) : IRequestHandler<CreateDepositCommand, DepositDto>
    {
        public async Task<DepositDto> Handle(
            CreateDepositCommand command,
            CancellationToken cancellationToken
        )
        {
            Deposit deposit = await service.CreateDepositAsync(
                command.Date,
                command.MonetaryFundId,
                command.Amount,
                command.UserId
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
