import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { RequestBusRouteIndex, ResponseBusRouteData, CreateBusRoute, BusRouteDetail, BusRoute } from './../types/model/bus-route';
import RepositoryFactory from '@/repositories/repository';
import { BusRouteRepository } from '@/repositories/repository-bus-route';
import { defineStore } from 'pinia';
import { reactive, ref } from 'vue';

export const useBusRouteStore = defineStore('busRoute', () => {
  const busRouteFactory = RepositoryFactory.create('busRoute') as BusRouteRepository;
  const RequestBusRouteIndex = reactive<RequestBusRouteIndex>({
    pageNumber: null,
    pageSize: 5,
    status: null,
    keyword: null,
  });

  const dataBusRoute = reactive<{ value: ResponseBusRouteData }>({
    value: {
      pageNumber: 1,
      pageSize: 3,
      firstPage: '',
      lastPage: '',
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });
  const dataBusRouteDetail = reactive<{value: BusRouteDetail}>({
    value: {
      id: 1,
      name: '',
      description: '',
      status: 1,
      busStops: [],
      buses: []
    }  
  });

  const getAllBusRoute = async () => {
    await busRouteFactory.getAllBusRoute(
      RequestBusRouteIndex,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusRoute.value = response;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const getBusRouteDetail = async (id: number) => {
    await busRouteFactory.getBusRouteDetail(
      id,
      (res: SuccessResponse) => {
        const response = res.data;
        dataBusRouteDetail.value = response;   
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const createBusRoute = async (
    params: CreateBusRoute,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busRouteFactory.addBusRoute(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateBusRoute = async (
    id: number,
    params: CreateBusRoute,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busRouteFactory.updateBusRoute(
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

  const deleteBusRoute = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busRouteFactory.deleteBusRoute(
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
    dataBusRoute,
    RequestBusRouteIndex,
    dataBusRouteDetail,
    deleteBusRoute,
    getBusRouteDetail,
    updateBusRoute,
    createBusRoute,
    getAllBusRoute,
  };
});
