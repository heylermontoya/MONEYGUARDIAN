export interface Expense{
    id:string;
    date: string | Date;
    monetaryFundId: string;
    monetaryFundName: string;    
    observation: string;
    merchant: string;
    documentType: string;    
    userId: string;
    userName: string;

    details: ExpenseDetail[]; 

}

export interface ExpenseDetail {
    expenseDetailId:string;
    expenseTypeId: string;
    expenseTypeName: string;
    amount: number;
}
