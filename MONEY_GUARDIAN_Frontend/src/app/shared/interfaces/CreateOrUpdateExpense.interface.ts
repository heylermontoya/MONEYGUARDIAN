export interface CreateOrUpdateExpense{
    id?:string;
    date: string | Date;
    monetaryFundId: string;    
    monetaryFundName: string;
    userId: string;
    userName: string;
    observation: string;
    merchant: string;
    documentType: string;
    
    details: CreateOrUpdateExpenseDetail[]; 
}

export interface CreateOrUpdateExpenseDetail {
    id?:string;
    expenseTypeId: string;
    amount: number;
}
