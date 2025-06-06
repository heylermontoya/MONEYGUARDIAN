import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
    {
        path:'home',
        loadComponent: () => import('./feature/home/home.component').then(m => m.HomeComponent),
        canActivate: [AuthGuard]  
    },
    { 
        path: 'ExpenseType', 
        loadComponent:()=> import('./feature/expense-type/expense-type.component').then((m) => m.ExpenseTypeComponent),
        canActivate: [AuthGuard]           
    },
    { 
        path: 'MonetaryFund', 
        loadComponent:()=> import('./feature/monetary-fund/monetary-fund.component').then((m) => m.MonetaryFundComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'users', 
        loadComponent:()=> import('./feature/user/user.component').then((m) => m.UserComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'Deposit', 
        loadComponent:()=> import('./feature/deposit/deposit.component').then((m) => m.DepositComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'Expense', 
        loadComponent:()=> import('./feature/expense/expense.component').then((m) => m.ExpenseComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'Budget', 
        loadComponent:()=> import('./feature/budget/budget.component').then((m) => m.BudgetComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'ViewTransactions', 
        loadComponent:()=> import('./feature/view-transactions/view-transactions.component').then((m) => m.ViewTransactionsComponent),
        canActivate: [AuthGuard]
    },
    { 
        path: 'ChartTransaction', 
        loadComponent:()=> import('./feature/chart-transaction/chart-transaction.component').then((m) => m.ChartTransactionComponent),
        canActivate: [AuthGuard]
    },
    { path: 'login', loadComponent: () => import('./core/component/login/login.component').then(m => m.LoginComponent) },
    { path: '', redirectTo: '/home', pathMatch: 'full' },
    { path: '**', redirectTo: '/home' }
];
