/// <reference types="jasmine" />

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { of, Subject, throwError } from 'rxjs';
import { TournamentPageComponent } from './tournament-page.component';
import { TournamentService } from '../../services/tournament.service';
import { Pokemon } from '../../models/pokemon.model';

describe('TournamentPageComponent', () => {
  let component: TournamentPageComponent;
  let fixture: ComponentFixture<TournamentPageComponent>;
  let tournamentService: jasmine.SpyObj<TournamentService>;

  const pokemon: Pokemon[] = Array.from({ length: 10 }, (_, index) => ({
    id: index + 1,
    name: `pokemon-${index + 1}`,
    type: 'normal',
    wins: index,
    losses: 0,
    ties: 0
  }));

  beforeEach(async () => {
    tournamentService = jasmine.createSpyObj<TournamentService>('TournamentService', ['getTournamentStatistics']);
    tournamentService.getTournamentStatistics.and.returnValue(of(pokemon));

    await TestBed.configureTestingModule({
      imports: [TournamentPageComponent],
      providers: [{ provide: TournamentService, useValue: tournamentService }]
    }).compileComponents();

    fixture = TestBed.createComponent(TournamentPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should update sortDirection when changed', () => {
    component.sortDirection = 'asc' as typeof component.sortDirection;
    component.changeSort();
    expect(component.sortDirection).toBe('asc');
    expect(tournamentService.getTournamentStatistics).toHaveBeenCalled();
  });

  it('should load the first page of tournament results', () => {
    expect(component.total).toEqual(pokemon);
    expect(component.pagedTotal.length).toBe(8);
    expect(component.pagedTotal[0].id).toBe(1);
  });

  it('should request tournament results without participant count', () => {
    component.loadData();

    expect(tournamentService.getTournamentStatistics).toHaveBeenCalledWith(
      component.sortBy,
      component.sortDirection);
  });

  it('should show a loading state while results are pending', () => {
    const results$ = new Subject<Pokemon[]>();
    tournamentService.getTournamentStatistics.and.returnValue(results$);

    component.loadData();
    fixture.detectChanges();

    expect(component.isLoading).toBeTrue();
    expect(fixture.nativeElement.querySelector('[role="status"]')).not.toBeNull();

    results$.next(pokemon);
    results$.complete();
    fixture.detectChanges();

    expect(component.isLoading).toBeFalse();
    expect(fixture.nativeElement.querySelector('[role="status"]')).toBeNull();
  });

  it('should reset to the first page when page size changes', () => {
    component.currentPage = 2;
    component.pageSize = 4;
    component.changePageSize();

    expect(component.currentPage).toBe(1);
    expect(component.pagedTotal.length).toBe(4);
  });

  it('should not move beyond the last page', () => {
    component.nextPage();
    component.nextPage();

    expect(component.currentPage).toBe(2);
    expect(component.pagedTotal.map(item => item.id)).toEqual([9, 10]);
  });

  it('should show an error when tournament results fail to load', () => {
    tournamentService.getTournamentStatistics.and.returnValue(throwError(() => new Error('request failed')));

    component.loadData();

    expect(component.errorMessage).toBe('Unable to load tournament statistics.');
  });
});
