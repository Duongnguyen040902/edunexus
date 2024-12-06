import { CreateBusRoute } from './../types/model/bus-route';
import { RequestBusRouteIndex } from '@/types/model/bus-route';
import { BaseRepository } from './base-repository';
import { API_URL } from '@/constants/api/endpoint';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';

export class BusRouteRepository extends BaseRepository {
  public async getAllBusRoute(
    params: RequestBusRouteIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.BUS_ROUTE.INDEX, params, success, error, false, true);
  }

  public async addBusRoute(
    params: CreateBusRoute,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {  
    return await this.post(API_URL.BUS_ROUTE.ADD, params, success, error);
  }

  public async updateBusRoute(
    id: number,
    params: CreateBusRoute,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {  
    return await this.put(`${API_URL.BUS_ROUTE.UPDATE}/${id}`, params, success, error);
  }

  public async getBusRouteDetail(
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_ROUTE.DETAIL}/${params}`, [], success, error, false, true);
  }

  public async deleteBusRoute(
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.BUS_ROUTE.DELETE}/${params}`, [], success, error);
  }
}
