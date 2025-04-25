import { TestBed } from '@angular/core/testing';

import { PaginationHelperService } from './pagination-helper.service';

describe('PaginationHelperService', () => {
  let service: PaginationHelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PaginationHelperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
