import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormExpenseComponent } from './form-expense.component';

describe('FormExpenseComponent', () => {
  let component: FormExpenseComponent;
  let fixture: ComponentFixture<FormExpenseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormExpenseComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormExpenseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
