import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestGetAttendanceRecord, RequestGetListAttendance, ResponseGetAttendanceRecord } from '@/types/model/class-attendance';

export class AttendanceRepository extends BaseRepository {
  public async getListAttendance(
    params: RequestGetListAttendance,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.ATTENDANCE.LIST}`, params, success, error);
  }
  public async getAttendanceRecord(
    params: RequestGetAttendanceRecord,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.ATTENDANCE.GET}`, params, success, error);
  }
  public async prepareCreateAttendance(
    params: RequestGetListAttendance,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.ATTENDANCE.PREPARE}`, params, success, error);
  }
  public async createAttendance(
    data: ResponseGetAttendanceRecord[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(`${API_URL.ATTENDANCE.CREATE}`, data, success, error);
  }
  public async updateAttendance(
    data: ResponseGetAttendanceRecord[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.ATTENDANCE.UPDATE}`, data, success, error);
  }

  public async getPupilAttendance(
    params: {semesterId: number, date: Date},
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.ATTENDANCE.PUPIL_ATTENDANCE}`, params, success, error);
  }
}
