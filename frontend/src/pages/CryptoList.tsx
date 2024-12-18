import { useQuery } from '@tanstack/react-query';
import { Link, useSearchParams } from 'react-router-dom';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { Input } from '@/components/ui/input';
import { useEffect, useMemo, useState } from 'react';
import { CryptoCurrency } from '@/types';
import { useAxios } from '@/hooks';
import { debounce } from 'lodash';
import { ArrowUpDown, ChevronLeft, ChevronRight } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Card, CardContent } from '@/components/ui/card';

type SortField = 'name' | 'symbol' | 'price' | 'change';
type SortOrder = 'asc' | 'desc';

export default function CryptoList() {
  const [searchParams, setSearchParams] = useSearchParams();
  
  // Initialize state from URL params
  const [search, setSearch] = useState(searchParams.get('search') || '');
  const [debouncedSearch, setDebouncedSearch] = useState(search);
  const [sortField, setSortField] = useState<SortField>(searchParams.get('sortBy') as SortField || 'name');
  const [sortOrder, setSortOrder] = useState<SortOrder>(searchParams.get('order') as SortOrder || 'asc');
  const [currentPage, setCurrentPage] = useState(Number(searchParams.get('page')) || 1);

  // Update URL when states change
  useEffect(() => {
    const params = new URLSearchParams();
    if (debouncedSearch) params.set('search', debouncedSearch);
    if (sortField) params.set('sortBy', sortField);
    if (sortOrder) params.set('order', sortOrder);
    if (currentPage > 1) params.set('page', currentPage.toString());
    
    setSearchParams(params);
  }, [debouncedSearch, sortField, sortOrder, currentPage, setSearchParams]);

  const pageSize = 20;
  const axios = useAxios();
  
  useEffect(() => {
    const handler = debounce(() => {
      setDebouncedSearch(search);
    }, 300); // 300ms delay

    handler();
    return () => handler.cancel();
  }, [search]);

  const { data: cryptos, isLoading } = useQuery({
    queryKey: ['cryptos', debouncedSearch, sortField, sortOrder, currentPage],
    queryFn: async () => {
      const response = await axios.get<CryptoCurrency[]>('/currencies', {
        params: {
          searchTerm: search,
          sortBy: sortField,
          isDescending: sortOrder === 'desc',
          pageNumber: currentPage,
          pageSize
        }
      });
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
    setCurrentPage(1); // Reset to first page when sorting changes
  };

  const handleNextPage = () => {
    setCurrentPage(prev => prev + 1);
  };

  const handlePrevPage = () => {
    setCurrentPage(prev => Math.max(1, prev - 1));
  };

  const SearchInput = useMemo(() => (
    <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
      <h1 className="text-2xl font-bold">Cryptocurrencies</h1>
      <Input
        placeholder="Search cryptocurrencies..."
        className="w-full sm:max-w-xs"
        value={search}
        onChange={(e) => {
          setSearch(e.target.value);
          setCurrentPage(1); // Reset to first page when search changes
        }}
      />
    </div>
  ), [search])

  if (isLoading) return (
    <>
      {SearchInput}
      <div>Loading...</div>
    </>
  );

  return (
    <div className="space-y-4">
      {SearchInput}
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
                <Button variant="ghost" onClick={() => handleSort('price')} className="px-2 border-none bg-transparent hover:bg-accent ml-auto">
                  Price <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
              <TableHead className="text-right">
                <Button variant="ghost" onClick={() => handleSort('change')} className="px-2 border-none bg-transparent hover:bg-accent ml-auto">
                  24h Change <ArrowUpDown className="ml-1 h-4 w-4" />
                </Button>
              </TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {cryptos?.map((crypto) => (
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
        {cryptos?.map((crypto) => (
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

      {/* Pagination Controls */}
      <div className="flex items-center justify-between px-2">
        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="sm"
            onClick={handlePrevPage}
            disabled={currentPage === 1}
          >
            <ChevronLeft className="h-4 w-4" />
            Previous
          </Button>
          <div className="text-sm">
            Page {currentPage}
          </div>
          <Button
            variant="outline"
            size="sm"
            onClick={handleNextPage}
            disabled={!cryptos || cryptos.length < pageSize}
          >
            Next
            <ChevronRight className="h-4 w-4" />
          </Button>
        </div>
      </div>
    </div>
  );
}