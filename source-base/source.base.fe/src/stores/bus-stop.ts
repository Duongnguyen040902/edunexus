import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { BusStopRepository } from '@/repositories/repository-bus-stop';
import { BusStop, CreateBusStop, RequestBusStopIndex, ResponseBusStopData } from '@/types/model/bus-stop';
import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';

export const useBusStopStore = defineStore('busStop', () => {
  const busStopFactory = RepositoryFactory.create('busStop') as BusStopRepository;
  
  const RequestBusStopIndex = reactive<RequestBusStopIndex>({
    pageNumber: 1,
    pageSize: 10,
    status: null,
    keyword: null,
    busRouteId: 0,
  });

  const dataBusStop = reactive<{ value: ResponseBusStopData }>({
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
  
  const dataBusStopDetail = reactive<BusStop>({
    id: 1,
    name: '',
    pickUpTime: '',
    returnTime: '',
    address: '',
    busRouteId: 0,
    status: 0,
    index: 0,
  });

  const getAllBusStop = async () => {
    await busStopFactory.getAllBusStop(
      RequestBusStopIndex,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusStop.value = response;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const getBusStopDetail = async (id: number) => {
    await busStopFactory.getBusStopDetail(
      id,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusStopDetail.value = response;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const createBusStop = async (
    params: CreateBusStop,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busStopFactory.addBusStop(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateBusStop = async (
    id: number,
    params: CreateBusStop,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busStopFactory.updateBusStop(
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

  const deleteBusStop = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busStopFactory.deleteBusStop(
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
    dataBusStop,
    RequestBusStopIndex,
    dataBusStopDetail,
    deleteBusStop,
    getBusStopDetail,
    updateBusStop,
    createBusStop,
    getAllBusStop,
  };
});
