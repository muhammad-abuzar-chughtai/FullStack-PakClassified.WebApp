import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementImage } from './advertisement-image';

describe('AdvertisementImage', () => {
  let component: AdvertisementImage;
  let fixture: ComponentFixture<AdvertisementImage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementImage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementImage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
