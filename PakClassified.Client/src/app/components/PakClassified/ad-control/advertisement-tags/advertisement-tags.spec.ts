import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementTags } from './advertisement-tags';

describe('AdvertisementTags', () => {
  let component: AdvertisementTags;
  let fixture: ComponentFixture<AdvertisementTags>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementTags]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementTags);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
