import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormExpenseTypeComponent } from './form-expense-type.component';

describe('FormExpenseTypeComponent', () => {
  let component: FormExpenseTypeComponent;
  let fixture: ComponentFixture<FormExpenseTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormExpenseTypeComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormExpenseTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
