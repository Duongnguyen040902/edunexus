import { pa } from 'element-plus/es/locale';
import { API_URL } from '@/constants/api/endpoint';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { BaseRepository } from './base-repository';
import { RequestGetSemesterInterface, RequestCreateAndUpdateSemester, RequestSemesterIndex } from '@/types/model/semester';


export class SemesterRepository extends BaseRepository {
  public async getSemester(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.SEMESTER.GET}`, {}, success, error);
  }
  public async getCurrentSemester(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.SEMESTER.GET_CURRENT}`, {}, success, error);
  }
  public async getListSemester(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.SEMESTER.GET, data, success, error);
  }

  public async getSemestersBySchoolYearId(params: RequestSemesterIndex, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) { const url = `${API_URL.SEMESTER.INDEX(params.schoolYearId)}`; return await this.get(url, {}, success, error); }
  public async getSemesterDetail(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) { return await this.get(API_URL.SEMESTER.DETAIL(id), {}, success, error); }

public async createSemester(params: RequestCreateAndUpdateSemester, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) { return await this.post(API_URL.SEMESTER.CREATE, params, success, error); }

public async updateSemester(params: RequestCreateAndUpdateSemester, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) { return await this.put(API_URL.SEMESTER.EDIT, params, success, error); }

public async deleteSemester(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) { return await this.delete(API_URL.SEMESTER.DELETE(id), {}, success, error); } 



}
