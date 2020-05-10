import { CanLoad, Route, UrlSegment, CanActivateChild, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree} from '@angular/router/router';
import { Injectable } from '@angular/core';
import { AuthenticationService } from './services/authentication.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class CanLoadIdentity implements CanLoad, CanActivateChild {
  
  constructor(private authenticationService:AuthenticationService) {}

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean>|Promise<boolean>|boolean {    
    return this.authenticationService.IsAuthenticatedUser().pipe(map(x=>!x));
  }

  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean|UrlTree>|Promise<boolean|UrlTree>|boolean|UrlTree {    
    return this.authenticationService.IsAuthenticatedUser().pipe(map(x=>!x));
  }
}