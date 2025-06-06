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
import { MonetaryFund } from '../../shared/interfaces/MonetaryFund.interface';
import { MonetaryFundService } from '../../shared/Service/monetary-fund/monetary-fund.service';
import { FormMonetaryFundComponent } from './component/form-monetary-fund/form-monetary-fund.component';

@Component({
  selector: 'app-monetary-fund',
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
    MonetaryFundService,
    HttpService
  ],
  templateUrl: './monetary-fund.component.html',
  styleUrl: './monetary-fund.component.scss'
})
export class MonetaryFundComponent implements OnInit {
 infoTable!: MonetaryFund[];
  loading = true;
  ref!: DynamicDialogRef;
  filters: FieldFilter[] = [];

  constructor(
    private readonly service: MonetaryFundService, 
    public dialogService: DialogService,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit() {
    this.getinfoTable();
  } 

  getinfoTable(){
    this.service.getList(this.filters).subscribe({
      next: (response: MonetaryFund[]) => {
        this.infoTable = response;
        this.loading = false;                
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    })
  }
  
  new(){
    this.ref = this.dialogService.open(FormMonetaryFundComponent, {
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
    infoComponent: MonetaryFund
  ){
    this.ref = this.dialogService.open(FormMonetaryFundComponent, {
      data: { 
        action:'actualizar',
        id: infoComponent.id,
        type: infoComponent.type,
        name: infoComponent.name
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
