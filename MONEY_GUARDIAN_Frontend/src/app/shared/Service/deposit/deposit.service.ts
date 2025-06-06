import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { Deposit } from '../../interfaces/Deposit.interface';
import { CreateOrUpdateDeposit } from '../../interfaces/CreateOrUpdateDeposit.interface';

@Injectable()
export class DepositService {

  constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<Deposit[]> {    
    return this.httpService.doPost<FieldFilter[],Deposit[]>(
      `${environment.endpoint_api_deposit}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateDeposit): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateDeposit,unknown>(
      `${environment.endpoint_api_deposit}`,
      createReservation
    );
  }
  
  public update(createReservation: CreateOrUpdateDeposit): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateDeposit,unknown>(
      `${environment.endpoint_api_deposit}`,
      createReservation
    );
  }
}
