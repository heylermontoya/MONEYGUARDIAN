import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { MonetaryFund } from '../../../../shared/interfaces/MonetaryFund.interface';
import { MonetaryFundService } from '../../../../shared/Service/monetary-fund/monetary-fund.service';
import { DropdownModule } from 'primeng/dropdown';
import { ExpenseService } from '../../../../shared/Service/expense/expense.service';
import { CreateOrUpdateExpense } from '../../../../shared/interfaces/CreateOrUpdateExpense.interface';
import { UserService } from '../../../../shared/Service/user/user.service';
import { InformationUser } from '../../../../shared/interfaces/InformationUser.interface';
import { ExpenseType } from '../../../../shared/interfaces/ExpenseType.interface';
import { ExpenseTypeService } from '../../../../shared/Service/expense-type/expense-type.service';
import { InputTextModule } from 'primeng/inputtext';
import { DividerModule } from 'primeng/divider';
import { ExpenseDetail } from '../../../../shared/interfaces/Expense.interface';
import { MessageService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { RegularExpressions } from '../../../../shared/constant/regex';

@Component({
  selector: 'app-form-expense',
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
    DividerModule,
    ProgressSpinnerModule
  ],
  templateUrl: './form-expense.component.html',
  styleUrl: './form-expense.component.scss'
})
export class FormExpenseComponent implements OnInit {
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
  listUser: InformationUser[] = [];
  listExpenseType: ExpenseType[] = [];

  constructor(    
    private readonly formBuilder: FormBuilder, 
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private readonly service: ExpenseService,     
    private readonly monetaryFundService: MonetaryFundService,     
    private readonly userService: UserService,     
    private readonly expenseTypeService: ExpenseTypeService,     
    private readonly messageService: MessageService,     
  ) {
    this.componentForm = this.formBuilder.group({      
      date: ['', Validators.required],
      monetaryFundId: ['', Validators.required],      
      observation: ['',  [Validators.required, Validators.maxLength(20)]],      
      merchant: ['',  [Validators.required, Validators.maxLength(20)]],      
      documentType: ['',  [Validators.required, Validators.maxLength(20)]],      
      userId: ['', Validators.required],       
      details: this.formBuilder.array([this.createDetalleGroup()])
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
        userId: this.config.data.userId,
        observation: this.config.data.observation,
        merchant: this.config.data.merchant,
        documentType: this.config.data.documentType
      });

      this.details.clear();

      this.config.data.details.forEach((detalle: ExpenseDetail) => {
        const detalleGroup = this.formBuilder.group({
          expenseDetailId: [detalle.expenseDetailId],
          expenseTypeId: [detalle.expenseTypeId, Validators.required],
          amount: [detalle.amount, [Validators.required, Validators.pattern('^[0-9]+$')]]
        });

        this.details.push(detalleGroup);
      });

    }
  }

  createDetalleGroup(): FormGroup {
    return this.formBuilder.group({
      expenseDetailId: [null],
      expenseTypeId: [null, Validators.required],
      amount: ['', [Validators.required, Validators.pattern(RegularExpressions.NUMERIC)]]
    });
  }

  get details(): FormArray {
    return this.componentForm.get('details') as FormArray;
  }

  addDetalle() {
    this.details.push(this.createDetalleGroup());
  }

  removeDetalle(index: number) {
    this.details.removeAt(index);
  }

  loadInformation(){
    this.getMonetaryFund();    
    this.getUser();    
    this.getExpenseType();    
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
  
  getUser(){
    this.loading = true;
    this.userService.getList([]).subscribe({
      next: response => {
        this.loading = false;
        this.listUser = response;        
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    });
  }
  
  getExpenseType(){
    this.loading = true;
    this.expenseTypeService.getList([]).subscribe({
      next: response => {
        this.loading = false;
        this.listExpenseType = response;        
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

  private createParserFormData(): CreateOrUpdateExpense{
    debugger;
    return {      
      date: this.componentForm.value.date,
      monetaryFundId:this.componentForm.value.monetaryFundId,
      monetaryFundName: this.componentForm.value.monetaryFundName,
      userId: this.componentForm.value.userId,
      userName: this.componentForm.value.userName,
      observation: this.componentForm.value.observation,
      merchant: this.componentForm.value.merchant,
      documentType: this.componentForm.value.documentType,

      details:this.componentForm.value.details
    }
  }

  private updateParserFormData():CreateOrUpdateExpense{
    return {
      id: this.id,      
      date: this.componentForm.value.date,
      monetaryFundId:this.componentForm.value.monetaryFundId,
      monetaryFundName: this.componentForm.value.monetaryFundName,
      userId: this.componentForm.value.userId,
      userName: this.componentForm.value.userName,
      observation: this.componentForm.value.observation,
      merchant: this.componentForm.value.merchant,
      documentType: this.componentForm.value.documentType,

      details:this.componentForm.value.details
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
