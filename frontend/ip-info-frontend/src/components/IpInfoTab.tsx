import React, { useState } from 'react';
import { Box, TextField, Button, Paper, Typography, CircularProgress, Alert } from '@mui/material';
import { IpInfoResponse } from '../api/api';
import { fetchIpInfo } from '../api/api';

const IpInfoTab = () => {
  const [ip, setIp] = useState('');
  const [data, setData] = useState<IpInfoResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    
    try {
      const result = await fetchIpInfo(ip);
      setData(result);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Unknown error occurred');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" gutterBottom>
        IP Info Lookup
      </Typography>
      
      <Box component="form" onSubmit={handleSubmit} sx={{ mb: 3 }}>
        <TextField
          label="IP Address"
          variant="outlined"
          fullWidth
          value={ip}
          onChange={(e) => setIp(e.target.value)}
          sx={{ mb: 2 }}
        />
        <Button 
          type="submit" 
          variant="contained" 
          disabled={loading}
        >
          {loading ? <CircularProgress size={24} /> : 'Lookup'}
        </Button>
      </Box>

      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      {data && (
        <Box sx={{ mt: 2 }}>
          <Typography variant="h6">Results:</Typography>
          <pre>{JSON.stringify(data, null, 2)}</pre>
        </Box>
      )}
    </Paper>
  );
};

export default IpInfoTab;