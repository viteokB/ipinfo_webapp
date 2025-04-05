// Реэкспортируем типы из API
export * from '../api/api';

// Дополнительные типы для фронтенда
export interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

export interface ApiResponse<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
}

export interface LookupFormProps {
  onSubmit: (ip: string) => void;
  loading: boolean;
}

export interface LookupResultProps<T> {
  data: T | null;
  error: string | null;
}

// Типы для UI компонентов
export interface AlertState {
  open: boolean;
  message: string;
  severity: 'error' | 'warning' | 'info' | 'success';
}

export interface IpValidationResult {
  isValid: boolean;
  error?: string;
}

// Типы для данных, специфичных для фронтенда
export interface FormattedIpData {
  ip: string;
  location: string;
  isp: string;
  riskScore?: number;
  lastReported?: string;
}

export interface MapCoordinates {
  lat: number;
  lng: number;
  label: string;
}

// Вспомогательные типы
export type Nullable<T> = T | null;