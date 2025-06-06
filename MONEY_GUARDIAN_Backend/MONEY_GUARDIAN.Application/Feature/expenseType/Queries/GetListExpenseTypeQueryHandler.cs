using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.expenseType.Queries
{
    public class GetListExpenseTypeQueryHandler(
        IQueryWrapper queryWrapper
    ) : IRequestHandler<GetListExpenseTypeQuery, List<ExpenseTypeDto>>
    {
        public async Task<List<ExpenseTypeDto>> Handle(
            GetListExpenseTypeQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ExpenseType> listExpenseType =
               await queryWrapper
                   .QueryAsync<ExpenseType>(
                       ItemsMessageConstants.GetExpenseType
                           .GetDescription(),
                       new
                       { },
                       FieldFilterHelper.BuildQueryArgs(listFilters)
                   );

            return MapListExpenseTypeToListExpenseTypeDto(listExpenseType.ToList());
        }

        private static List<ExpenseTypeDto> MapListExpenseTypeToListExpenseTypeDto(List<ExpenseType> listExpenseType)
        {
            return listExpenseType.Select(expenseType =>
                new ExpenseTypeDto()
                {
                    Id = expenseType.Id,
                    Code = expenseType.Code,
                    Name = expenseType.Name
                }
            ).ToList();
        }
    }
}
