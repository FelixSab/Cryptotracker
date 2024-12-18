import { PriceHistory } from "@/types";

export interface CryptoCurrency {
  id: number;
  symbol: string;
  name: string;
  currentPrice: number;
  priceChange24h: number;
  lastUpdated: Date;
  priceHistory: PriceHistory[];
}
