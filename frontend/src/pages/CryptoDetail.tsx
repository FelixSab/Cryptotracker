import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer } from 'recharts';
import { useAxios } from '@/hooks';
import { CryptoCurrency } from '@/types';
import { Card, CardContent } from '@/components/ui/card';

export default function CryptoDetail() {
  const { id } = useParams();
  const axios = useAxios();
  
  const { data: crypto, isLoading } = useQuery({
    queryKey: ['crypto', id],
    queryFn: async () => {
      const response = await axios.get<CryptoCurrency>(`/currencies/${id}`);
      return response.data;
    },
  });

  if (isLoading) return <div>Loading...</div>;
  if (!crypto) return <div>Cryptocurrency not found</div>;

  return (
    <div className="space-y-6">
      <Card>
        <CardContent className="p-6">
          <div className="flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4">
            <div>
              <h1 className="text-2xl sm:text-3xl font-bold">{crypto.name}</h1>
              <p className="text-gray-500">{crypto.symbol.toUpperCase()}</p>
            </div>
            <div className="text-left sm:text-right w-full sm:w-auto">
              <p className="text-xl sm:text-2xl font-bold">${crypto.currentPrice.toFixed(2)}</p>
              <p className={`${crypto.priceChange24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
                {crypto.priceChange24h.toFixed(2)}%
              </p>
            </div>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardContent className="p-6">
          <div className="h-[300px] sm:h-[400px] w-full">
            <ResponsiveContainer width="100%" height="100%">
              <LineChart
                data={crypto.priceHistory}
                margin={{ top: 5, right: 5, left: 5, bottom: 5 }}
              >
                <CartesianGrid strokeDasharray="3 3" />
                <XAxis 
                  dataKey="timestamp" 
                  tickFormatter={(value) => new Date(value).toLocaleDateString()}
                />
                <YAxis 
                  domain={['auto', 'auto']}
                  tickFormatter={(value) => `$${value.toFixed(2)}`}
                />
                <Tooltip 
                  labelFormatter={(value) => new Date(value).toLocaleString()}
                  formatter={(value) => [`$${Number(value).toFixed(2)}`, 'Price']}
                  contentStyle={{ 
                    backgroundColor: '#1a1a1a',
                    color: '#ffffff',
                    border: 'none',
                    borderRadius: '6px',
                    padding: '8px'
                  }}
                />
                <Line 
                  type="monotone" 
                  dataKey="price" 
                  stroke="#646cff" 
                  dot={false}
                />
              </LineChart>
            </ResponsiveContainer>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}