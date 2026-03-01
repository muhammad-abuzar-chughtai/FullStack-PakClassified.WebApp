import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SecurityInterceptor } from './security.interceptor';

describe('SecurityInterceptor', () => {
  let component: SecurityInterceptor;
  let fixture: ComponentFixture<SecurityInterceptor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SecurityInterceptor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SecurityInterceptor);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
