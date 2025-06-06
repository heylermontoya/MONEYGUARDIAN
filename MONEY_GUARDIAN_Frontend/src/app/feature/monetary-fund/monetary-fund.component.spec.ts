import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MonetaryFundComponent } from './monetary-fund.component';

describe('MonetaryFundComponent', () => {
  let component: MonetaryFundComponent;
  let fixture: ComponentFixture<MonetaryFundComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonetaryFundComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MonetaryFundComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
