import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IndividualCardsComponent } from './individual-cards.component';

describe('IndividualCardsComponent', () => {
  let component: IndividualCardsComponent;
  let fixture: ComponentFixture<IndividualCardsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IndividualCardsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IndividualCardsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
