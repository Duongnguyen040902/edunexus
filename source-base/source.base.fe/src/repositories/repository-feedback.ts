import { BaseRepository } from '@/repositories/base-repository.ts';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses.ts';
import { API_URL } from '@/constants/api/endpoint.ts';
import { PupilFeedback } from '@/types/model/feedback.ts';

export class FeedbackRepository extends BaseRepository {
  public async getClassFeedback(
    params: { classId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.FEEDBACK.CLASS_FEEDBACKS}`, params, success, error);
  }
  public async createFeedback(
    data: PupilFeedback,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(`${API_URL.FEEDBACK.CREATE_FEEDBACK}`, data, success, error);
  }

  public async updateFeedback(
    data: PupilFeedback,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.FEEDBACK.UPDATE_FEEDBACK}`, data, success, error);
  }

  public async deleteFeedback(
    data: {pupilId: number, semesterId: number},
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(`${API_URL.FEEDBACK.DELETE_FEEDBACK}`, data, success, error);
  }

  public async getFeedbackDetail(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.FEEDBACK.GET_PUPIL_FEEDBACK}`, {}, success, error);
  }
}
