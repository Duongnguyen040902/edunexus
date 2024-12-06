import { BaseRepository } from './base-repository';
import { API_URL } from '@/constants/api/endpoint';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { CreateBusStop, RequestBusStopIndex } from '@/types/model/bus-stop';

export class BusStopRepository extends BaseRepository {
  public async getAllBusStop(
    params: RequestBusStopIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.BUS_STOP.INDEX, params, success, error, false, true);
  }

  public async addBusStop(
    params: CreateBusStop,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {  
    return await this.post(API_URL.BUS_STOP.CREATE, params, success, error);
  }

  public async updateBusStop(
    id: number,
    params: CreateBusStop,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {  
    return await this.put(`${API_URL.BUS_STOP.UPDATE}/${id}`, params, success, error);
  }

  public async getBusStopDetail(
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_STOP.DETAIL}/${params}`, [], success, error, false, true);
  }

  public async deleteBusStop(
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.BUS_STOP.DELETE}/${params}`, [], success, error);
  }
}
