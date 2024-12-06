import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { Bus, CreateBus, RequestBusIndex, ResponseBusData } from '@/types/model/bus';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { BusRepository } from '@/repositories/repository-bus';
import { ResponseGetBus, ResponseGetBusDetail, ViewBusEnrollDetailDTO } from '@/types/model/bus';
import { reactive } from 'vue';
export const useBusStore = defineStore('bus', () => {
  const busFactory = RepositoryFactory.create('bus') as BusRepository;

  const getBusForAdmin = async (id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    dataBusDetail.value=[];
    await busFactory.getBusForAdmin(
      id,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusDetail.value = response;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
        dataBusDetail.value=[];
      },
    );
  };

  const getBusDetail = async (
    params: { busId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busFactory.getBusDetail(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetBusDetail,
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getAssignedBus = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await busFactory.getAssignedBus(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetBus,
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getEnrolledBus = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await busFactory.getEnrolledBus(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetBus,
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getBusEnrollDetail = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await busFactory.getBusEnrollDetail(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ViewBusEnrollDetailDTO[],
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const getBusDetailOfPupil = async (
    params: { busId: number; semesterId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busFactory.getBusDetailOfPupil(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetBusDetail,
          message: res.message,
          succeeded: res.succeeded,
        };
        return success(successResponse);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const RequestBusIndex = reactive<RequestBusIndex>({
    pageNumber: 1,
    pageSize: 10,
    status: null,
    keyword: null,
    busRouteId: 0,
  });

  const dataBus = reactive<{ value: ResponseBusData }>({
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

  const dataBusDetail = reactive<Bus>({
    id: 1,
    name: '',
    driverName: '',
    driverPhone: '',
    licensePlate: '',
    seatNumber: 0,
    busRouteId: 0,
    status: 0,
  });

  const getAllBuses = async (busRouteId: number) => {
    RequestBusIndex.busRouteId = busRouteId;
    await busFactory.getAllBuses(
      RequestBusIndex,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBus.value = response;
        console.log(dataBus.value.data);
      },
      (err: ErrorResponse) => {
      },
    );
  };

 

  const createBus = async (
    params: CreateBus,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busFactory.addBus(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateBus = async (
    id: number,
    params: CreateBus,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busFactory.updateBus(
      id,
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const deleteBus = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busFactory.deleteBus(
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
    getBusDetail,
    getAssignedBus,
    getEnrolledBus,
    getBusDetailOfPupil,
    getBusEnrollDetail,
    dataBus,
    RequestBusIndex,
    dataBusDetail,
    deleteBus,
    getBusForAdmin,
    updateBus,
    createBus,
    getAllBuses,
  };
});
