import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestCreateClubInterface, RequestGetClubInterface } from '@/types/model/club';

export class ClubRepository extends BaseRepository {
  public async getEnrolledClub(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLUB.ENROLLED_CLUB}`, {}, success, error, false);
  }
  public async getClubDetail(
    params: { clubId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLUB.DETAIL}/${params.clubId}`, params, success, error);
  }
  public async getAssignedClub(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLUB.ASSIGNED_CLUB}`, {}, success, error);
  }

  public async getAllClubs(
    data: RequestGetClubInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLUB.GET_ALL}`, data, success, error, false, true);
  }

  public async getClubDetailForAdmin(
    params: { clubId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.CLUB.MAIN_DETAIL}?clubId=${params.clubId}`, [], success, error);
  }

  public async createClub(
    data: RequestCreateClubInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(`${API_URL.CLUB.CREATE}`, data, success, error);
  }

  public async updateClub(
    params: { clubId: number },
    data: RequestCreateClubInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.CLUB.UPDATE}/${params.clubId}`, data, success, error);
  }

  public async deleteClub(
    params: { clubId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.CLUB.DELETE}/${params.clubId}`, {}, success, error);
  }
}
