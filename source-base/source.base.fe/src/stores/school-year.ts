import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { SchoolYearRespository } from '@/repositories/repository-school-year';
import {
  ErrorResponseSchoolYear,
  RequestSchoolYearIndex,
  RequestCreateAndUpdateSchoolYear,
  ResponseSchoolYearDetail,
  ResponseSchoolYearIndex,
} from '@/types/model/school-year';
import { defineStore } from 'pinia';
import { reactive } from 'vue';
import { mapErrorKeys, clearErrorKeys } from '@/helpers/state';
export const useSchoolYearStore = defineStore('schoolYear', () => {
  const schoolYearFactory = RepositoryFactory.create('schoolYear') as SchoolYearRespository;
  const requestSchoolYearIndex = reactive<RequestSchoolYearIndex>({ pageNumber: 1, pageSize: 10 });

  const dataSchoolYear = reactive<{ value: ResponseSchoolYearIndex }>({
    value: {
      pageNumber: 1,
      pageSize: 10,
      firstPage: 1,
      lastPage: 1,
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });

  const requestSchoolYearUpdate = reactive<RequestCreateAndUpdateSchoolYear>({
    id: 0,
    name: '',
    startDate: '',
    endDate: '',
    isActive: false,
  });

  const schoolYearDetail = reactive<{ value: ResponseSchoolYearDetail }>({
    value: { id: 0, name: '', startDate: '', endDate: '', isActive: false, schoolId: 0 },
  });
  const errorSchoolYear = reactive(<ErrorResponseSchoolYear>{ Name: [], StartDate: [], EndDate: [] });
  const errorSchoolYearKeys: (keyof ErrorResponseSchoolYear)[] = ['Name', 'StartDate', 'EndDate'];

  const getSchoolYearIndex = async () => {
    await schoolYearFactory.getAllSchoolYear(
      requestSchoolYearIndex,
      (res: SuccessResponse) => {
        const response = res.data as ResponseSchoolYearIndex;
        dataSchoolYear.value = response;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const getSchoolYearDetail = async (id: number) => {
    await schoolYearFactory.getSchoolYearDetail(
      id,
      (res: SuccessResponse) => {
        schoolYearDetail.value = res.data as ResponseSchoolYearDetail;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const createSchoolYear = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,) => {
    await schoolYearFactory.createSchoolYear(
      requestSchoolYearUpdate,
      (res: SuccessResponse) => {
        clearErrorKeys(errorSchoolYearKeys, errorSchoolYear);
        return success(res);
      },
      (err: ErrorResponse) => {
        mapErrorKeys(errorSchoolYearKeys, errorSchoolYear, err.errors as ErrorResponseSchoolYear);
        error(err);
      },
    );
  };

  const updateSchoolYear = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,) => {
    await schoolYearFactory.updateSchoolYear(
      requestSchoolYearUpdate,
      (res: SuccessResponse) => {
        clearErrorKeys(errorSchoolYearKeys, errorSchoolYear);
        return success(res);
      },
      (err: ErrorResponse) => {
        mapErrorKeys(errorSchoolYearKeys, errorSchoolYear, err.errors as ErrorResponseSchoolYear);
        error(err);
      },
    );
  };

  const deleteSchoolYear = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await schoolYearFactory.deleteSchoolYear(
      id,
      res => {
        return success(res);
      },
      (err: ErrorResponse) => {
        error(err);
      },
    );
  };
  return {
    errorSchoolYear,
    errorSchoolYearKeys,
    requestSchoolYearIndex,
    dataSchoolYear,
    requestSchoolYearUpdate,
    schoolYearDetail,
    deleteSchoolYear,
    getSchoolYearIndex,
    getSchoolYearDetail,
    createSchoolYear,
    updateSchoolYear,
  };
});
