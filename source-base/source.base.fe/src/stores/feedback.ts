import { FeedbackRepository } from '@/repositories/repository-feedback.ts';
import RepositoryFactory from '@/repositories/repository.ts';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses.ts';
import { PupilFeedback } from '@/types/model/feedback.ts';
import { defineStore } from 'pinia';

export const useFeedbackStore = defineStore('feedback', () => {
  const feedbackFactory = RepositoryFactory.create('feedback') as FeedbackRepository;

  const getClassFeedback = async (
    params: { classId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await feedbackFactory.getClassFeedback(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as PupilFeedback[],
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

  const createFeedback = async (
    data: PupilFeedback,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await feedbackFactory.createFeedback(
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
      },
    );
  };

  const updateFeedback = async (
    data: PupilFeedback,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await feedbackFactory.updateFeedback(
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
      },
    );
  };

  const deleteFeedback = async (
    data: { pupilId: number; semesterId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await feedbackFactory.deleteFeedback(
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
      },
    );
  };

  const getListFeedbackOfPupil = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await feedbackFactory.getFeedbackDetail(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as PupilFeedback[],
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
  return {
    deleteFeedback,
    getClassFeedback,
    createFeedback,
    updateFeedback,
    getListFeedbackOfPupil
  };
});
