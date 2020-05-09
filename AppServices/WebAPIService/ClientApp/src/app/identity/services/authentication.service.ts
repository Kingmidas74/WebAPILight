import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, throwError } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { StorageMap } from '@ngx-pwa/local-storage';
import { environment } from '../../../environments/environment';

import { JWTToken } from '../models/JWTToken';
import { UserRegistrationData } from '../models/userRegistrationData';

import * as uuid from 'uuid';


@Injectable({
  providedIn:'root'
})
export class AuthenticationService {

  private identityError:BehaviorSubject<string> = new BehaviorSubject<string>('');
  identityError$ = this.identityError.asObservable();
  public token:BehaviorSubject<JWTToken> = new BehaviorSubject<JWTToken>(new JWTToken());

  constructor(private httpClient:HttpClient, private storage: StorageMap) {};
  
  public sendTokenRequest(userPhone:string, userPassword:string):Observable<any> {  
    const body = new HttpParams()
      .set('grant_type', environment.identityService.user.grantType)
      .set('scope', environment.identityService.user.scope)
      .set('client_id', environment.identityService.user.clientId)
      .set('client_secret', environment.identityService.user.secret)
      .set('phone', userPhone)
      .set('password', userPassword);
    return this.httpClient.post('/connect/token',body).pipe(
      catchError((error)=>this.handleError(error)),
      tap(response => this.storage.set(environment.constants.JWTTokenStorageKey,response).subscribe(() => {}))
    );
  }

  public createIdentity(phone:string, email:string, password:string, confirmPassword:string):Observable<UserRegistrationData> {
    const body = new  UserRegistrationData();
    body.ConfirmPassword=confirmPassword;
    body.Password=password;
    body.Email=email;
    body.Phone=phone;
    body.Id=uuid.v4();
    
    return this.httpClient.post('/identity/createIdentity',body).pipe(
      catchError((error)=>this.handleError(error)),
      map(_=>body)
    );
  }

  public confirmIdentity(id:string, phone:string, password:string, code:string):Observable<any> {
    const body = {
      Id:id,
      Code:code,
      Redirect:'http://localhost:4200'
    };    
    return this.httpClient.post('/identity/confirmIdentity',body).pipe(
      catchError((error)=>this.handleError(error))
    );
  }

  handleError(error: HttpErrorResponse):Observable<string> {
    if(error.status>=400 && error.status<500) {
      console.error('An error occurred:', error.error.message, error);
      return throwError(error.message);
    }
    else {
      console.error(error);
      console.error(
        `Backend returned code ${error.status}, ` +
        `body was: ${error.error}`);
      return throwError('Unknown error');
    }
  };
}
