import { METHOD } from '@/constants/api/method';

export interface HttpRequestOptions {
  method: METHOD;
  url: string;
  data?: object;
  params?: object;
}
