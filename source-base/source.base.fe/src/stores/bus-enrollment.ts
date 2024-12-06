import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { BusEnrollmentRepository } from '@/repositories/repository-bus-enrollment';
import {
  BusSupervisorDTOResponse,
  CreateBusEnrollment,
  PupilDTOResponse,
  RequestBusEnrollmentIndex,
  ResponseBusEnrollmentData,
} from '@/types/model/bus-enrollment';
import { defineStore } from 'pinia';
import { reactive } from 'vue';

export const useBusEnrollmentStore = defineStore('bus-enrollment', () => {
  const busEnrollmentFactory = RepositoryFactory.create('bus_enrollment') as BusEnrollmentRepository;

  const requestBusEnrollmentIndex = reactive<RequestBusEnrollmentIndex>({
    pageNumber: 1,
    pageSize: 10,
    busId: null,
    semesterId: null,
  });
  const dataPupils = reactive<PupilDTOResponse>({
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    gender: false,
    genderName: '',
    donorName: '',
    donorPhoneNumber: '',
    address: '',
    dateOfBirth: '',
    schoolId: 0,
    schoolName: '',
    accountStatus: 0,
    accountStatusName: '',
    username: '',
    image: '',
  });

  const dataBusSupervisor = reactive<BusSupervisorDTOResponse>({
    id: 0,
    firstName: '',
    lastName: '',
    phoneNumber: '',
    address: '',
    gender: false,
    schoolId: 0,
  });

  const dataBusEnrollment = reactive<{ value: ResponseBusEnrollmentData }>({
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

  const dataPupilsInBusStop = reactive<PupilDTOResponse>({
    id: 0,
    firstName: '',
    lastName: '',
    email: '',
    gender: false,
    genderName: '',
    donorName: '',
    donorPhoneNumber: '',
    address: '',
    dateOfBirth: '',
    schoolId: 0,
    schoolName: '',
    accountStatus: 0,
    accountStatusName: '',
    username: '',
    image: '',
  });
  const dataBusEnrollmentDetail = reactive<CreateBusEnrollment>({
    busId: 0,
    pupilId: null,
    semesterId: 0,
    busSupervisorId: null,
    busStopId: null,
  });

  const getAllBusEnrollments = async () => {
    await busEnrollmentFactory.getAllBusEnrollments(
      requestBusEnrollmentIndex,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusEnrollment.value = response;
        console.log(dataBusEnrollment.value.data);
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const getAllPupilWithoutBus = async (semesterId: number) => {
    await busEnrollmentFactory.getPupilsWithoutBus(
      semesterId,
      (res: SuccessResponse) => {
        const response = res.data;
        dataPupils.value = response;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const getAllPupilInBusStop = async (semesterId: number, busStopId : number) => {
    await busEnrollmentFactory.getPupilsInBusStop(
      semesterId,
      busStopId,
      (res: SuccessResponse) => {
        const response = res.data;
        dataPupilsInBusStop.value = response;
      },
      (err: ErrorResponse) => {
        dataPupilsInBusStop.value = [];
      },
    );
  };

  const getAllBusSupervisorWithoutBus = async (semesterId: number) => {
    await busEnrollmentFactory.getBusSupervisorWithoutBus(
      semesterId,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusSupervisor.value = response;
        console.log(dataBusSupervisor.value.data);
      },
      (err: ErrorResponse) => {
        dataBusSupervisor.value = [];
      },
    );
  };

  const getBusEnrollmentDetail = async (id: number) => {
    await busEnrollmentFactory.getBusEnrollmentDetail(
      id,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusEnrollmentDetail.value = response;
        console.log('detail', dataBusEnrollmentDetail.value);
      },
      (err: ErrorResponse) => {
        console.log('err', err);
        dataBusEnrollmentDetail.value = [];
      },
    );
  };

  const createBusEnrollment = async (
    params: CreateBusEnrollment[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busEnrollmentFactory.addBusEnrollment(
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

  const updateBusEnrollment = async (
    id: number,
    params: CreateBusEnrollment,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busEnrollmentFactory.updateBusEnrollment(
      id,
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

  const deleteBusEnrollment = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busEnrollmentFactory.deleteBusEnrollment(
      id,
      res => {
        success(res);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  return {
    dataBusEnrollment,
    dataPupils,
    requestBusEnrollmentIndex,
    dataBusEnrollmentDetail,
    dataBusSupervisor,
    dataPupilsInBusStop,
    getAllBusSupervisorWithoutBus,
    getAllPupilInBusStop,
    getAllPupilWithoutBus,
    deleteBusEnrollment,
    getBusEnrollmentDetail,
    updateBusEnrollment,
    createBusEnrollment,
    getAllBusEnrollments,
  };
});
