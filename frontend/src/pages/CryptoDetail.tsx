import { useQuery } from '@tanstack/react-query';
import { useParams } from 'react-router-dom';
import { LineChart, Line, XAxis, YAxis, CartesianGrid, Tooltip } from 'recharts';
import { useAxios } from '@/hooks';
import { CryptoCurrency } from '@/types';

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
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">{crypto.name}</h1>
          <p className="text-gray-500">{crypto.symbol.toUpperCase()}</p>
        </div>
        <div className="text-right">
          <p className="text-2xl font-bold">${crypto.currentPrice.toFixed(2)}</p>
          <p className={`${crypto.priceChange24h >= 0 ? 'text-green-600' : 'text-red-600'}`}>
            {crypto.priceChange24h.toFixed(2)}%
          </p>
        </div>
      </div>

      <div className="h-[400px] w-full">
        <LineChart
          width={800}
          height={400}
          data={crypto.priceHistory}
          margin={{ top: 5, right: 30, left: 20, bottom: 5 }}
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
            contentStyle={{ color: '#646cff' }}
          />
          <Line 
            type="monotone" 
            dataKey="price" 
            stroke="#8884d8" 
            dot={false}
          />
        </LineChart>
      </div>
    </div>
  );
}