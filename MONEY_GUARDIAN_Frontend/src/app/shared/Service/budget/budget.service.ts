import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Budget } from '../../interfaces/Budget.interface';
import { CreateOrUpdateBudget } from '../../interfaces/CreateOrUpdateBudget.interface';

@Injectable()
export class BudgetService {  
  constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<Budget[]> {    
    return this.httpService.doPost<FieldFilter[],Budget[]>(
      `${environment.endpoint_api_Budget}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateBudget): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateBudget,unknown>(
      `${environment.endpoint_api_Budget}`,
      createReservation
    );
  }
  
  public update(createReservation: CreateOrUpdateBudget): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateBudget,unknown>(
      `${environment.endpoint_api_Budget}`,
      createReservation
    );
  }
}
