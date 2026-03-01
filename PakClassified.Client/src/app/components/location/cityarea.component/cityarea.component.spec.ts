import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CityareaComponent } from './cityarea.component';

describe('CityareaComponent', () => {
  let component: CityareaComponent;
  let fixture: ComponentFixture<CityareaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CityareaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CityareaComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
