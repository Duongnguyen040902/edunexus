import { BaseRepository } from './base-repository';
import { API_URL } from '@/constants/api/endpoint';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { CreateClubEnrollment, RequestClubEnrollmentIndex, UpdateClubEnrollment } from '@/types/model/club-enrollment';

export class ClubEnrollmentRepository extends BaseRepository {
  public async getBySemester(
    params: RequestClubEnrollmentIndex,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(API_URL.CLUB_ENROLLMENT.GET_BY_SEMESTER, params, success, error, false, true);
  }

  public async createClubEnrollment(
    params: CreateClubEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.CLUB_ENROLLMENT.CREATE, params, success, error);
  }

  public async updateClubEnrollment(
    params: UpdateClubEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.CLUB_ENROLLMENT.UPDATE_INFO}`, params, success, error);
  }

  public async removeClubEnrollment(
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.CLUB_ENROLLMENT.REMOVE}/${id}`, [], success, error);
  }

  public async getTeachersNotInClub(
    clubId: number,
    semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(
      `${API_URL.CLUB_ENROLLMENT.GET_TEACHER_NOT_IN_CLUB}?clubId=${clubId}&semesterId=${semesterId}`,
      [],
      success,
      error,
    );
  }

  public async getPupilsNotInClub(
    clubId: number,
    semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(
      `${API_URL.CLUB_ENROLLMENT.GET_PUPIL_NOT_IN_CLUB}?clubId=${clubId}&semesterId=${semesterId}`,
      [],
      success,
      error,
    );
  }

  public async getPupilsRegisterClub(
    clubId: number,
    semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(
      `${API_URL.CLUB_ENROLLMENT.GET_PUPIL_REGISTER_CLUB}?clubId=${clubId}&semesterId=${semesterId}`,
      [],
      success,
      error,
    );
  }

  public async getMemberToCopyClub(
    nextSemesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(
      `${API_URL.CLUB_ENROLLMENT.GET_MEMBER_TO_COPY}?nextSemesterId=${nextSemesterId}`,
      [],
      success,
      error,
    );
  }
}
