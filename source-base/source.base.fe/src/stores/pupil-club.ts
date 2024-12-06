import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { PupilClubRepository } from '@/repositories/repository-pupil-club';
import {
  RequestGetClubIndex,
  RequestCreateAndUpdateClubEnrollment,
  ResponseGetClubIndex,
  ResponseGetClubEnrollment,
  ReponseGetNextSemester,
  ReponseGetSemesters,
} from '@/types/model/pupil-club';
import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';

export const useClubStore = defineStore('pupilClub', () => {
  const clubFactory = RepositoryFactory.create('pupilClub') as PupilClubRepository;
  const semesters = reactive<ReponseGetSemesters>({
    data: [],
  });
  const requestGetClubIndex = reactive<RequestGetClubIndex>({
    pageNumber: 1,
    pageSize: 10,
  });

  const dataClubIndex = reactive<{ value: ResponseGetClubIndex }>({
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

  const requestCreateAndUpdateClubEnrollment = reactive<RequestCreateAndUpdateClubEnrollment>({
    clubId: 0,
    status: 0,
  });

  const dataClubEnrollment = reactive<{ value: ResponseGetClubEnrollment }>({
    value: {
      data: [],
    },
  });

  const nextSemester = reactive<ReponseGetNextSemester>({
    id: 0,
    semesterName: '',
    semesterCode: '',
    startDate: '',
    endDate: '',
    isActive: false,
    schoolYearId: 0,
  });

  const getClubsBySemesterActive = async () => {
    await clubFactory.getClubsBySemesterActive(
      requestGetClubIndex,
      (res: SuccessResponse) => {
        const response = res.data as ResponseGetClubIndex;
        dataClubIndex.value = response;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const pupilCreateClubEnrollment = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.pupilCreateClubEnrollment(
      requestCreateAndUpdateClubEnrollment,
      res => {
        return success(res);
      },
      err => {
        return error(err);
      },
    );
  };

  const updateClubEnrollment = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.updateClubEnrollment(
      requestCreateAndUpdateClubEnrollment,
      res => {
        return success(res);
      },
      err => {
        return error(err);
      },
    );
  };

  const getNextSemester = async () => {
    await clubFactory.getNextSemester(
      res => {
        const response = res.data as ReponseGetNextSemester;
        nextSemester.id = response.id;
        nextSemester.semesterName = response.semesterName;
        nextSemester.semesterCode = response.semesterCode;
        nextSemester.startDate = response.startDate; 
        nextSemester.endDate = response.endDate;
        nextSemester.isActive = response.isActive;
        nextSemester.schoolYearId = response.schoolYearId;
      },
      err => {
        return err;
      },
    );
  }

  const getClubEnrollmentByPupilId = async (semesterId: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.getClubEnrollmentByPupilId(
      { semesterId: semesterId },
      res => {
        const response = res as ResponseGetClubEnrollment;
        dataClubEnrollment.value.data = response.data;
        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const getSemesters = async () => {
    await clubFactory.getSemesters(
      res => {
        const response = res as ReponseGetSemesters;
        semesters.data = response.data;
      },
      err => {
        return err;
      },
    );
  };
  return {
    semesters,
    requestGetClubIndex,
    getNextSemester,
    getSemesters,
    requestCreateAndUpdateClubEnrollment,
    dataClubIndex,
    dataClubEnrollment,
    nextSemester,
    getClubsBySemesterActive,
    pupilCreateClubEnrollment,
    updateClubEnrollment,
    getClubEnrollmentByPupilId,
  };
});