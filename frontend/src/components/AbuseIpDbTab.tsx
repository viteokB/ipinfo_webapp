import React, { useState } from 'react';
import { Box, TextField, Button, Paper, Typography, CircularProgress, Alert, Slider } from '@mui/material';
import { AbuseIpDbResponse } from '../api/api';
import { fetchAbuseIpDb } from '../api/api';

const AbuseIpDbTab = () => {
  const [ip, setIp] = useState('');
  const [maxAgeInDays, setMaxAgeInDays] = useState(90);
  const [data, setData] = useState<AbuseIpDbResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    
    try {
      const result = await fetchAbuseIpDb(ip, maxAgeInDays);
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
        AbuseIPDB Lookup
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
        
        <Typography gutterBottom>Max Age in Days</Typography>
        <Slider
          value={maxAgeInDays}
          onChange={(e, value) => setMaxAgeInDays(value as number)}
          min={1}
          max={365}
          step={1}
          valueLabelDisplay="auto"
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

export default AbuseIpDbTab;