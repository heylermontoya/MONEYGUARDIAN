import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { CalendarModule } from 'primeng/calendar';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { TableModule } from 'primeng/table';
import { UserService } from '../../../../shared/Service/user/user.service';
import { CreateOrUpdateUser } from '../../../../shared/interfaces/CreateOrUpdateUser.interface';
import { InputTextModule } from 'primeng/inputtext';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-form-user',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    CalendarModule,
    ConfirmDialogModule,
    InputTextModule
  ],
  templateUrl: './form-user.component.html',
  styleUrl: './form-user.component.scss'
})
export class FormUserComponent implements OnInit {
  componentForm: FormGroup;
    requiredField = 'Campo obligatorio';
    headerName = '';
    action = '';
    id= '';
    loading = false;
    possitiveNumber = 'Positive number greater than or equal to zero';
    possitiveNumberInt = 'Positive number integer greater than or equal to zero';
    possitiveNumberIntGreatOne = 'Positive number integer greater than or equal to one';
  
  
    constructor(    
      private readonly formBuilder: FormBuilder, 
      public ref: DynamicDialogRef,
      public config: DynamicDialogConfig,
      private readonly service: UserService,     
      private readonly messageService: MessageService
    ) {
      this.componentForm = this.formBuilder.group({      
        userName: ['', Validators.required],
        name: ['', Validators.required],      
      });
    }
  
    ngOnInit() {
  
      this.action = this.config.data.action;
  
      this.headerName = this.config.data.name;
  
      if (this.action === 'actualizar'){
        this.id = this.config.data.id;
        this.componentForm.patchValue({
          userName: this.config.data.userName,
          name: this.config.data.name
        });
      } 
    }
  
    onSubmit() {
      if (this.componentForm.valid) {
        if(this.action === 'crear') {
          const createParser = this.createParserFormData();
          this.service.create(createParser).subscribe({
            next: () => {
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
  
    private createParserFormData(): CreateOrUpdateUser{
      return {
        userName: this.componentForm.value.userName,
        name: this.componentForm.value.name      
      }
    }
  
    private updateParserFormData():CreateOrUpdateUser{
      return {
        id: this.id,      
        userName: this.componentForm.value.userName,
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
