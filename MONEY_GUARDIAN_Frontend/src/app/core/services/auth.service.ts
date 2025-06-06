import { Injectable, inject } from '@angular/core';
import { Auth, authState, GoogleAuthProvider, signInWithPopup, signOut, User } from '@angular/fire/auth';
import { Router } from '@angular/router';
import { map, Observable,BehaviorSubject } from 'rxjs';
import { UserService } from '../../shared/Service/user/user.service';
import { InformationUser } from '../../shared/interfaces/InformationUser.interface';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly auth = inject(Auth);
  private readonly router = inject(Router);
  private readonly userService = inject(UserService)
  private readonly messageService = inject(MessageService)

  private readonly isLocalLoggedIn$ = new BehaviorSubject<boolean>(this.checkLocalLogin());

  private checkLocalLogin(): boolean {
    return localStorage.getItem('local_login') === 'true';
  }

  async login(username: string, password: string): Promise<void> { 
    
    this.userService.ValidLoginUser({userName: username,password:password}).subscribe(
    {
      next: (response: InformationUser) => 
      {        
          
          localStorage.setItem('local_login', 'true');
          localStorage.setItem('local_username', username); 
          localStorage.setItem('local_userid', response.id); 
          this.isLocalLoggedIn$.next(true);
          this.router.navigate(['/home']);                  
      }, 
      error: (error) => {
        this.showError(error);
      }
    });        
  }

  async signInWithGoogle(): Promise<void> {
    try {
      const provider = new GoogleAuthProvider();
      const result = await signInWithPopup(this.auth, provider);

      const user = result.user;
      const userName = user.displayName;

      if (!userName) {
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: 'No se pudo obtener el nombre del usuario desde Google.',
        });
        return;
      }
      
      this.userService.RegisterUserGoogle({userName: userName}).subscribe(
      {
        next: (response: InformationUser) => 
        {        
          
          localStorage.removeItem('local_login');
          localStorage.setItem('local_userid', response.id); 
          this.router.navigate(['/home']);                
        }, 
        error: (error) => 
        {
          this.showError(error); 
        }      
      }); 
           
    } catch (error) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Error during sign in with Google: '+ error,
      });
    }
  }

  async signOut(): Promise<void> {
    try {
      await signOut(this.auth);
      localStorage.removeItem('local_login');
      localStorage.removeItem('local_username');
      localStorage.removeItem('local_userid');
      this.isLocalLoggedIn$.next(false);
      this.router.navigate(['/login']);

    } catch (error) {
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: 'Error during sign out: '+ error
      });
    }
  }

  isLoggedIn(): Observable<boolean> {
    return authState(this.auth).pipe(
      map(user => !!user),
      map(firebaseLoggedIn => firebaseLoggedIn || this.isLocalLoggedIn$.value)
    );
  }

  getUserName(): Observable<string | null> {
    return authState(this.auth).pipe(
      map((user: User | null) => {
        if (user?.displayName) 
        {
          return user?.displayName ?? null
        }
        else if (localStorage.getItem('local_login') === 'true') 
        {
          return localStorage.getItem('local_username'); 
        } 
        else {
          return null;
        }
      })
    );
  }

  getUserEmail(): Observable<string | null> {
    return authState(this.auth).pipe(map((user: User | null) => user?.email ?? null));
  }

  private showError(error: any): void {
    if(error?.status === 400)
    {              
      this.messageService.add({
        severity: 'error',
        summary: 'Error',
        detail: error.error.message,
      });
    }
    else{
      this.showMessage('error', 'Error', 'Algo salió mal, intente de nuevo');
    }   
  }

  private showMessage(severity: 'success' | 'info' | 'warn' | 'error', summary: string, detail: string): void {
    this.messageService.add({ severity, summary, detail });
  }
}
