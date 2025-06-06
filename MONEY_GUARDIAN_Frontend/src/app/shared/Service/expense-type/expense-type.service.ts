import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { ExpenseType } from '../../interfaces/ExpenseType.interface';
import { Observable } from 'rxjs';
import { CreateOrUpdateExpenseType } from '../../interfaces/CreateOrUpdateExpenseType.interface';

@Injectable()
export class ExpenseTypeService {

  constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<ExpenseType[]> {    
    return this.httpService.doPost<FieldFilter[],ExpenseType[]>(
      `${environment.endpoint_api_ExpenseType}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateExpenseType): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateExpenseType,unknown>(
      `${environment.endpoint_api_ExpenseType}`,
      createReservation
    );
  }
  
  public update(createReservation: CreateOrUpdateExpenseType): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateExpenseType,unknown>(
      `${environment.endpoint_api_ExpenseType}`,
      createReservation
    );
  }  
}
