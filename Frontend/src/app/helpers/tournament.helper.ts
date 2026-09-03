export function getTypeBadgeClass(type: string): string {
  const t = type.toLowerCase();

  if (t === 'fire') return 'text-bg-danger';
  if (t === 'water') return 'text-bg-primary';
  if (t === 'grass') return 'text-bg-success';
  if (t === 'electric') return 'text-bg-warning';
  if (t === 'psychic') return 'text-bg-info';
  if (t === 'ghost' || t === 'dark') return 'text-bg-dark';

  return 'text-bg-secondary';
}

export function getURL(id: number): string {
    return `https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/${id}.png`;
}

export function getWinRate(wins: number, losses: number, ties: number): number {
    const totalBattles = wins + losses + ties;
    return totalBattles === 0 ? 0 : Math.round((wins / totalBattles) * 100);
}
