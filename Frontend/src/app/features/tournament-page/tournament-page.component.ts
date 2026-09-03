import { Component, OnInit } from '@angular/core';
import { TournamentService } from '../../services/tournament.service';
import { Pokemon, SortDirection, SortOptions } from '../../models/pokemon.model';
import { FormsModule } from '@angular/forms';
import { IndividualCardsComponent } from '../individual-cards/individual-cards.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tournament-page',
  imports: [FormsModule, IndividualCardsComponent, CommonModule],
  standalone: true,
  templateUrl: './tournament-page.component.html',
  styleUrls: ['./tournament-page.component.scss']
})
export class TournamentPageComponent implements OnInit {

  total: Pokemon[] = [];
  pagedTotal: Pokemon[] = [];

  sortBy: SortOptions = SortOptions.Wins;
  sortDirection: SortDirection = SortDirection.Asc;

  sortOptionsList = Object.values(SortOptions);
  sortDirectionList = Object.values(SortDirection);

  pageSize = 8;
  currentPage = 1;

  errorMessage: string | null = null;

  directionLabels: Record<string, string> = {
  asc: 'Ascending',
  desc: 'Descending'
};


  constructor(private tournamentService: TournamentService) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.errorMessage = null;

    this.tournamentService.getTournamentStatistics(this.sortBy, this.sortDirection)
      .subscribe({
        next: (data) => {
          this.total = data;
          this.currentPage = 1;
          this.updatePage();
        },
        error: () => {
          this.errorMessage = 'Unable to load tournament statistics.';
        }
      });
  }

  
  changeSort(): void {
    this.loadData();
  }

 updatePage(): void {
  const start = (this.currentPage - 1) * this.pageSize;
  this.pagedTotal = this.total.slice(start, start + this.pageSize);
}

changePageSize(): void {
  this.currentPage = 1;
  this.updatePage();
}

nextPage(): void {
  const totalPages = Math.ceil(this.total.length / this.pageSize);
  if (this.currentPage < totalPages) {
    this.currentPage++;
    this.updatePage();
  }
}

prevPage(): void {
  if (this.currentPage > 1) {
    this.currentPage--;
    this.updatePage();
  }
}

}
