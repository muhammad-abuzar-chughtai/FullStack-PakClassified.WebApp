import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WelcomeAdmin } from './welcome-admin';

describe('WelcomeAdmin', () => {
  let component: WelcomeAdmin;
  let fixture: ComponentFixture<WelcomeAdmin>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WelcomeAdmin]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WelcomeAdmin);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
