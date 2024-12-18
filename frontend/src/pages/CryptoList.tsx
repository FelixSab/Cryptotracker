import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Input } from '@/components/ui/input';
import { useState } from 'react';
import { CryptoCurrency } from '@/types';
import { useAxios } from '@/hooks';
import { ArrowUpDown } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

type SortField = 'name' | 'symbol' | 'currentPrice' | 'priceChange24h';
type SortOrder = 'asc' | 'desc';

export default function CryptoList() {
  const [search, setSearch] = useState('');
  const [sortField, setSortField] = useState<SortField>('name');
  const [sortOrder, setSortOrder] = useState<SortOrder>('asc');
  const axios = useAxios();

  const { data: cryptos, isLoading } = useQuery({
    queryKey: ['cryptos'],
    queryFn: async () => {
      const response = await axios.get<CryptoCurrency[]>('/currencies');
      return response.data;
    },
  });

  const handleSort = (field: SortField) => {
    if (field === sortField) {
      setSortOrder(sortOrder === 'asc' ? 'desc' : 'asc');
    } else {
      setSortField(field);
      setSortOrder('asc');
    }
  };

  const sortedAndFilteredCryptos = cryptos
    ?.filter(crypto =>
      crypto.name.toLowerCase().includes(search.toLowerCase()) ||
      crypto.symbol.toLowerCase().includes(search.toLowerCase())
    )
    .sort((a, b) => {
      const factor = sortOrder === 'asc' ? 1 : -1;
      return a[sortField] > b[sortField] ? factor : -factor;
    });

  if (isLoading) return <div>Loading...</div>;

  return (
    <div className="space-y-4">
      <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
        <h1 className="text-2xl font-bold">Cryptocurrencies</h1>
        <Input
          placeholder="Search cryptocurrencies..."
          className="w-full sm:max-w-xs"
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
      </div>

      {/* Desktop View */}
      <div className="hidden sm:block rounded-md border">
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>
                <Button variant="ghost" onClick={() => handleSort('name')} className="px-2 border-none bg-transparent hover:bg-accent">
                  Name <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
              <TableHead>
                <Button variant="ghost" onClick={() => handleSort('symbol')} className="px-2 border-none bg-transparent hover:bg-accent">
                  Symbol <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
              <TableHead className="text-right">
                <Button variant="ghost" onClick={() => handleSort('currentPrice')} className="px-2 border-none bg-transparent hover:bg-accent ml-auto">
                  Price <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
              <TableHead className="text-right">
                <Button variant="ghost" onClick={() => handleSort('priceChange24h')} className="px-2 border-none bg-transparent hover:bg-accent ml-auto">
                  24h Change <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {sortedAndFilteredCryptos?.map((crypto) => (
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

      {/* Mobile View */}
      <div className="grid grid-cols-1 gap-4 sm:hidden">
        {sortedAndFilteredCryptos?.map((crypto) => (
          <Card key={crypto.id}>
            <CardContent className="p-4">
              <Link to={`/crypto/${crypto.id}`} className="hover:underline">
                <div className="flex justify-between items-start mb-2">
                  <div>
                    <h3 className="font-medium">{crypto.name}</h3>
                    <p className="text-sm text-gray-500">{crypto.symbol.toUpperCase()}</p>
                  </div>
                  <div className="text-right">
                    <p className="font-medium">${crypto.currentPrice.toFixed(2)}</p>
                    <p className={`text-sm ${crypto.priceChange24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                      {crypto.priceChange24h.toFixed(2)}%
                    </p>
                  </div>
                </div>
              </Link>
            </CardContent>
          </Card>
        ))}
      </div>
    </div>
  );
}
