import { API_URL } from '@/constants/api/endpoint';
import { RequestCreateAndUpdateTimetableInterface, RequestGetTimetableInterface } from './../types/model/timetable';
// src/repositories/timetable-repository.ts
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { BaseRepository } from './base-repository';

export class TimetableRepository extends BaseRepository {
  public async getTimeTableDetail(
    params: RequestGetTimetableInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.TIMETABLE.GET}/${params.classId}/${params.semesterId}`, params, success, error);
  }
  public async createTimeTable(
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(`${API_URL.TIMETABLE.CREATE}`, data, success, error);
  }
  public async updateTimeTable(
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.TIMETABLE.UPDATE}`, data, success, error);
  }
  public async deleteTimeTable(
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.TIMETABLE.DELETE}`, data, success, error);
  }
}
