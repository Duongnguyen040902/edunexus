import { SubjectRepository } from '@/repositories/repository-subject';
import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';

import {
  ErrorResponseCreateSubjectInterface,
  RequestCreateSubjectInterface,
  ResponseGetSubjectInterface,
} from '@/types/model/subject';
import { reactive } from 'vue';
import { ErrorResponse } from '@/constants/api/responses';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state.ts';

export const useSubjectStore = defineStore('subject', () => {
  const subjectFactory = RepositoryFactory.create('subject') as SubjectRepository;
  const subjects = reactive<{ value: ResponseGetSubjectInterface[] }>({ value: [] });
  const requestCreateSubject = reactive<RequestCreateSubjectInterface>({
    id: 0,
    name: '',
    code: '',
    schoolId: 0,
  });
  const errorSubjectKeys: (keyof ErrorResponseCreateSubjectInterface)[] = ['Name', 'Code', 'SchoolId'];

  const errorSubject = reactive<ErrorResponseCreateSubjectInterface>({
    Name: [],
    Code: [],
    SchoolId: [],
  });
  const getSubjects = async (
    success: (res: ResponseGetSubjectInterface[]) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await subjectFactory.getAllSubject(
      res => {
        subjects.value = res.data as ResponseGetSubjectInterface[];
        return success(res.data as ResponseGetSubjectInterface[]);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const createSubject = async (
    success: (res: ResponseGetSubjectInterface) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await subjectFactory.createSubject(
      requestCreateSubject,
      res => {
        clearErrorKeys(errorSubjectKeys, errorSubject);
        return success(res.data as ResponseGetSubjectInterface);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseCreateSubjectInterface;
        mapErrorKeys(errorSubjectKeys, errorSubject, errorsResponse);
        error(err);
      },
    );
  };

  const updateSubject = async (
    id: number,
    success: (res: ResponseGetSubjectInterface) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await subjectFactory.EditSubject(
      id,
      requestCreateSubject,
      res => {
        clearErrorKeys(errorSubjectKeys, errorSubject);
        return success(res.data as ResponseGetSubjectInterface);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseCreateSubjectInterface;
        mapErrorKeys(errorSubjectKeys, errorSubject, errorsResponse);
        error(err);
      },
    );
  };

  const deleteSubject = async (
    id: number,
    success: (res: ResponseGetSubjectInterface) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await subjectFactory.deleteSubject(
      id,
      res => {
        return success(res.data as ResponseGetSubjectInterface);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    requestCreateSubject,
    errorSubject,
    subjects,
    getSubjects,
    createSubject,
    updateSubject,
    deleteSubject,
  };
});
