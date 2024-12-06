import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { SchoolRepository } from '@/repositories/repository-school';
import { reactive } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { SchoolDashboardDTO, SchoolInfoResponseInterface } from '@/types/model/school';
import { ResponseGetBusDetail } from '@/types/model/bus.ts';
import { ChangePasswordDTO } from '@/types/model/change-password.ts';

export const useSchoolStore = defineStore('school', () => {
  const schoolFactory = RepositoryFactory.create('school') as SchoolRepository;
  const schoolInfo = reactive<{ value: SchoolInfoResponseInterface | null }>({ value: null });

  const getSchoolInfo = async (
    params: [],
    success: (res: SchoolInfoResponseInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    try {
      await schoolFactory.getInfoSchool(
        params,
        res => {
          schoolInfo.value = res.data as SchoolInfoResponseInterface;
          return success(res.data as SchoolInfoResponseInterface);
        },
        err => {
          console.log('Error fetching school info:', err);
          error(err);
        },
      );
    } catch (err) {
      console.error('Unexpected error fetching school info:', err);
    }
  };

  const updateSchoolInfo = async (
    params: SchoolInfoResponseInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await schoolFactory.updateSchool(
      params,
      res => {
        success(res);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getDashboard = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    try {
      await schoolFactory.getDashboard(
        res => {
          const successResponse: SuccessResponse = {
            data: res.data as SchoolDashboardDTO,
            message: res.message,
            succeeded: res.succeeded,
          };
          return success(successResponse);
        },
        err => {
          console.log('Error fetching school dashboard:', err);
          error(err);
        },
      );
    } catch (err) {
      console.error('Unexpected error fetching school dashboard:', err);
    }
  };

  const ChangePassword = async (
    params: ChangePasswordDTO,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await schoolFactory.ChangePassword(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as Boolean,
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

  return { schoolInfo, getSchoolInfo, updateSchoolInfo, getDashboard, ChangePassword };
});
