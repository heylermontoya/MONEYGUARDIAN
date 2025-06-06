import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { map, first } from 'rxjs/operators';

@Injectable({
    providedIn: 'root'
  })
export class AuthGuard implements CanActivate {

  constructor(private readonly auth$: AuthService, private readonly router: Router) {}

  canActivate(): Observable<boolean> {
    return this.auth$.isLoggedIn().pipe(
      first(),  
      map(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['/login']);
          return false;
        }
        return true;
      })
    );
  }
}
