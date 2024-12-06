import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';

import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { ClassApplicationRepository } from '@/repositories/repository-class-application';
import { RequestResponseClassApplication, ResponseGetClassApplication } from '@/types/model/class-application';
export const useClassApplicationStore = defineStore('classApplication', () => {
  const classApplicationFactory = RepositoryFactory.create('classApplication') as ClassApplicationRepository;

  const getClassApplicationList = async (
    params: { classId: number; semesterId: number; categoryId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.getClassApplicationList(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data ?? [],
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        error(err);
      },
    );
  };

  const getCategory = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await classApplicationFactory.getCategory(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        error(err);
      },
    );
  };

  const responseClassApplication = async (
    params: RequestResponseClassApplication,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.responseClassApplication(
      params,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const getPupilApplication = async (
    params: { semesterId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.getPupilApplication(
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
        error(err);
      },
    );
  };

  const createClassApplication = async (
    data: ResponseGetClassApplication,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.createClassApplication(
      data,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateClassApplication = async (
    data: ResponseGetClassApplication,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.updateClassApplication(
      data,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const deleteClassApplication = async (
    params: { id: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classApplicationFactory.deleteClassApplication(
      params,
      res => {
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    deleteClassApplication,
    getPupilApplication,
    responseClassApplication,
    getClassApplicationList,
    getCategory,
    createClassApplication,
    updateClassApplication,
  };
});
