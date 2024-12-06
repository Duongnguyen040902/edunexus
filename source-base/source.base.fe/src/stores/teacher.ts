import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { reactive } from 'vue';
import { ErrorResponse } from '@/constants/api/responses';
import { TeacherAssignRepository } from '@/repositories/repository-teacher';
import { ListTeacherAssignResponseInterface, RequestGetTeacherAssignInterface } from '@/types/model/teacher';

export const useTeacherStore = defineStore('teacherAssign', () => {
  const teacherFactory = RepositoryFactory.create('teacherAssign') as TeacherAssignRepository;
  const listTeacherAssign = reactive<{ value: ListTeacherAssignResponseInterface | null }>({ value: null });
  const listTeacherInClassAssign = reactive<{ value: ListTeacherAssignResponseInterface | null }>({ value: null });
  const getTeacherAssign = async (
    params: RequestGetTeacherAssignInterface,
    success: (res: ListTeacherAssignResponseInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await teacherFactory.getListTeacherAssign(
      params,
      res => {
        if (res.data) {
          listTeacherAssign.value = res.data;
          return success(listTeacherAssign.value);
        }
      },
      (err: ErrorResponse) => {
        console.error('Error fetching teacher assignments:', err);
      },
    );
  };

  return {
    getTeacherAssign,
  };
});
