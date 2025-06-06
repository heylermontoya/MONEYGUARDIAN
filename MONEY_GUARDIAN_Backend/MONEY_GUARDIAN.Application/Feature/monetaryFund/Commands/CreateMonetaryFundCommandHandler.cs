using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Commands
{
    public class CreateMonetaryFundCommandHandler(
        MonetaryFundService service
    ) : IRequestHandler<CreateMonetaryFundCommand, MonetaryFundDto>
    {
        public async Task<MonetaryFundDto> Handle(
            CreateMonetaryFundCommand command,
            CancellationToken cancellationToken
        )
        {
            MonetaryFund monetaryFund = await service.CreateMonetaryFundAsync(
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
