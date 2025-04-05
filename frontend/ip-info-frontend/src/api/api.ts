import axios from 'axios';

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5000/api';

const apiClient = axios.create({
  baseURL: API_BASE_URL,
  withCredentials: false,
  headers: {
    'Accept': 'application/json',
    'Content-Type': 'application/json',
    'api-version': '1.0'
  }
});

// Add CORS headers to every request
apiClient.interceptors.request.use(config => {
  config.headers = config.headers || {};
  config.headers['Access-Control-Allow-Origin'] = '*';
  config.headers['Access-Control-Allow-Methods'] = 'GET,PUT,POST,DELETE,PATCH,OPTIONS';
  config.headers['Access-Control-Allow-Headers'] = 'Content-Type, Authorization, api-version';
  return config;
});

// Error handling interceptor
apiClient.interceptors.response.use(
  response => response,
  error => {
    if (error.response?.status === 0) {
      console.error('CORS error detected');
      throw new Error('CORS error: Failed to connect to API');
    }
    return Promise.reject(error);
  }
);

export interface Timezone {
  id: string;
  currentTime: string;
  code: string;
  isDaylightSaving: boolean;
  gmtOffset: number;
}

export interface IpInfoResponse {
  ip: string;
  asn?: string;
  as_Name?: string;
  as_Domain?: string;
  countryCode?: string;
  continentCode?: string;
  continent?: string;
}

export interface FreeGeoIpResponse {
  data: {
    ip: string;
    hostname?: string;
    type?: string;
    rangeType?: {
      type: string;
      description: string;
    };
    connection?: {
      asn: number;
      organization: string;
      isp: string;
      range: string;
    };
    location?: {
      geonamesId: number;
      latitude: number;
      longitude: number;
      zip?: string;
      continent?: {
        code: string;
        name: string;
        nameTranslated: string;
        geonamesId: number;
        wikidataId: string;
      };
      country?: {
        alpha2: string;
        alpha3: string;
        callingCodes: string[];
        currencies: Array<{
          symbol: string;
          name: string;
          symbolNative: string;
          decimalDigits: number;
          rounding: number;
          code: string;
          namePlural: string;
        }>;
        emoji: string;
        ioc: string;
        languages: Array<{
          name: string;
          nameNative: string;
        }>;
        name: string;
        nameTranslated: string;
        timezones: string[];
        isInEuropeanUnion: boolean;
        fips: string;
        geonamesId: number;
        hascId: string;
        wikidataId: string;
      };
      city?: {
        fips: string;
        alpha2?: string;
        geonamesId: number;
        hascId?: string;
        wikidataId: string;
        name: string;
        nameTranslated: string;
      };
      region?: {
        fips: string;
        alpha2: string;
        geonamesId: number;
        hascId: string;
        wikidataId: string;
        name: string;
        nameTranslated: string;
      };
    };
    timezone?: Timezone;
  };
}

export interface AbuseIpDbResponse {
  data: {
    ipAddress: string;
    isPublic: boolean;
    ipVersion: number;
    isWhitelisted: boolean | null;
    abuseConfidenceScore: number;
    countryCode: string;
    usageType: string;
    isp: string;
    domain: string;
    hostnames: string[];
    isTor: boolean;
    totalReports: number;
    numDistinctUsers: number;
    lastReportedAt: string | null;
  };
}

export interface IPDataResponseDTO {
  ipInfoResponse: IpInfoResponse;
  freeGeoIpResponse: FreeGeoIpResponse;
  abuseIpDbResponse: AbuseIpDbResponse;
}

export interface MyResponseMessage {
  message: string;
  errors?: string[];
}

export const checkCors = async (): Promise<boolean> => {
  try {
    await apiClient.options('');
    return true;
  } catch (error) {
    console.error('CORS check failed:', error);
    return false;
  }
};

export const fetchIpInfo = async (ip: string): Promise<IpInfoResponse> => {
  const response = await apiClient.get<IpInfoResponse>(`/ipinfo/${ip}`);
  return response.data;
};

export const fetchFreeGeoIp = async (ip: string): Promise<FreeGeoIpResponse> => {
  const response = await apiClient.get<FreeGeoIpResponse>(`/freegeoip/${ip}`);
  return response.data;
};

export const fetchAbuseIpDb = async (ip: string, maxAgeInDays = 90): Promise<AbuseIpDbResponse> => {
  const response = await apiClient.get<AbuseIpDbResponse>(
    `/abuseipdb/${ip}`,
    { params: { maxAgeInDays } }
  );
  return response.data;
};

export const fetchFullData = async (ip: string): Promise<IPDataResponseDTO> => {
  const response = await apiClient.get<IPDataResponseDTO>(`/full/${ip}`);
  return response.data;
};

export const fetchMyIpInfo = async (): Promise<IpInfoResponse> => {
  const response = await apiClient.get<IpInfoResponse>('/ipinfo/my');
  return response.data;
};

export const fetchMyFreeGeoIp = async (): Promise<FreeGeoIpResponse> => {
  const response = await apiClient.get<FreeGeoIpResponse>('/freegeoip/my');
  return response.data;
};

export const fetchMyAbuseIpDb = async (maxAgeInDays = 90): Promise<AbuseIpDbResponse> => {
  const response = await apiClient.get<AbuseIpDbResponse>(
    '/abuseipdb/my',
    { params: { maxAgeInDays } }
  );
  return response.data;
};

export const fetchMyFullData = async (): Promise<IPDataResponseDTO> => {
  const response = await apiClient.get<IPDataResponseDTO>('/full/my');
  return response.data;
};