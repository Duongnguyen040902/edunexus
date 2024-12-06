import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { TimeSlotRepository } from '@/repositories/repository-timeslot';
import { RequestCreateTimeSlotInterface, RequestGetAllTimeSlotInterface, ResponseGetTimeSlotInterface } from '@/types/model/timeslot';
import { reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { RequestGetTimeSlotInterface } from '@/types/model/timeslot';

export const useTimeSlotStore = defineStore('time_slot', () => {
  const timeSlotFactory = RepositoryFactory.create('time_slot') as TimeSlotRepository;
  const timeSlots = reactive<{ value: ResponseGetTimeSlotInterface[] }>({ value: [] });

  const getTimeSlots = async (
    success: (res: ResponseGetTimeSlotInterface[]) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timeSlotFactory.getAllTimeSlot(
      res => {
        timeSlots.value = res.data as ResponseGetTimeSlotInterface[];
        return success(res.data as ResponseGetTimeSlotInterface[]);
      },
      err => {
        error(err);
      },
    );
  };

  const requestGetAllTimeSlots = reactive<RequestGetAllTimeSlotInterface>({
    pageNumber: 1,
    pageSize: 10,
    isActive: null,
    keyword: '',
  });

  const timeSlotData = reactive<{ value: ResponseGetTimeSlotInterface | null }>({
    value: null,
  });

  const timeSlotDetail = reactive< {value:ResponseGetTimeSlotInterface | null }>({
    value: null,
  });

  const getAllTimeSlots = async () => {
    
    await timeSlotFactory.getAllTimeSlots(
      requestGetAllTimeSlots,
      (res: SuccessResponse) => {
        timeSlotData.value = res.data;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const getTimeSlotDetail = async (id: number) => {
    await timeSlotFactory.getTimeSlotDetail(
      id,
      (res: SuccessResponse) => {
        timeSlotDetail.value = res.data;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const createTimeSlot = async (
    params: RequestCreateTimeSlotInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timeSlotFactory.addTimeSlot(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const updateTimeSlot = async (
    id: number,
    params: RequestCreateTimeSlotInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timeSlotFactory.updateTimeSlot(
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

  const deleteTimeSlot = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timeSlotFactory.deleteTimeSlot(
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
    getTimeSlots,
    requestGetAllTimeSlots,
    timeSlotData,
    timeSlotDetail,
    getAllTimeSlots,
    getTimeSlotDetail,
    createTimeSlot,
    updateTimeSlot,
    deleteTimeSlot,
  };
});
