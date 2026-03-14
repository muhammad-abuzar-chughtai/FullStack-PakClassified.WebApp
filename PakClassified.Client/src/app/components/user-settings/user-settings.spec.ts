import { ComponentFixture, TestBed } from '@angular/core/testing';

import { USERSETTINGS } from './user-settings';

describe('USERSETTINGS', () => {
  let component: USERSETTINGS;
  let fixture: ComponentFixture<USERSETTINGS>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [USERSETTINGS]
    })
    .compileComponents();

    fixture = TestBed.createComponent(USERSETTINGS);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
