import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementCategory } from './advertisement-category';

describe('AdvertisementCategory', () => {
  let component: AdvertisementCategory;
  let fixture: ComponentFixture<AdvertisementCategory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementCategory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementCategory);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
