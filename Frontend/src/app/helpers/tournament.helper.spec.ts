import { getTypeBadgeClass, getURL, getWinRate } from './tournament.helper';

describe('tournament helpers', () => {
  it('returns the expected badge class for supported types', () => {
    expect(getTypeBadgeClass('FIRE')).toBe('text-bg-danger');
    expect(getTypeBadgeClass('water')).toBe('text-bg-primary');
    expect(getTypeBadgeClass('unknown')).toBe('text-bg-secondary');
  });

  it('builds the Pokemon sprite URL from the id', () => {
    expect(getURL(25)).toBe(
      'https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png'
    );
  });

  it('calculates rounded win rates and handles tournaments with no battles', () => {
    expect(getWinRate(2, 1, 1)).toBe(50);
    expect(getWinRate(0, 0, 0)).toBe(0);
  });
});