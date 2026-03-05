import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Advertisement } from './advertisement';

describe('Advertisement', () => {
  let component: Advertisement;
  let fixture: ComponentFixture<Advertisement>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Advertisement]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Advertisement);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
