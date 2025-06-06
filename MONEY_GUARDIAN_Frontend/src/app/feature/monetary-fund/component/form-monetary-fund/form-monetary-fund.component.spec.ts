import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormMonetaryFundComponent } from './form-monetary-fund.component';

describe('FormMonetaryFundComponent', () => {
  let component: FormMonetaryFundComponent;
  let fixture: ComponentFixture<FormMonetaryFundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FormMonetaryFundComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FormMonetaryFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
