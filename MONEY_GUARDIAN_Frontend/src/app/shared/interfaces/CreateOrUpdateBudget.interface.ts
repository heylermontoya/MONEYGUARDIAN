export interface CreateOrUpdateBudget{
    id?:string;
    userId: string;
    expenseTypeId: string;    
    month:string;
    year:string;
    amount:string;
}