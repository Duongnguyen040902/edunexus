import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { TeacherProfileRepository } from '@/repositories/repository-teacherProfile';
import { reactive } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import {
  RequestUpdateDataTeacherInterface,
} from '@/types/model/teacher';

export const useTeacherStore = defineStore('teacherProfile', () => {
  const teacherFactory = RepositoryFactory.create('teacherProfile') as TeacherProfileRepository;
  const teacherProfile = reactive<{ value: RequestUpdateDataTeacherInterface | null }>({ value: null });

  const getTeacherProfile = async (
    success: (res: RequestUpdateDataTeacherInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await teacherFactory.getTeacherProfile(
      res => {
        teacherProfile.value = res.data as RequestUpdateDataTeacherInterface;
        return success(res.data as RequestUpdateDataTeacherInterface);
      },
      err => {
        error(err);
      },
    );
  };

  const updateTeacher = async (
    params: RequestUpdateDataTeacherInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    console.log('params-store', params);

    await teacherFactory.updateTeacher(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    teacherProfile,
    updateTeacher,
    getTeacherProfile,
  };
});
