import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { ExpenseTypeService } from '../../../../shared/Service/expense-type/expense-type.service';
import { CreateOrUpdateExpenseType } from '../../../../shared/interfaces/CreateOrUpdateExpenseType.interface';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-form-expense-type',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    CalendarModule,
    ConfirmDialogModule,
    InputTextModule,
    ProgressSpinnerModule
  ],
  templateUrl: './form-expense-type.component.html',
  styleUrl: './form-expense-type.component.scss'
})
export class FormExpenseTypeComponent implements OnInit {
  componentForm: FormGroup;
  requiredField = 'Campo obligatorio';
  headerName = '';
  action = '';
  id= '';
  loading = false;

  constructor(    
    private readonly formBuilder: FormBuilder, 
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private readonly service: ExpenseTypeService,   
    private readonly messageService: MessageService,       
  ) {
    this.componentForm = this.formBuilder.group({      
      name: ['',  [Validators.required, Validators.maxLength(10)]],      
    });
  }

  ngOnInit() {
    this.action = this.config.data.action;
    this.headerName = this.config.data.name;

    if (this.action === 'actualizar'){
      this.id = this.config.data.id;
      this.componentForm.patchValue({
        name: this.config.data.name
      });
    } 
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
      }
      else{
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

  private createParserFormData(): CreateOrUpdateExpenseType{
    return {
      name: this.componentForm.value.name      
    }
  }

  private updateParserFormData():CreateOrUpdateExpenseType{
    return {
      id: this.id,      
      name: this.componentForm.value.name   
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
