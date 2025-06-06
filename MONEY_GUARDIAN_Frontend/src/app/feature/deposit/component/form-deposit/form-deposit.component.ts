import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { DepositService } from '../../../../shared/Service/deposit/deposit.service';
import { CreateOrUpdateDeposit } from '../../../../shared/interfaces/CreateOrUpdateDeposit.interface';
import { MonetaryFund } from '../../../../shared/interfaces/MonetaryFund.interface';
import { MonetaryFundService } from '../../../../shared/Service/monetary-fund/monetary-fund.service';
import { DropdownModule } from 'primeng/dropdown';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { RegularExpressions } from '../../../../shared/constant/regex';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-form-deposit',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    CalendarModule,
    ConfirmDialogModule,
    DropdownModule,
    InputTextModule,
    ProgressSpinnerModule
  ],
  templateUrl: './form-deposit.component.html',
  styleUrl: './form-deposit.component.scss'
})
export class FormDepositComponent implements OnInit {
componentForm: FormGroup;
  requiredField = 'Campo obligatorio';
  headerName = '';
  action = '';
  id= '';
  loading = false;
  calendarReady = false;
  possitiveNumberIntGreatZero = 'Debe ser un número mayor o igual a cero';
  possitiveNumberInt = 'Debe ser un número entero positivo';

  listMonetaryFund: MonetaryFund[] = [];

  constructor(    
    private readonly formBuilder: FormBuilder, 
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private readonly service: DepositService,     
    private readonly monetaryFundService: MonetaryFundService,  
    private readonly messageService: MessageService,        
  ) {
    this.componentForm = this.formBuilder.group({      
      date: ['', Validators.required],
      monetaryFundId: ['', Validators.required],      
      amount: ['', [Validators.required, Validators.pattern(RegularExpressions.NUMERIC)]]
    });
  }

  ngOnInit() {
    setTimeout(() => {
      this.calendarReady = true;
    });

    this.loadInformation();    

    this.action = this.config.data.action;

    this.headerName = this.config.data.name;

    if (this.action === 'actualizar'){
      this.id = this.config.data.id;
      this.componentForm.patchValue({
        date: this.config.data.date,
        monetaryFundId: this.config.data.monetaryFundId,
        amount: this.config.data.amount,
      });
    } 
  }

  loadInformation(){
    this.getMonetaryFund();    
  }

  getMonetaryFund(){
    this.loading = true;
    this.monetaryFundService.getList([]).subscribe({
      next: response => {
        this.loading = false;
        this.listMonetaryFund = response;        
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    });
  }

  onSubmit() {
    if (this.componentForm.valid) {
      this.loading = true;
      if(this.action === 'crear') {
        const createParser = this.createParserFormData();
        this.service.create(createParser).subscribe({
          next: () => {
            this.loading = false;
            this.onClose();   
          }, 
          error: (error) => {
            this.loading = false;
            this.showError(error);
          }
        });
      } else{
        const updateParser = this.updateParserFormData();
        this.service.update(updateParser).subscribe({
          next: () => {
            this.loading = false;
            this.onClose();   
          }, 
          error: (error) => {
            this.loading = false;
            this.showError(error);
          }
        });
      }
    }
  }

  onClose() {
    this.componentForm.reset();
    this.ref.close();
  }

  private createParserFormData(): CreateOrUpdateDeposit{
    return {
      date: this.componentForm.value.date,
      monetaryFundId: this.componentForm.value.monetaryFundId,      
      amount: this.componentForm.value.amount,      
      userId: localStorage.getItem('local_userid')!
    }
  }

  private updateParserFormData():CreateOrUpdateDeposit{
    return {
      id: this.id,      
      date: this.componentForm.value.date,
      monetaryFundId: this.componentForm.value.monetaryFundId,      
      amount: this.componentForm.value.amount
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
