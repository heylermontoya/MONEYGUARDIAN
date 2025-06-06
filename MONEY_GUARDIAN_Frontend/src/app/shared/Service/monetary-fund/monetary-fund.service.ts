import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { MonetaryFund } from '../../interfaces/MonetaryFund.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { CreateOrUpdateMonetaryFund } from '../../interfaces/CreateOrUpdateMonetaryFund.interface';

@Injectable()
export class MonetaryFundService {
constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<MonetaryFund[]> {    
    return this.httpService.doPost<FieldFilter[],MonetaryFund[]>(
      `${environment.endpoint_api_MonetaryFund}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateMonetaryFund): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateMonetaryFund,unknown>(
      `${environment.endpoint_api_MonetaryFund}`,
      createReservation
    );
  }
  
  public update(createReservation: CreateOrUpdateMonetaryFund): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateMonetaryFund,unknown>(
      `${environment.endpoint_api_MonetaryFund}`,
      createReservation
    );
  }
}
