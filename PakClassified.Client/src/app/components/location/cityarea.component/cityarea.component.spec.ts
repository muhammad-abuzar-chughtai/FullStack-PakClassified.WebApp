import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CityAreaComponent } from './cityarea.component';

describe('CityareaComponent', () => {
  let component: CityAreaComponent;
  let fixture: ComponentFixture<CityAreaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CityAreaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CityAreaComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
