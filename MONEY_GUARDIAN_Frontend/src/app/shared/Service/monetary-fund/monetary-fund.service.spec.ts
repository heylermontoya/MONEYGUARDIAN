import { TestBed } from '@angular/core/testing';

import { MonetaryFundService } from './monetary-fund.service';

describe('MonetaryFundService', () => {
  let service: MonetaryFundService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MonetaryFundService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
