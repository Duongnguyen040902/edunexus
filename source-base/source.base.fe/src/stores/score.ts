import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { ScoreRepository } from '@/repositories/repository-score';
import { ResponseGetScore } from '@/types/model/score';

export const useScoreStore = defineStore('score', () => {
    const scoreFactory = RepositoryFactory.create('score') as ScoreRepository;

    const getClassScore = async (
        params: { classId: number, semesterId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.getClassScore(
            params,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            },
        );
    };

    const getPupilForCreate = async (
        params: { entityId: number, semesterId: number, subjectId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.getPupilForCreate(
            params,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            },
        );
    };

    const getPupilScores = async (
        params: { entityId: number, semesterId: number, subjectId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.getPupilScores(
            params,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            },
        );
    };

    const updatePupilScores = async (
        data: ResponseGetScore[],
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.updatePupilScores(
            data,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    }

    const createPupilScores = async (
        data: ResponseGetScore[],
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.createPupilScores(
            data,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    }

    const getIndividualScore = async (
        params: { semesterId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await scoreFactory.getIndividualScore(
            params,
            res => {
                const successResponse: SuccessResponse = {
                    data: res.data,
                    message: res.message,
                    succeeded: res.succeeded,
                };
                return success(successResponse);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    }
    return {
        getIndividualScore,
        getClassScore,
        getPupilForCreate,
        getPupilScores,
        updatePupilScores,
        createPupilScores,
    };
}
);
