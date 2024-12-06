import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { ClassEnrollmentRepository } from '@/repositories/repository-class_enrollment';
import {
  RequestAssignPupilInterface,
  RequestAssignTeacherInterface,
  RequestDeleteTeacherInterface,
  RequestGetClassEnrollment,
  RequestGetTeacherIdInterface,
  RequestGetTeacherSwapInterface,
  RequestSwapTeacherInterface,
  RequestUpdateAssignTeacherInterface,
  ResponseClassEnrollmentData,
} from '@/types/model/class-enrollment';
import { reactive, ref } from 'vue';

export const useClassEnrollmentStore = defineStore('class_enrollment', () => {
  const classEnrollmentFactory = RepositoryFactory.create('class_enrollment') as ClassEnrollmentRepository;
  const classIdFromClassEnrollment = ref<number>();
  const dataMember = reactive<{ value: ResponseClassEnrollmentData }>({
    value: {
      pageNumber: 1,
      pageSize: 10,
      firstPage: '',
      lastPage: '',
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });
  const requestGetMemberClass = reactive<RequestGetClassEnrollment>({
    pageNumber: 1,
    pageSize: 10,
    keyword: null,
    semesterId: null,
    classId: null,
  });

  const dataMemberInNextSemester = reactive<{ value: ResponseClassEnrollmentData[] }>({
    value: {
      id: 0,
      classId: 0,
      className: '',
      block: 0,
      teacherName: '',
      teacherId: '',
      teacherCode: '',
      teacherImage: '',
      pupilName: '',
      pupilId: 0,
      pupilCode: '',
      pupilImage: '',
      semesterId: 0,
    },
  });

  const assignTeacherToClass = async (
    params: RequestAssignTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.assignTeacherToClass(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const updateAssignTeacher = async (
    params: RequestUpdateAssignTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.updateTeacherAssign(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const pupilsToGradute = async (
    params: number[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.pupilsToGradute(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const assignPupilToClass = async (
    params: RequestAssignPupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.assignPupilToClass(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const removeTeacherFromClass = async (
    params: RequestDeleteTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.removeTeacherFromClass(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const removeMemberFromClass = async (
    params: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.removeMemberFromClass(
      params,
      res => {
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getTeacherIdFromClassEnrollment = async (
    params: RequestGetTeacherIdInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.getTeacherId(
      params,
      res => {
        classIdFromClassEnrollment.value = res.data;
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getMemberInClass = async () => {
    await classEnrollmentFactory.getListMemberInClass(
      requestGetMemberClass,
      res => {
        const response = res.data;
        dataMember.value = response;
      },
      err => {
        console.log('error', err);
      },
    );
  };

  const getMemberInClassInNextSemester = async (nextSemesterId: number) => {
    await classEnrollmentFactory.getListMemberInNextSemester(
      nextSemesterId,
      res => {
        const response = res.data;
        dataMemberInNextSemester.value = response;
      },
      err => {
        console.log('error', err);
      },
    );
  };

  const getTeacherSwap = async (
    params: RequestGetTeacherSwapInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.getListTeacherInClassAssign(
      params,
      res => {
        console.log(res);
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const swapTeacher = async (
    params: RequestSwapTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await classEnrollmentFactory.swapTeacherInClassAssign(
      params,
      res => {
        console.log(res);
        return success(res.data as SuccessResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };
  return {
    classIdFromClassEnrollment,
    requestGetMemberClass,
    getMemberInClassInNextSemester,
    pupilsToGradute,
    dataMemberInNextSemester,
    swapTeacher,
    dataMember,
    removeMemberFromClass,
    getTeacherSwap,
    updateAssignTeacher,
    assignPupilToClass,
    getMemberInClass,
    assignTeacherToClass,
    removeTeacherFromClass,
    getTeacherIdFromClassEnrollment,
  };
});
