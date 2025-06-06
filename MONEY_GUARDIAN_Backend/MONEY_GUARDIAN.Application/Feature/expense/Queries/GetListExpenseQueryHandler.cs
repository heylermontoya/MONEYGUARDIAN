using MediatR;
using MONEY_GUARDIAN.Application.DTOs;
using MONEY_GUARDIAN.Domain.Entities;
using MONEY_GUARDIAN.Domain.Enums;
using MONEY_GUARDIAN.Domain.Helpers;
using MONEY_GUARDIAN.Domain.Ports;
using MONEY_GUARDIAN.Domain.QueryFilters;

namespace MONEY_GUARDIAN.Application.Feature.expense.Queries
{
    public class GetListExpenseQueryHandler(
        IQueryWrapper queryWrapper,
        IGenericRepository<ExpenseDetail> expenseDetailRepository
    ) : IRequestHandler<GetListExpenseQuery, List<ExpenseDto>>
    {
        public async Task<List<ExpenseDto>> Handle(
            GetListExpenseQuery query,
            CancellationToken cancellationToken
        )
        {
            List<FieldFilter> listFilters = query.FieldFilter != null ? query.FieldFilter.ToList() : [];

            IEnumerable<ExpenseDto> listExpense =
                await queryWrapper
                    .QueryAsync<ExpenseDto>(
                        ItemsMessageConstants.GetExpenseDetail
                            .GetDescription(),
                        new
                        { },
                        FieldFilterHelper.BuildQueryArgs(listFilters)
                    );

            listExpense = listExpense.DistinctBy(x => x.Id);

            foreach (var expenseHeader in listExpense)
            {
                List<ExpenseDetail> listExpenseDetail = (await expenseDetailRepository.GetAsync(
                    expense => expense.ExpenseHeaderId == expenseHeader.Id,
                    includeStringProperties: $"{nameof(ExpenseDetail.ExpenseType)}"
                )).ToList();

                var detailDtos = listExpenseDetail.Select(detail => new DetailExpenseDto
                {
                    ExpenseDetailId = detail.Id,
                    ExpenseTypeId = detail.ExpenseTypeId,
                    ExpenseTypeName = detail.ExpenseType.Name,
                    Amount = detail.Amount
                });

                expenseHeader.Details.AddRange(detailDtos);
            }

            return listExpense.ToList();
        }
    }
}
