import axios, { AxiosError, AxiosResponse } from 'axios';
import { ref } from 'vue';
import { Cookie } from '@/classes/cookie';

const axiosInstance = axios.create({
  // @ts-expect-error - Vite injects env vars at runtime, so TypeScript doesn't recognize them
  baseURL: import.meta.env.VITE_APP_API_URL,
  timeout: 50000,
  withCredentials: true,
});

export const accept = ref('application/json');
export const contentType = ref('application/json');

const handleUnauthorizedError = () => {
  Cookie.remove('token');
  const queryString = `${window.location.pathname}${window.location.search}`;
  Cookie.set('redirect', queryString);
  window.location.href = '/authen/login';
};

const handleForbiddenError = () => {
  window.location.href = '/error/forbidden';
};

axiosInstance.interceptors.request.use(
  config => {
    const token = Cookie.get('token');

    config.headers['Authorization'] = `Bearer ${token}`;
    config.headers['Accept'] = accept.value;
    config.headers['Content-Type'] = contentType.value;

    return config;
  },
  error => {
    return Promise.reject(error);
  },
);

axiosInstance.interceptors.response.use(
  (response: AxiosResponse) => {
    return response.data;
  },
  (error: AxiosError) => {
    if (error.response?.status === 401) {
      handleUnauthorizedError();
    } else if (error.response?.status === 403) {
      handleForbiddenError();
    }
    return Promise.reject(error);
  },
);

export default axiosInstance;
