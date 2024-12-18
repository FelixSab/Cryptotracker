import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import { QueryClient, QueryClientProvider } from '@tanstack/react-query';
import Layout from '@/components/Layout';
import CryptoList from '@/pages/CryptoList';
import CryptoDetail from '@/pages/CryptoDetail';

const queryClient = new QueryClient();

function App() {
  return (
    <QueryClientProvider client={queryClient}>
      <Router>
        <Routes>
          <Route path="/" element={<Layout />}>
            <Route index element={<CryptoList />} />
            <Route path="/crypto/:id" element={<CryptoDetail />} />
          </Route>
        </Routes>
      </Router>
    </QueryClientProvider>
  );
}

export default App;