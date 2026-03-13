import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdPage } from './ad-page';

describe('AdPage', () => {
  let component: AdPage;
  let fixture: ComponentFixture<AdPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdPage);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
