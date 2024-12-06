import { API_URL } from '@/constants/api/endpoint';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { BaseRepository } from '@/repositories/base-repository';
import {
  RequestCreatePupilInterface,
  RequestDeletePupil,
  RequestGetPupilDetailInterface,
  RequestListPupilInterface,
} from '@/types/model/pupil-account';
import { RequestImportExcelInterface } from '@/types/model/teacher-account';

export class PupilRepository extends BaseRepository {
  public async getAllPupils(
    params: RequestListPupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.PUPIL.ALL}`, params, success, error, false, true);
  }

  public async createPupil(
    params: RequestCreatePupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.PUPIL.CREATE, params, success, error, true);
  }

  public async getPupilDetail(
    params: RequestGetPupilDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.PUPIL.DETAIL}/${params.pupilId}`, params, success, error, false, false);
  }

  public async updatePupil(
    pupilId: number,
    params: RequestCreatePupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.PUPIL.UPDATE}/${params.pupilId}`, params, success, error, true);
  }
  
  public async deletePupil(
    params: RequestDeletePupil,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(API_URL.PUPIL.DELETE, params.ids, success, error);
  }

  public async importExcelPupil(
    file: RequestImportExcelInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.PUPIL.IMPORT_EXCEL, file, success, error, true);
  }
}
