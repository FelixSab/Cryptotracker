
export interface CryptoCurrencyList {
  id: number;
  symbol: string;
  name: string;
  currentPrice: number;
  priceChange24h: number;
  lastUpdated: Date;
}
