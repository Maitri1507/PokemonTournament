import { Routes } from '@angular/router';
import { TournamentPageComponent } from './features/tournament-page/tournament-page.component';
import { ErrorPageComponent } from './features/error-page/error-page.component';

export const routes: Routes = [
  { path: '', component: TournamentPageComponent },
  { path: 'error', component: ErrorPageComponent },
  { path: '**', redirectTo: '' }
];
