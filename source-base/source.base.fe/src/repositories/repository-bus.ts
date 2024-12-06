import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import semester from '@/views/School/Semester.vue';
import { CreateBus, RequestBusIndex } from '@/types/model/bus';
export class BusRepository extends BaseRepository {
  public async getEnrolledBus(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.BUS.ENROLLMENT_BUS}`, {}, success, error, false);
  }
  public async getBusDetail(
    params: { busId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS.DETAIL_BUS}`, params, success, error);
  }
  public async getAssignedBus(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.BUS.ASSIGNED_BUS}`, {}, success, error);
  }

  public async getBusEnrollDetail(

    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS.PUPIL_ENROLLMENT}`, {}, success, error);
  }

  public async getBusDetailOfPupil(
    params: { busId: number,semesterId:number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS.PUPIL_BUS_DETAIL}/${params.busId}/${params.semesterId}`, params, success, error);
  }
  public async getAllBuses(
    params: RequestBusIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.BUS.INDEX, params, success, error, false, true);
  }

  public async addBus(params: CreateBus, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.BUS.CREATE, params, success, error);
  }

  public async updateBus(
    id: number,
    params: CreateBus,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.BUS.UPDATE}/${id}`, params, success, error);
  }

  public async getBusForAdmin(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.BUS.INFORMATION}/${id}`, [], success, error, false, true);
  }

  public async deleteBus(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.delete(`${API_URL.BUS.DELETE}/${id}`, [], success, error);
  }
}
