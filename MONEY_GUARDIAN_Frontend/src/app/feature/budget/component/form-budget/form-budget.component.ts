import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { DropdownModule } from 'primeng/dropdown';
import { BudgetService } from '../../../../shared/Service/budget/budget.service';
import { CreateOrUpdateBudget } from '../../../../shared/interfaces/CreateOrUpdateBudget.interface';
import { UserService } from '../../../../shared/Service/user/user.service';
import { ExpenseTypeService } from '../../../../shared/Service/expense-type/expense-type.service';
import { InformationUser } from '../../../../shared/interfaces/InformationUser.interface';
import { ExpenseType } from '../../../../shared/interfaces/ExpenseType.interface';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { RegularExpressions } from '../../../../shared/constant/regex';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-form-budget',
  standalone: true,
  imports: [
    InputTextModule,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    CalendarModule,
    ConfirmDialogModule,
    DropdownModule,
    ProgressSpinnerModule
  ],
  templateUrl: './form-budget.component.html',
  styleUrl: './form-budget.component.scss'
})
export class FormBudgetComponent implements OnInit {
componentForm: FormGroup;
  requiredField = 'Campo obligatorio';
  headerName = '';
  action = '';
  id= '';
  loading = false;

  possitiveNumberInt = 'Debe ser un número entero positivo';

  possitiveNumberIntGreatZero = 'Debe ser un número mayor o igual a cero';
  possitiveNumberIntGreat2020 = 'Debe ser un número mayor o igual a 2020';
  possitiveNumberIntGreatOne = 'Debe ser un número mayor o igual a uno';
  possitiveNumberIntLessTwelve = 'Debe ser un número menor o igual a doce';

  listUser: InformationUser[] = [];
  listexpenseType: ExpenseType[] = [];

  constructor(    
    private readonly formBuilder: FormBuilder, 
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private readonly service: BudgetService,     
    private readonly userService: UserService,     
    private readonly expenseTypeService: ExpenseTypeService,     
    private readonly messageService: MessageService,     
  ) {
    this.componentForm = this.formBuilder.group({      
      userId: ['', Validators.required],
      expenseTypeId: ['', Validators.required],      
      amount: ['', [Validators.required,Validators.min(0),Validators.pattern(RegularExpressions.NUMERIC)]],
      year: ['', [Validators.required,Validators.min(2020),Validators.pattern(RegularExpressions.NUMERIC)]],
      month: ['', [Validators.required,Validators.min(1),Validators.max(12),Validators.pattern(RegularExpressions.NUMERIC)]]
    });
  }

  ngOnInit() {
    this.loadInformation();    

    this.action = this.config.data.action;

    this.headerName = this.config.data.name;

    if (this.action === 'actualizar'){
      this.id = this.config.data.id;
      this.componentForm.patchValue({
        month: this.config.data.month,
        year: this.config.data.year,
        amount: this.config.data.amount,
        userId: this.config.data.userId,
        expenseTypeId: this.config.data.expenseTypeId,
      });
    } 
  }

  loadInformation(){
    this.getUser();    
    this.getExpenseType();    
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
        this.listexpenseType = response;        
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
          }, error: (error) => {
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
          }, error: (error) => {
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

  private createParserFormData(): CreateOrUpdateBudget{
    return {
      expenseTypeId: this.componentForm.value.expenseTypeId,
      userId: this.componentForm.value.userId,      
      month: this.componentForm.value.month,      
      year: this.componentForm.value.year,      
      amount: this.componentForm.value.amount,      
    }
  }

  private updateParserFormData():CreateOrUpdateBudget{
    return {
      id: this.id,      
      expenseTypeId: this.componentForm.value.expenseTypeId,
      userId: this.componentForm.value.userId,      
      month: this.componentForm.value.month,      
      year: this.componentForm.value.year,      
      amount: this.componentForm.value.amount,      
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
