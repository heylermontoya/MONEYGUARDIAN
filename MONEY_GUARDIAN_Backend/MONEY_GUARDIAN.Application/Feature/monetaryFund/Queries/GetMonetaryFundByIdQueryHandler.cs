using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Services.monetaryFund;

namespace MONEY_GUARDIAN.Application.Feature.monetaryFund.Queries
{
    public class GetMonetaryFundByIdQueryHandler(
        MonetaryFundService service
    ) : IRequestHandler<GetMonetaryFundByIdQuery, MonetaryFundDto>
    {
        public async Task<MonetaryFundDto> Handle(
            GetMonetaryFundByIdQuery command,
            CancellationToken cancellationToken
        )
        {
            MonetaryFund monetaryFund = await service.GetMonetaryFundById(
                command.Id
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
