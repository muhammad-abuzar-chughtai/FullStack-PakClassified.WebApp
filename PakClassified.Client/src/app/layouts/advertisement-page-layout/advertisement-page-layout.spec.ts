import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementPageLayout } from './advertisement-page-layout';

describe('AdvertisementPageLayout', () => {
  let component: AdvertisementPageLayout;
  let fixture: ComponentFixture<AdvertisementPageLayout>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementPageLayout]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementPageLayout);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
