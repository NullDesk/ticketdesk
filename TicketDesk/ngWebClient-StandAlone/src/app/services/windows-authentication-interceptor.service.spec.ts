import { TestBed, inject } from '@angular/core/testing';

import { WindowsAuthenticationInterceptorService } from './windows-authentication-interceptor.service';

describe('WindowsAuthenticationInterceptorService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WindowsAuthenticationInterceptorService]
    });
  });

  it('should be created', inject([WindowsAuthenticationInterceptorService], (service: WindowsAuthenticationInterceptorService) => {
    expect(service).toBeTruthy();
  }));
});
