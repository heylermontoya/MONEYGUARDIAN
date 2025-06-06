import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { ButtonModule } from "primeng/button";
import { FormsModule } from '@angular/forms';
import { DividerModule } from 'primeng/divider';
import { UserService } from '../../../shared/Service/user/user.service';
import { HttpService } from '../../../shared/Service/http-service/http.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ButtonModule,FormsModule,DividerModule ],
  providers:[
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {

  isLoggedIn = false;
  username = '';
  password = '';

  constructor(private readonly auth$: AuthService) {}

  loginWithGoogle(): void {
    this.auth$.signInWithGoogle();
  } 
  
  loginWithUsername() {
    this.auth$.login(this.username, this.password);    
  }
}
