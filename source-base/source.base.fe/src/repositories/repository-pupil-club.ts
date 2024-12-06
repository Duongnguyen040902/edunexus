import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import {
  RequestGetClubIndex,
  RequestGetClubEnrollment,
  RequestCreateAndUpdateClubEnrollment,
} from '@/types/model/pupil-club';

export class PupilClubRepository extends BaseRepository {
  public async getClubsBySemesterActive(
    data: RequestGetClubIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.CLUB.GET_BY_SEMESTER_ACTIVE, data, success, error, false, true);
  }

  public async pupilCreateClubEnrollment(
    data: RequestCreateAndUpdateClubEnrollment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.CLUB_ENROLLMENT.PUPIL_CREATE, data, success, error);
  }

  public async updateClubEnrollment(
    data: RequestCreateAndUpdateClubEnrollment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.CLUB_ENROLLMENT.UPDATE, data, success, error);
  }

  public async getClubEnrollmentByPupilId(
    data: RequestGetClubEnrollment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.CLUB_ENROLLMENT.GET_BY_PUPIL_ID, data, success, error);
  }

  public async getCurrentSemester(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SEMESTER.GET_CURRENT, {}, success, error);
  }

  public async getNextSemester(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SEMESTER.GET_NEXT, {}, success, error);
  }

  public async getSemesters(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.SEMESTER.GET, {}, success, error);
  }
}