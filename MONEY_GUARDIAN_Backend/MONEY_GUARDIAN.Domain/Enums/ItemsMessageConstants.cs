using System.ComponentModel;

namespace MONEY_GUARDIAN.Domain.Enums
{
    public enum ItemsMessageConstants
    {
        [Description("GetBudgets")]
        GetBudgets,
        [Description("GetMonetaryFund")]
        GetMonetaryFund,
        [Description("GetExpenseType")]
        GetExpenseType,
        [Description("GetUsers")]
        GetUsers,
        [Description("GetDeposits")]
        GetDeposits,
        [Description("GetExpenseDetail")]
        GetExpenseDetail,
        [Description("GetReport")]
        GetReport,
        [Description("GetReportChart")]
        GetReportChart
    }
}
