import { Routes } from '@angular/router';
import { TournamentPageComponent } from './features/tournament-page/tournament-page.component';

export const routes: Routes = [
  { path: '', component: TournamentPageComponent },
  { path: '**', redirectTo: '' }
];
