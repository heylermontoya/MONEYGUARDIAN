import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChartTransactionComponent } from './chart-transaction.component';

describe('ChartTransactionComponent', () => {
  let component: ChartTransactionComponent;
  let fixture: ComponentFixture<ChartTransactionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChartTransactionComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChartTransactionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
