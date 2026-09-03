import { Component, Input } from '@angular/core';
import { Pokemon } from '../../models/pokemon.model';
import { getTypeBadgeClass, getURL, getWinRate } from '../../helpers/tournament.helper';

@Component({
  selector: 'app-individual-cards',
  standalone: true,
  imports: [],
  templateUrl: './individual-cards.component.html',
  styleUrl: './individual-cards.component.css'
})
export class IndividualCardsComponent {
 @Input({ required: true }) pokemon!: Pokemon;

    get Url(): string {
        return getURL(this.pokemon.id);
    }

    get typeBadgeClass(): string {
        return getTypeBadgeClass(this.pokemon.type);
    }

    get winRate(): number {
        return getWinRate(this.pokemon.wins, this.pokemon.losses, this.pokemon.ties);
    }
}
