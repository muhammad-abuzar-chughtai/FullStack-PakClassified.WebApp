import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementSubcategory } from './advertisement-subcategory';

describe('AdvertisementSubcategory', () => {
  let component: AdvertisementSubcategory;
  let fixture: ComponentFixture<AdvertisementSubcategory>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementSubcategory]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementSubcategory);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
