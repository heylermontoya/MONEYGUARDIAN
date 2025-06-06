import { Injectable } from '@angular/core';
import { HttpService } from '../http-service/http.service';
import { FieldFilter } from '../../interfaces/FieldFilter.interface';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { InformationUser } from '../../interfaces/InformationUser.interface';
import { CreateOrUpdateUser } from '../../interfaces/CreateOrUpdateUser.interface';
import { LoginUser } from '../../interfaces/LoginUser.interface';
import { LoginUserWithGoogle } from '../../interfaces/LoginUserWithGoogle.interface';

@Injectable({
  providedIn: 'root'  // <-- Asegura que Angular lo puede inyectar globalmente
})
export class UserService {
 constructor(private readonly httpService: HttpService) { }

  public getList(data: FieldFilter[]): Observable<InformationUser[]> {    
    return this.httpService.doPost<FieldFilter[],InformationUser[]>(
      `${environment.endpoint_api_user}/list`,
      data
    );
  }

  public create(createReservation: CreateOrUpdateUser): Observable<unknown> {
    return this.httpService.doPost<CreateOrUpdateUser,unknown>(
      `${environment.endpoint_api_user}`,
      createReservation
    );
  }
  
  public ValidLoginUser(loginUser: LoginUser): Observable<InformationUser> {
    return this.httpService.doPost<LoginUser,InformationUser>(
      `${environment.endpoint_api_user}/loginUser`,
        loginUser
    );
  }
  
  public RegisterUserGoogle(loginUser: LoginUserWithGoogle): Observable<InformationUser> {
    return this.httpService.doPost<LoginUserWithGoogle,InformationUser>(
      `${environment.endpoint_api_user}/registerUserGoogle`,
        loginUser
    );
  }
  
  
  
  public update(createReservation: CreateOrUpdateUser): Observable<unknown> {
    return this.httpService.doPut<CreateOrUpdateUser,unknown>(
      `${environment.endpoint_api_user}`,
      createReservation
    );
  }
}
