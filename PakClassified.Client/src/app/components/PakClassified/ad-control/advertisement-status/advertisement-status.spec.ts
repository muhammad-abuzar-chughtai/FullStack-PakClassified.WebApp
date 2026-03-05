import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementStatus } from './advertisement-status';

describe('AdvertisementStatus', () => {
  let component: AdvertisementStatus;
  let fixture: ComponentFixture<AdvertisementStatus>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementStatus]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementStatus);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
