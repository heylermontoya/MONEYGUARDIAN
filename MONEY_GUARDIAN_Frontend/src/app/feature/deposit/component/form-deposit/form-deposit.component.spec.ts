import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormDepositComponent } from './form-deposit.component';

describe('FormDepositComponent', () => {
  let component: FormDepositComponent;
  let fixture: ComponentFixture<FormDepositComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormDepositComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormDepositComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
