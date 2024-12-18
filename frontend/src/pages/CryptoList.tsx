import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table';
import { Input } from '@/components/ui/input';
import { useState } from 'react';
import { CryptoCurrency } from '@/types';
import { useAxios } from '@/hooks';

export default function CryptoList() {
  const [search, setSearch] = useState('');
  const axios = useAxios();
  
  const { data: cryptos, isLoading } = useQuery({
    queryKey: ['cryptos'],
    queryFn: async () => {
      const response = await axios.get<CryptoCurrency[]>('/currencies');
      return response.data;
    },
  });

  const filteredCryptos = cryptos?.filter(crypto =>
    crypto.name.toLowerCase().includes(search.toLowerCase()) ||
    crypto.symbol.toLowerCase().includes(search.toLowerCase())
  );

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">Cryptocurrencies</h1>
        <Input
          placeholder="Search cryptocurrencies..."
          className="max-w-xs"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>
      <div className="rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Symbol</TableHead>
              <TableHead className="text-right">Price</TableHead>
              <TableHead className="text-right">24h Change</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {filteredCryptos?.map((crypto) => (
              <TableRow key={crypto.id}>
                <TableCell>
                  <Link to={`/crypto/${crypto.id}`} className="hover:underline">
                    {crypto.name}
                  </Link>
                </TableCell>
                <TableCell>{crypto.symbol.toUpperCase()}</TableCell>
                <TableCell className="text-right">
                  ${crypto.currentPrice.toFixed(2)}
                </TableCell>
                <TableCell className={`text-right ${crypto.priceChange24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                  {crypto.priceChange24h.toFixed(2)}%
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </div>
    </div>
  );
}
