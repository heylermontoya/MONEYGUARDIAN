import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ToolbarModule } from "primeng/toolbar";
import { AvatarModule } from "primeng/avatar";
import { CommonModule } from "@angular/common";
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { DividerModule } from 'primeng/divider';
import { MenuItem } from 'primeng/api';
import { PanelMenuModule } from 'primeng/panelmenu';

@Component({
  selector: 'app-header',  
  standalone: true,
  imports: [
    CommonModule,
    SidebarModule,    
    AvatarModule,
    ButtonModule,
    DividerModule,
    ToolbarModule,
    PanelMenuModule 
  ],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class HeaderComponent implements OnInit{
  sidebarVisible = false;
  dropdownVisible = false;
  userName = '';
  isLogged= true;

  menuItems: MenuItem[] = [
  {
    label: 'Mantenimientos',
    items: [
      { label: 'Tipos de gastos', routerLink: ['/ExpenseType'] },
      { label: 'Fondos monetarios', routerLink: ['/MonetaryFund'] }
    ]
  },
  {
    label: 'Movimientos',
    items: [
      { label: 'Presupuesto por tipo de gasto', routerLink: ['/Budget'] },
      { label: 'Registros de gastos', routerLink: ['/Expense'] },
      { label: 'Depósitos', routerLink: ['/Deposit'] }
    ]
  },  
  {
    label: 'Consultas y reportes',
    items: [
      { label: 'Consulta de movimientos', routerLink: ['/ViewTransactions'] },
      { label: 'Gráfico comparativo de presupuesto y ejecución', routerLink: ['/ChartTransaction'] }
    ]
  }
];

  constructor(private readonly auth$: AuthService) {}

  ngOnInit() 
  {        
      this.auth$.getUserName().subscribe(name => {        
        this.userName = name!;
        if(this.userName === null){
          this.isLogged= false;
        } else{
          this.isLogged= true;
        }        
      });        
  }

  logout(){
    this.auth$.signOut();
    this.isLogged= false;
    this.dropdownVisible = false;
  }

  toggleDropdown(){
    this.dropdownVisible = !this.dropdownVisible;
  }

  toggleSidebar() {
    this.sidebarVisible = !this.sidebarVisible;
  }
}
