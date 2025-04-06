import React, { useState, useEffect } from 'react';
import { Box, Tabs, Tab, Paper, Typography, CircularProgress, Alert } from '@mui/material';
import { 
  fetchMyIpInfo, 
  fetchMyFreeGeoIp, 
  fetchMyAbuseIpDb, 
  fetchMyFullData,
  IpInfoResponse,
  FreeGeoIpResponse,
  AbuseIpDbResponse,
  IPDataResponseDTO
} from '../api/api';

const MyIpDataTab = () => {
  const [tabValue, setTabValue] = useState(0);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [ipInfo, setIpInfo] = useState<IpInfoResponse | null>(null);
  const [freeGeoIp, setFreeGeoIp] = useState<FreeGeoIpResponse | null>(null);
  const [abuseIpDb, setAbuseIpDb] = useState<AbuseIpDbResponse | null>(null);
  const [fullData, setFullData] = useState<IPDataResponseDTO | null>(null);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError(null);
      
      try {
        if (tabValue === 0) {
          const data = await fetchMyIpInfo();
          setIpInfo(data);
        } else if (tabValue === 1) {
          const data = await fetchMyFreeGeoIp();
          setFreeGeoIp(data);
        } else if (tabValue === 2) {
          const data = await fetchMyAbuseIpDb();
          setAbuseIpDb(data);
        } else if (tabValue === 3) {
          const data = await fetchMyFullData();
          setFullData(data);
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Unknown error occurred');
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [tabValue]);

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" gutterBottom>
        My IP Data
      </Typography>
      
      <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
        <Tabs value={tabValue} onChange={handleTabChange} variant="scrollable">
          <Tab label="IP Info" />
          <Tab label="FreeGeoIP" />
          <Tab label="AbuseIPDB" />
          <Tab label="Full Data" />
        </Tabs>
      </Box>

      {loading && <CircularProgress sx={{ display: 'block', mx: 'auto', my: 2 }} />}
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}

      {tabValue === 0 && ipInfo && (
        <Box>
          <Typography variant="h6">Your IP Info:</Typography>
          <pre>{JSON.stringify(ipInfo, null, 2)}</pre>
        </Box>
      )}

      {tabValue === 1 && freeGeoIp && (
        <Box>
          <Typography variant="h6">Your FreeGeoIP Data:</Typography>
          <pre>{JSON.stringify(freeGeoIp, null, 2)}</pre>
        </Box>
      )}

      {tabValue === 2 && abuseIpDb && (
        <Box>
          <Typography variant="h6">Your AbuseIPDB Data:</Typography>
          <pre>{JSON.stringify(abuseIpDb, null, 2)}</pre>
        </Box>
      )}

      {tabValue === 3 && fullData && (
        <Box>
          <Typography variant="h6">Your Full IP Data:</Typography>
          <pre>{JSON.stringify(fullData, null, 2)}</pre>
        </Box>
      )}
    </Paper>
  );
};

export default MyIpDataTab;