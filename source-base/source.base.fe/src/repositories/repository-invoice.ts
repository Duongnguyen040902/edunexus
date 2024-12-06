import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import {
  RequestCreateInvoiceManager, RequestCreatePayment,
  RequestDeleteInvoiceManager,
  RequestIndexInvoice,
  RequestListInvoiceInterface
} from '@/types/model/invoice.ts';

export class InvoiceManagerRepository extends BaseRepository {
  public async getAllInvoice(
    params: RequestIndexInvoice,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.INVOICE_MANAGER.INDEX, params, success, error, false, true);
  }

  public async getAllInvoices(
    params: RequestListInvoiceInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.INVOICE.ALL}`, params, success, error, false,true);
  }

  public async getInvoiceManagerDetail(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.INVOICE_MANAGER.DETAIL(id), {}, success, error, false, true);
  }

  public async getInvoiceDetail(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(API_URL.INVOICE.DETAIL(id), {}, success, error, false, true);
  }

  public async createInvoiceManager(
    params: RequestCreateInvoiceManager,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.INVOICE_MANAGER.CREATE, params, success, error);
  }

  public async updateInvoiceManager(
    id: number,
    params: RequestCreateInvoiceManager,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.INVOICE_MANAGER.EDIT(id), params, success, error);
  }

  public async deleteInvoiceManager(
    params: RequestDeleteInvoiceManager,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(API_URL.INVOICE_MANAGER.DELETE, params.ids, success, error);
  }

  public async createPayment(
    params: RequestCreatePayment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.INVOICE_MANAGER.CREATE_PAYMENT, params, success, error);
  }

  public async getInvoiceBySubscription(
    subscriptionId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.INVOICE.GET_BY_SUBSCRIPTION}/${subscriptionId}`, {}, success, error, false,false);
  }

  public async deleteListInvoice(
    params: RequestDeleteInvoiceManager,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(API_URL.INVOICE.DELETE, params.ids, success, error);
  }
}
