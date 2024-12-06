import { BaseRepository } from './base-repository';
import { API_URL } from '@/constants/api/endpoint';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { CreateBusEnrollment, RequestBusEnrollmentIndex } from '@/types/model/bus-enrollment';

export class BusEnrollmentRepository extends BaseRepository {
  public async getAllBusEnrollments(
    params: RequestBusEnrollmentIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.BUS_ENROLLMENT.INDEX, params, success, error, false, true);
  }

  public async addBusEnrollment(
    params: CreateBusEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.BUS_ENROLLMENT.CREATE, params, success, error);
  }

  public async updateBusEnrollment(
    id: number,
    params: CreateBusEnrollment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.BUS_ENROLLMENT.UPDATE}/${id}`, params, success, error);
  }

  public async getBusEnrollmentDetail(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_ENROLLMENT.DETAIL}/${id}`, [], success, error, false, true);
  }

  public async getPupilsWithoutBus(
    semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_ENROLLMENT.GET_PUPILS_WITHOUT_BUS}?semesterId=${semesterId}`,[], success, error, false, true);
  }

  public async getPupilsInBusStop(
    semesterId: number,
    busStopId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_ENROLLMENT.GET_PUPILS_IN_BUS_STOP}?semesterId=${semesterId}&busStopId=${busStopId}`,[], success, error, false, true);
  }

  public async getBusSupervisorWithoutBus(
    semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_ENROLLMENT.GET_BUS_SUPERVISOR_WITHOUT_BUS}?semesterId=${semesterId}`,[], success, error, false, true);
  }

  public async deleteBusEnrollment(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.BUS_ENROLLMENT.DELETE}/${id}`, [], success, error);
  }
}
