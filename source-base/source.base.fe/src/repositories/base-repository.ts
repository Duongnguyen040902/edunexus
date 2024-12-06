import axiosInstance, { contentType } from '@/plugins/axios';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { API_URL } from '@/constants/api/endpoint';
import { AxiosResponse, AxiosError } from 'axios';
import { Cookie } from '@/classes/cookie';
import { METHOD } from '@/constants/api/method';
import { HttpRequestOptions } from '@/constants/api/request';

export class BaseRepository {
  public async get(
    endpoint: string,
    params: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    isMultipart: boolean = false,
    isPaginate: boolean = false,
  ) {
    return this.request(METHOD.GET, endpoint, params, success, error, isMultipart, isPaginate);
  }

  public async post(
    endpoint: string,
    data: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    isMultipart: boolean = false,
  ) {
    return this.request(METHOD.POST, endpoint, data, success, error, isMultipart);
  }

  public async put(
    endpoint: string,
    data: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    isMultipart: boolean = false,
  ) {
    return this.request(METHOD.PUT, endpoint, data, success, error, isMultipart);
  }

  public async patch(
    endpoint: string,
    data: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    isMultipart: boolean = false,
  ) {
    return this.request(METHOD.PATCH, endpoint, data, success, error, isMultipart);
  }

  public async delete(
    endpoint: string,
    data: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return this.request(METHOD.DELETE, endpoint, data, success, error);
  }

  private async request(
    method: METHOD,
    endpoint: string,
    data: object,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    isMultipart: boolean = false,
    isPaginate: boolean = false,
  ) {
    let payload = 'data';
    if (method === 'get') {
      payload = 'params';
    }

    if (isMultipart) {
      contentType.value = 'multipart/form-data';
    }

    const options: HttpRequestOptions = {
      method: method,
      url: endpoint,
      [payload]: data,
    };

    await axiosInstance(options)
      .then((res: AxiosResponse<SuccessResponse>) =>
        isPaginate ? this.handleSuccessWithPaginate(res, success) : this.handleSuccess(res, success),
      )
      .catch((err: AxiosError) => this.handleError(err, error))
      .finally(() => {
        if (isMultipart) {
          contentType.value = 'application/json';
        }
      });
  }

  private handleSuccess(res: AxiosResponse<SuccessResponse>, success: (res: SuccessResponse) => void) {
    const { data } = res.data;
    success({
      data: data || res.data || [],
      message: res.data.message || '',
      succeeded: true,
    });
  }

  private handleSuccessWithPaginate(res: AxiosResponse<SuccessResponse>, success: (res: SuccessResponse) => void) {
    success({
      data: res,
      message: res.data.message || '',
      succeeded: true,
    });
  }

  private handleError(err: AxiosError, error: (err: ErrorResponse) => void) {
    const response = err.response as AxiosResponse<ErrorResponse>;
    const errorResponse: ErrorResponse = {
      message: response?.data?.message || err.message,
      code: response?.status || 500,
      responseCode: response?.status || 500,
      errors: response?.data?.errors || {},
      data: response?.data?.data || undefined,
    };
    error(errorResponse);
  }
}
