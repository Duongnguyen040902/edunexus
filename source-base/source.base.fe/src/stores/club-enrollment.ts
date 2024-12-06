import { UpdateClubEnrollment } from './../types/model/club-enrollment';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { ClubEnrollmentRepository } from '@/repositories/repository-club-enrollment';
import {
  CreateClubEnrollment,
  RequestClubEnrollmentIndex,
  ResponseClubEnrollmentData,
  TeacherDTOResponse,
  PupilDTOResponse,
  ClubEnrollment,
} from '@/types/model/club-enrollment';
import { defineStore } from 'pinia';
import { reactive } from 'vue';

export const useClubEnrollmentStore = defineStore('clubEnrollment', () => {
  const clubEnrollmentFactory = RepositoryFactory.create('clubEnrollment') as ClubEnrollmentRepository;

  const requestClubEnrollmentIndex = reactive<RequestClubEnrollmentIndex>({
    pageNumber: 1,
    pageSize: 10,
    keyword: null,
    clubId: null,
    semesterId: null,
  });

  const dataClubEnrollment = reactive<{ value: ResponseClubEnrollmentData }>({
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

  const dataTeachersNotInClub = reactive<TeacherDTOResponse[]>([]);
  const dataPupilsNotInClub = reactive<PupilDTOResponse[]>([]);
  const dataPupilRegisterClub = reactive<ClubEnrollment[]>([]);
  const dataCopyClub = reactive<ClubEnrollment[]>([]);

  const getBySemester = async () => {
    await clubEnrollmentFactory.getBySemester(
      requestClubEnrollmentIndex,
      (res: SuccessResponse) => {
        const response = res.data;
        dataClubEnrollment.value = response;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const getTeachersNotInClub = async (clubId: number, semesterId: number) => {
    await clubEnrollmentFactory.getTeachersNotInClub(
      clubId,
      semesterId,
      (res: SuccessResponse) => {
        dataTeachersNotInClub.value = res.data;
      },
      (err: ErrorResponse) => {
        dataTeachersNotInClub.value = [];
      },
    );
  };

  const getMemberToCopyClub = async (nextSemesterId: number) => {
    await clubEnrollmentFactory.getMemberToCopyClub(
      nextSemesterId,
      (res: SuccessResponse) => {
        dataCopyClub.value = res.data;
      },
      (err: ErrorResponse) => {
        dataCopyClub.value = [];
      },
    );
  };

  const getPupilsNotInClub = async (clubId: number, semesterId: number) => {
    await clubEnrollmentFactory.getPupilsNotInClub(
      clubId,
      semesterId,
      (res: SuccessResponse) => {
        dataPupilsNotInClub.value = res.data;
      },
      (err: ErrorResponse) => {
        dataPupilsNotInClub.value = [];
      },
    );
  };

  const getPupilsRegisterClub = async (clubId: number, semesterId: number) => {
    await clubEnrollmentFactory.getPupilsRegisterClub(
      clubId,
      semesterId,
      (res: SuccessResponse) => {
        dataPupilRegisterClub.value = res.data; 
        console.log("bb",dataPupilRegisterClub.value);
      },
      (err: ErrorResponse) => {
        dataPupilRegisterClub.value = [];
      },
    );
  };

  const createClubEnrollment = async (
    params: CreateClubEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubEnrollmentFactory.createClubEnrollment(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateClubEnrollment = async (
    params: UpdateClubEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubEnrollmentFactory.updateClubEnrollment(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const removeClubEnrollment = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubEnrollmentFactory.removeClubEnrollment(
      id,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    dataClubEnrollment,
    dataTeachersNotInClub,
    dataPupilsNotInClub,
    requestClubEnrollmentIndex,
    dataPupilRegisterClub,
    dataCopyClub,
    getMemberToCopyClub,
    getPupilsRegisterClub,
    getBySemester,
    getTeachersNotInClub,
    getPupilsNotInClub,
    createClubEnrollment,
    updateClubEnrollment,
    removeClubEnrollment,
  };
});
