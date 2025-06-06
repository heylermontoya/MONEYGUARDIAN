import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { TableModule } from 'primeng/table';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { FieldFilter } from '../../shared/interfaces/FieldFilter.interface';
import { ConfirmationService, MessageService } from 'primeng/api';
import { HttpService } from '../../shared/Service/http-service/http.service';
import { UserService } from '../../shared/Service/user/user.service';
import { ViewTransactions } from '../../shared/interfaces/ViewTransactions.interface';
import { ViewTransactionService } from '../../shared/Service/view-transaction/view-transaction.service';

@Component({
  selector: 'app-view-transactions',
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
  providers:
  [
    DialogService,
    ConfirmationService,
    UserService,
    HttpService
  ],
  templateUrl: './view-transactions.component.html',
  styleUrl: './view-transactions.component.scss'
})
export class ViewTransactionsComponent implements OnInit {
  infoTable!: ViewTransactions[];
  loading = true;
  ref!: DynamicDialogRef;
  filters: FieldFilter[] = [];
  selectedDateRange!: Date[];

  constructor(
    private readonly service: ViewTransactionService, 
    public dialogService: DialogService,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit() {
    this.getinfoTable();
  } 
  
  getinfoTable(){
    this.service.getList(this.filters).subscribe({
      next: (response: ViewTransactions[]) => {
        this.infoTable = response;
        this.loading = false;                
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    })
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

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }

  onDateFilter(dates: Date[],field: string) {
    if (dates && dates.length === 2) {
      const [startDate, endDate] = dates;

      startDate.setHours(0, 0, 0, 0);

      endDate.setHours(23, 59, 59, 999);
      
      const formattedStart = this.formatDate(startDate);
      const formattedEnd = this.formatDate(endDate);

      const data = {
        field: field,
        value: formattedStart,
        EndDate: formattedEnd,
        typeDateTime: 0
      };

      const indiceExist = this.filters.findIndex(item => item.field === data.field);
                
      if (indiceExist !== -1) {
          this.filters.splice(indiceExist, 1);
      }

      if (data.value) {
          this.filters.push(data);
      }
  
      this.getinfoTable(); 
    }
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
