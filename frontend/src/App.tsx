import React, { useState, useEffect } from 'react';
import { Container, Tabs, Tab, Box, Typography, CircularProgress, Alert } from '@mui/material';
import { 
  fetchIpInfo,
  fetchFreeGeoIp,
  fetchAbuseIpDb,
  fetchFullData,
  fetchMyIpInfo,
  fetchMyFreeGeoIp,
  fetchMyAbuseIpDb,
  fetchMyFullData,
  checkCors,
  IpInfoResponse,
  FreeGeoIpResponse,
  AbuseIpDbResponse,
  IPDataResponseDTO
} from './api/api';
import IpInfoTab from './components/IpInfoTab';
import FreeGeoIpTab from './components/FreeGeoIpTab';
import AbuseIpDbTab from './components/AbuseIpDbTab';
import FullDataTab from './components/FullDataTab';
import MyIpDataTab from './components/MyIpDataTab';

function App() {
  const [tabValue, setTabValue] = useState(0);
  const [corsError, setCorsError] = useState<string | null>(null);
  const [isCheckingCors, setIsCheckingCors] = useState(true);

  // Добавляем проверку CORS при загрузке приложения
  useEffect(() => {
    const verifyCors = async () => {
      try {
        setIsCheckingCors(true);
        const isCorsEnabled = await checkCors();
        if (!isCorsEnabled) {
          setCorsError('CORS не настроен на сервере. Некоторые функции могут не работать.');
        }
      } catch (error) {
        setCorsError(`Ошибка проверки CORS: ${error instanceof Error ? error.message : 'Неизвестная ошибка'}`);
      } finally {
        setIsCheckingCors(false);
      }
    };

    verifyCors();
  }, []);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setTabValue(newValue);
  };

  if (isCheckingCors) {
    return (
      <Container maxWidth="lg" sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
        <CircularProgress size={60} />
        <Typography variant="h6" sx={{ ml: 2 }}>Проверка соединения с сервером...</Typography>
      </Container>
    );
  }

  return (
    <Container maxWidth="lg" sx={{ mt: 4, mb: 4 }}>
      <Typography variant="h3" component="h1" gutterBottom align="center">
        IP Information Service
      </Typography>

      {corsError && (
        <Alert severity="warning" sx={{ mb: 3 }}>
          {corsError}
        </Alert>
      )}

      <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 3 }}>
        <Tabs 
          value={tabValue} 
          onChange={handleTabChange} 
          variant="scrollable"
          aria-label="IP information tabs"
        >
          <Tab label="IP Info" />
          <Tab label="FreeGeoIP" />
          <Tab label="AbuseIPDB" />
          <Tab label="Full Data" />
          <Tab label="My IP Data" />
        </Tabs>
      </Box>

      {tabValue === 0 && <IpInfoTab />}
      {tabValue === 1 && <FreeGeoIpTab />}
      {tabValue === 2 && <AbuseIpDbTab />}
      {tabValue === 3 && <FullDataTab />}
      {tabValue === 4 && <MyIpDataTab />}
    </Container>
  );
}

export default App;