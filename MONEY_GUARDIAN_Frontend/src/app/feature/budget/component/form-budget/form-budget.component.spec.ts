import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormBudgetComponent } from './form-budget.component';

describe('FormBudgetComponent', () => {
  let component: FormBudgetComponent;
  let fixture: ComponentFixture<FormBudgetComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormBudgetComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormBudgetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
