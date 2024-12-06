import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { TeacherRepository } from '@/repositories/repository-teacherAccount';
import {
  RequestCreateTeacherInterface,
  RequestDeleteTeacher,
  RequestGetTeacherDetailInterface,
  RequestImportExcelInterface,
  ResponseGetListTeachersInterface,
  ResponseGetTeacherDetailInterface,
} from '@/types/model/teacher-account';
import { reactive } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { RequestGetListTeachersInterface } from '@/types/model/teacher-account';

export const useTeacherStore = defineStore('teacher', () => {
  const teacherFactory = RepositoryFactory.create('teacher') as TeacherRepository;
  const teachers = reactive<{ value: ResponseGetListTeachersInterface | null }>({ value: null });
  const teacherDetail = reactive<{ value: ResponseGetTeacherDetailInterface | null }>({ value: null });
  const requestDeleteTeacher = reactive<RequestDeleteTeacher>({
    ids: [],
  });
  const requestImportExcelTeacher = reactive<RequestImportExcelInterface>({
    file: new Blob(),
  });
  const getAllTeachers = async (
    params: RequestGetListTeachersInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    teacherFactory.getAllTeachers(
      params,
      res => {
        console.log('API Response-store:', res);
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetListTeachersInterface,
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

  const createTeacher = async (
    teacherData: RequestCreateTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await teacherFactory.createTeacher(
      teacherData,
      res => {
        success(res);
      },
      err => {
        const errorResponse: ErrorResponse = {
          ...err,
          errors: err.errors,
        };
        return error(errorResponse);
      },
    );
  };

  const updateTeacher = async (
    teacherId: number, //Todo
    params: RequestCreateTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    console.log('params-store', params);

    await teacherFactory.updateTeacher(
      teacherId,
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

  const getTeacherDetail = async (
    params: RequestGetTeacherDetailInterface,
    success: (res: ResponseGetTeacherDetailInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    console.log('params', params);

    await teacherFactory.getTeacherDetail(
      { teacherId: params },
      res => {
        teacherDetail.value = res.data as ResponseGetTeacherDetailInterface;
        return success(res.data as ResponseGetTeacherDetailInterface);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const deleteTeachers = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await teacherFactory.deleteTeacher(
      requestDeleteTeacher,
      res => {
        requestDeleteTeacher.ids = [];
        success(res);
      },
      err => {
        console.log(err);
        error(err);
      },
    );
  };

  const importExcelTeacherAccount = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    console.log("Đã vào error");
    await teacherFactory.importExcelTeacher(
      requestImportExcelTeacher,
      res => {
        success(res);
      },
      err => {
        console.log("Đã vào error1", err);
        if (err.data?.fileName && err.data?.fileContent) {
          console.log("Đã vào error2");
          
          const binaryContent = atob(err.data?.fileContent);
        
          const uint8Array = new Uint8Array([...binaryContent].map(char => char.charCodeAt(0)));
  
          const decoder = new TextDecoder('utf-8');
          const decodedContent = decoder.decode(uint8Array);
          const blob = new Blob([decodedContent], { type: 'text/plain;charset=utf-8' });
  
          const link = document.createElement('a');
          link.href = URL.createObjectURL(blob);
          link.download = err.data?.fileName;
          link.click();
        }
  
        const errorResponse: ErrorResponse = {
          ...err,
          errors: err.errors,
        };
        return error(errorResponse);
      },
    );
  };
  


  return {
    teachers,
    requestDeleteTeacher,
    requestImportExcelTeacher,
    getAllTeachers,
    createTeacher,
    getTeacherDetail,
    updateTeacher,
    deleteTeachers,
    importExcelTeacherAccount,
  };
});
