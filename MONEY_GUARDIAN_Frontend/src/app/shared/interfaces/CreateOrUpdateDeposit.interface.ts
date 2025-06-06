export interface CreateOrUpdateDeposit{
    id?:string;
    date: string;
    monetaryFundId: string;    
    amount: string;
    userId?: string;
}