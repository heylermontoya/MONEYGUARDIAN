import { Component, OnInit } from '@angular/core';
import { ChartModule } from 'primeng/chart';
import { CalendarModule } from 'primeng/calendar';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogService } from 'primeng/dynamicdialog';
import { ViewTransactionService } from '../../shared/Service/view-transaction/view-transaction.service';
import { FieldFilter } from '../../shared/interfaces/FieldFilter.interface';
import { InfoChart } from '../../shared/interfaces/InfoChart.interface';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@Component({
  selector: 'app-chart-transaction',
  standalone: true,
  imports: [
    ChartModule,
    CalendarModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    ConfirmDialogModule,
    ProgressSpinnerModule
  ],
  providers:[
    DialogService,
    ConfirmationService
  ],
  templateUrl: './chart-transaction.component.html',
  styleUrl: './chart-transaction.component.scss'
})
export class ChartTransactionComponent  implements OnInit{
  selectedDateRange: Date[] = [];
  chartData: any;
  chartOptions: any;
  filters: FieldFilter[] = [];
  loading = false;

  constructor(
    private readonly service: ViewTransactionService, 
    public dialogService: DialogService,
    private readonly confirmationService: ConfirmationService,
    private readonly messageService: MessageService,     
  ) {}
      
  ngOnInit() {
    this.loadChartData();
  }

  loadChartData() {
    this.loading = true;
    this.service.getInfoChart(this.filters).subscribe({
      next: (response: InfoChart[]) => {
        const labels: string[] = [];
        const presupuestado: number[] = [];
        const ejecutado: number[] = [];

        const gastosMap = new Map<string, { presupuestado: number, ejecutado: number }>();

        response.forEach(item => {
          const name = item.name;
          const amount = parseFloat(item.amount.toString());
          const isPresupuestado = item.description.toLowerCase().includes('presup');

          if (!gastosMap.has(name)) {
            gastosMap.set(name, { presupuestado: 0, ejecutado: 0 });
          }

          const gasto = gastosMap.get(name)!;
          if (isPresupuestado) {
            gasto.presupuestado += amount;
          } else {
            gasto.ejecutado += amount;
          }
        });

        gastosMap.forEach((val, key) => {
          labels.push(key);
          presupuestado.push(val.presupuestado);
          ejecutado.push(val.ejecutado);
        });

        this.chartData = {
          labels: labels,
          datasets: [
            {
              label: 'Presupuestado',
              backgroundColor: '#42A5F5',
              data: presupuestado
            },
            {
              label: 'Ejecutado',
              backgroundColor: '#66BB6A',
              data: ejecutado
            }
          ]
        };

        this.chartOptions = {
          responsive: true,
          maintainAspectRatio: false,
          plugins: {
            legend: {
              position: 'top',
            },
            title: {
              display: true,
              text: 'Comparativo de presupuesto vs ejecución por tipo de gasto'
            }
          }
        };
        this.loading = false;
      },    
      error: (error) => {
        this.loading = false;
        this.showError(error);
      }
    });
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
    
  onDateFilter(dates: Date[],field: string) {
    if (dates && dates.length === 2) {
      const [startDate, endDate] = dates;

      startDate.setHours(0, 0, 0, 0);

      endDate.setHours(23, 59, 59, 999);
      
      const formattedStart = this.formatDate(startDate);
      const formattedEnd = this.formatDate(endDate);

      const data = {
        field: field,
        value: formattedStart,
        EndDate: formattedEnd,
        typeDateTime: 0
      };

      const indiceExist = this.filters.findIndex(item => item.field === data.field);
                
      if (indiceExist !== -1) {
          this.filters.splice(indiceExist, 1);
      }

      if (data.value) {
          this.filters.push(data);
      }
  
      this.loadChartData(); 
    }
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
