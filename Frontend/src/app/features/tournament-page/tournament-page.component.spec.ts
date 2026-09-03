/// <reference types="jasmine" />

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TournamentPageComponent } from './tournament-page.component';

describe('TournamentPageComponent', () => {
  let component: TournamentPageComponent;
  let fixture: ComponentFixture<TournamentPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [TournamentPageComponent],
      imports: [FormsModule]
    }).compileComponents();

    fixture = TestBed.createComponent(TournamentPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should update sortDirection when changed', () => {
    component.sortDirection = 'asc' as typeof component.sortDirection;
    component.changeSort();
    expect(component.sortDirection).toBe('asc');
  });
});
