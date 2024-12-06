import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestCreateTimeSlotInterface, RequestGetAllTimeSlotInterface, RequestGetTimeSlotInterface } from '@/types/model/timeslot';

export class TimeSlotRepository extends BaseRepository {
  public async getAllTimeSlot(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.TIME_SLOT.ALL, {}, success, error);
  }

  public async getAllTimeSlots(
    params: RequestGetAllTimeSlotInterface, 
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.TIME_SLOT.GET, params, success, error, false, true);
  }

  public async addTimeSlot(
    params: RequestCreateTimeSlotInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.TIME_SLOT.CREATE, params, success, error);
  }

  public async updateTimeSlot(
    id: number,
    params: RequestCreateTimeSlotInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.TIME_SLOT.UPDATE}/${id}`, params, success, error);
  }

  public async getTimeSlotDetail(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.TIME_SLOT.DETAIL}/${id}`, [], success, error, false, true);
  }

  public async deleteTimeSlot(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.TIME_SLOT.DELETE}/${id}`, [], success, error);
  }
}
