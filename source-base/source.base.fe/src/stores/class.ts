import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { END_POINT } from '@/constants/api/endpoint';
import { reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { ClassRepository } from '@/repositories/repository-class';
import {
  AddNewClassRequestInterface,
  DeleteClassRequestInterface, PupilClassesInterface,
  RequestGetClassDetailInterface,
  ResponseGetAssignedClassInterface,
  ResponseGetClassDetailInterface,
  updateClassRequestInterface,
} from '@/types/model/class';
import { ViewClassAdminResponseInterface } from '@/types/model/admin-school';

export const useClassStore = defineStore('class', () => {
  const classFactory = RepositoryFactory.create('class') as ClassRepository;
  const ClassDetail = reactive<{ value: ResponseGetClassDetailInterface[] }>({ value: [] });
  const addClassRequest = reactive<{ value: AddNewClassRequestInterface | null }>({ value: null });
  const dataClassDetail = reactive<ViewClassAdminResponseInterface>({
    id: 0,
    className: '',
    schoolId: '',
    status: 0,
    block: ''
  });
  const getClassDetail = async (
    params: RequestGetClassDetailInterface,
    success: (res: ResponseGetClassDetailInterface[]) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.getClassDetail(
      params,
      res => {
        ClassDetail.value = res.data as ResponseGetClassDetailInterface[];
        return success(res.data as ResponseGetClassDetailInterface[]);
      },
      err => {
        error(err);
      },
    );
  };

  const getAssignedClass = async ( success: (res: ResponseGetAssignedClassInterface) => void, error: (err: ErrorResponse) => void,) => {
    await classFactory.getAssignedClass(
      res => {
        return success(res.data as ResponseGetAssignedClassInterface);
      },
      err => {
        error(err);
      },
    );
  };
  const addNewClass = async (
    params: AddNewClassRequestInterface,
    success: (res: AddNewClassRequestInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.addNewClass(
      params,
      res => {
        return success(res.data as AddNewClassRequestInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const updateClass = async (
    params: updateClassRequestInterface,
    success: (res: updateClassRequestInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.updateNewClass(
      params,
      res => {
        return success(res.data as updateClassRequestInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const deleteClass = async (
    params: DeleteClassRequestInterface,
    success: (res: DeleteClassRequestInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.deleteClass(
      params,
      res => {
        return success(res.data as DeleteClassRequestInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const getEnrollmentClass = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await classFactory.getEnrolmentClass(
      res => {
        return success(res.data as ResponseGetAssignedClassInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const getPupilClasses = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await classFactory.getPupilClasses(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as PupilClassesInterface[],
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

  const getPupilClassDetail = async (
    params: RequestGetClassDetailInterface,
    success: (res: ResponseGetClassDetailInterface[]) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.getPupilClassDetail(
      params,
      res => {
        ClassDetail.value = res.data as ResponseGetClassDetailInterface[];
        return success(res.data as ResponseGetClassDetailInterface[]);
      },
      err => {
        error(err);
      },
    );
  };

  const getClassDetailForAdmin = async (
    params: number,
    success: (res: ViewClassAdminResponseInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classFactory.getClassDetailForAdmin(
      params,
      res => {
        dataClassDetail.value = res.data ;
        return res.data as ViewClassAdminResponseInterface;
      },
      err => {
      },
    );
  };

  return {
    getPupilClasses,
    getEnrollmentClass,
    addNewClass,
    addClassRequest,
    dataClassDetail,
    getClassDetailForAdmin,
    updateClass,
    deleteClass,
    getClassDetail,
    getAssignedClass,
    getPupilClassDetail,
  };
});
