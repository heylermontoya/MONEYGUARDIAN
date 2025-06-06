using MediatR;
using MONEY_GUARDIAN.Application.DTOs;

namespace MONEY_GUARDIAN.Application.Feature.budget.Queries
{
    public record GetBudgetByIdQuery(
        int Id
    ) : IRequest<BudgetDto>;
}
