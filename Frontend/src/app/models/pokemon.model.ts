export interface Pokemon {
    id: number;
    name: string;
    type: string;
    wins: number;
    losses: number;
    ties: number;
}

export enum SortOptions {
    Wins = 'wins',
    Losses = 'losses',
    Ties = 'ties',
    Name = 'name',
    Id = 'id'
}

export enum SortDirection {
    Asc = 'asc',
    Desc = 'desc'
}
