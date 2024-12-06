import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestUpdateDataPupilInterface } from '@/types/model/pupil';

export class PupilProfileRepository extends BaseRepository {
  public async getPupilProfile(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.PUPIL.PROFILE}`,{}, success, error);
  }

  public async updatePupil(
    pupilData: RequestUpdateDataPupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.PUPIL.PROFILE}`, pupilData, success, error, true);
  }
}