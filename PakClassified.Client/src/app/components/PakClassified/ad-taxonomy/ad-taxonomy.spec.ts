import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdTaxonomy } from './ad-taxonomy';

describe('AdTaxonomy', () => {
  let component: AdTaxonomy;
  let fixture: ComponentFixture<AdTaxonomy>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdTaxonomy]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdTaxonomy);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
