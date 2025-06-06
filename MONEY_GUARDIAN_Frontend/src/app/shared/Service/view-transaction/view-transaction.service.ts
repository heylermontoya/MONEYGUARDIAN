import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { ViewTransactions } from '../../interfaces/ViewTransactions.interface';
import { InfoChart } from '../../interfaces/InfoChart.interface';

@Injectable({
  providedIn: 'root'
})
export class ViewTransactionService {
  constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<ViewTransactions[]> {    
    return this.httpService.doPost<FieldFilter[],ViewTransactions[]>(
      `${environment.endpoint_view_transaction}/list`,
      data
    );
  }
  
  public getInfoChart(data: FieldFilter[]): Observable<InfoChart[]> {    
    return this.httpService.doPost<FieldFilter[],InfoChart[]>(
      `${environment.endpoint_view_transaction}/infoChart`,
      data
    );
  }

}
