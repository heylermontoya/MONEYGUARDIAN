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
import { InformationUser } from '../../shared/interfaces/InformationUser.interface';
import { UserService } from '../../shared/Service/user/user.service';
import { FormUserComponent } from './component/form-user/form-user.component';

@Component({
  selector: 'app-user',
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
      UserService,
      HttpService
    ],
  templateUrl: './user.component.html',
  styleUrl: './user.component.scss'
})
export class UserComponent implements OnInit {
  infoTable!: InformationUser[];
  loading = true;
  ref!: DynamicDialogRef;
  filters: FieldFilter[] = [];

  constructor(
    private readonly service: UserService, 
    public dialogService: DialogService,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService
  ) {}

  ngOnInit() {
    this.getinfoTable();
  } 

  getinfoTable(){
    this.service.getList(this.filters).subscribe({
      next: (response: InformationUser[]) => {
        this.infoTable = response;
        this.loading = false;                
      }, 
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    })
  }
  
  edit(
    infoComponent: InformationUser
  ){
    this.ref = this.dialogService.open(FormUserComponent, {
      data: { 
        action:'actualizar',
        id: infoComponent.id,
        userName: infoComponent.userName,
        name: infoComponent.name
      },
      width: '90%',
      height: '100%',
      contentStyle: { "max-height": "700px", "overflow": "auto" }
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
