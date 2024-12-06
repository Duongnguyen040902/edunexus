import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { ResponseGetScore } from '@/types/model/score';

export class ScoreRepository extends BaseRepository {
    public async getClassScore(
        params: { classId: number, semesterId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.SCORE.CLASS_SCORE_LIST}`, params, success, error);
    }

    public async getPupilForCreate(
        params: { entityId: number, semesterId: number, subjectId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.SCORE.GET_PUPIL_FOR_CREATE}`, params, success, error);
    }

    public async getPupilScores(
        params: { entityId: number, semesterId: number, subjectId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.SCORE.GET_PUPIL_SCORE}`, params, success, error);
    }

    public async updatePupilScores(
        data: ResponseGetScore[],
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.put(`${API_URL.SCORE.UPDATE}`, data, success, error);
    }

    public async createPupilScores(
        data: ResponseGetScore[],
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        console.log('data', data);
        return await this.post(`${API_URL.SCORE.CREATE}`, data, success, error);
    }

    public async getIndividualScore(
        params: { semesterId: number},
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.SCORE.GET_INDIVIDUAL_SCORE}`, params, success, error);
    }



}