import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdControl } from './ad-control';

describe('AdControl', () => {
  let component: AdControl;
  let fixture: ComponentFixture<AdControl>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdControl]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdControl);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
