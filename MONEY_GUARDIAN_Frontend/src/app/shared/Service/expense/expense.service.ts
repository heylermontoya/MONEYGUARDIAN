import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Expense } from '../../interfaces/Expense.interface';
import { CreateOrUpdateExpense } from '../../interfaces/CreateOrUpdateExpense.interface';

@Injectable()
export class ExpenseService {

  
  constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<Expense[]> {    
    return this.httpService.doPost<FieldFilter[],Expense[]>(
      `${environment.endpoint_api_Expense}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateExpense): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateExpense,unknown>(
      `${environment.endpoint_api_Expense}`,
      createReservation
    );
  }
  
  public update(createReservation: CreateOrUpdateExpense): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateExpense,unknown>(
      `${environment.endpoint_api_Expense}`,
      createReservation
    );
  }
}
