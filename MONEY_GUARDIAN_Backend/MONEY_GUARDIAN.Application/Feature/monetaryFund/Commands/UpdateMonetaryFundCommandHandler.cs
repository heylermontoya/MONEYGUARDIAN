using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Commands
{
    public class UpdateMonetaryFundCommandHandler(
        MonetaryFundService service
    ) : IRequestHandler<UpdateMonetaryFundCommand, MonetaryFundDto>
    {
        public async Task<MonetaryFundDto> Handle(
            UpdateMonetaryFundCommand command,
            CancellationToken cancellationToken
        )
        {
            MonetaryFund monetaryFund = await service.UpdateMonetaryFundAsync(
                command.Id,
                command.Name,
                command.Type
            );

            return MapMonetaryFundToMonetaryFundDto(monetaryFund);
        }

        private static MonetaryFundDto MapMonetaryFundToMonetaryFundDto(MonetaryFund monetaryFund)
        {
            return new MonetaryFundDto()
            {
                Id = monetaryFund.Id,
                Name = monetaryFund.Name,
                Type = monetaryFund.Type
            };
        }
    }
}
