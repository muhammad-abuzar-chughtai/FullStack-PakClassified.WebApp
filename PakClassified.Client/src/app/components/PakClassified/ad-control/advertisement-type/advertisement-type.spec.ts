import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementType } from './advertisement-type';

describe('AdvertisementType', () => {
  let component: AdvertisementType;
  let fixture: ComponentFixture<AdvertisementType>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementType]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementType);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
