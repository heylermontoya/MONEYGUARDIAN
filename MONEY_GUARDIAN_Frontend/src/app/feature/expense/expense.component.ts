import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FieldFilter } from '../../shared/interfaces/FieldFilter.interface';
import { ConfirmationService, MessageService } from 'primeng/api';
import { HttpService } from '../../shared/Service/http-service/http.service';
import { Expense } from '../../shared/interfaces/Expense.interface';
import { ExpenseService } from '../../shared/Service/expense/expense.service';
import { FormExpenseComponent } from './component/form-expense/form-expense.component';
import { MonetaryFundService } from '../../shared/Service/monetary-fund/monetary-fund.service';
import { ExpenseTypeService } from '../../shared/Service/expense-type/expense-type.service';
import { UserService } from '../../shared/Service/user/user.service';

@Component({
  selector: 'app-expense',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    CalendarModule,
    ConfirmDialogModule
  ],
  providers:[
    DialogService,
    ConfirmationService,
    ExpenseService,
    MonetaryFundService,
    UserService,
    ExpenseTypeService,
    HttpService
  ],
  templateUrl: './expense.component.html',
  styleUrl: './expense.component.scss'
})
export class ExpenseComponent implements OnInit {
  infoTable!: Expense[];
  loading = true;
  ref!: DynamicDialogRef;
  filters: FieldFilter[] = [];

  constructor(
    private readonly service: ExpenseService, 
    public dialogService: DialogService,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService     
  ) {}
  
  ngOnInit() {
    this.getinfoTable();
  } 
  
  getinfoTable(){
    this.service.getList(this.filters).subscribe({
      next: (response: Expense[]) => {
        this.infoTable = response;

        this.infoTable = response.map(e => ({
          ...e,
          details: e.details ?? []
        }));

        this.loading = false;                
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    })
  }
  
  new(){
    this.ref = this.dialogService.open(FormExpenseComponent, {
      data: { action:'crear' },
      width: '90%',
      height: '100%',
      contentStyle: { "max-height": "700px", "overflow": "auto" },
      dismissableMask: true 
    });

    this.ref.onClose.subscribe(() => {      
        this.getinfoTable();      
    });    
  }
  
  edit(
    infoComponent: Expense
  ){
    this.ref = this.dialogService.open(FormExpenseComponent, {
      data: { 
        action:'actualizar',
        id: infoComponent.id,
        date: infoComponent.date,
        monetaryFundId: infoComponent.monetaryFundId,
        monetaryFundName: infoComponent.monetaryFundName,
        userId: infoComponent.userId,
        userName: infoComponent.userName,
        observation: infoComponent.observation,
        merchant: infoComponent.merchant,
        documentType: infoComponent.documentType,
        details: infoComponent.details
      },
      width: '90%',
      height: '100%',
      contentStyle: { "max-height": "700px", "overflow": "auto" },
      dismissableMask: true 
    });

    this.ref.onClose.subscribe(() => {      
      this.getinfoTable();      
    });
  }

  onColumnFilter(event: Event, field: string) {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();

    const data = {
      field: field,
      value : value
    }

    const indiceExist = this.filters.findIndex(item => item.field === data.field);

    if(indiceExist !== -1) {
      this.filters.splice(indiceExist, 1);
    }

    if(data.value){
      this.filters.push(data);
    }

    this.getinfoTable(); 
  }  

  private showError(error: any): void {
    if(error?.status === 400)
    {              
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: error.error.message,
      });
    }
    else{
      this.showMessage('error', 'Error', 'Algo salió mal, intente de nuevo');
    }   
  }

  private showMessage(severity: 'success' | 'info' | 'warn' | 'error', summary: string, detail: string): void {
    this.messageService.add({ severity, summary, detail });
  }
}
