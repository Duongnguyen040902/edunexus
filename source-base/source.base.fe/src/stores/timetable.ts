import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { reactive } from 'vue';
import { ErrorResponse } from '@/constants/api/responses';
import { TimetableRepository } from '@/repositories/repository-timetable';
import {
  RequestGetTimetableInterface,
  ResponseGetTimetableInterface,
  RequestCreateAndUpdateTimetableInterface,
} from '@/types/model/timetable';

export const useTimetableStore = defineStore('timetable', () => {
  const timetableFactory = RepositoryFactory.create('timetable') as TimetableRepository;

  const timetable = reactive<{ value: ResponseGetTimetableInterface[] }>({
    value: [],
  });

  const fetchTimetable = async (
    params: RequestGetTimetableInterface,
    success: (res: ResponseGetTimetableInterface[]) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timetableFactory.getTimeTableDetail(
      params,
      res => {
        timetable.value = res.data as ResponseGetTimetableInterface[];
        return success(timetable.value);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const createTimetable = async (
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: ResponseGetTimetableInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timetableFactory.createTimeTable(
      data,
      res => {
        timetable.value.push(res.data as ResponseGetTimetableInterface);
        return success(res.data as ResponseGetTimetableInterface);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const deleteTimeTable = async (
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: ResponseGetTimetableInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timetableFactory.deleteTimeTable(
      data,
      res => {
        return success(res.data as ResponseGetTimetableInterface);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  const updateTimetable = async (
    data: RequestCreateAndUpdateTimetableInterface,
    success: (res: ResponseGetTimetableInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await timetableFactory.updateTimeTable(
      data,
      res => {
        return success(res.data as ResponseGetTimetableInterface);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };

  return {
    fetchTimetable,
    createTimetable,
    deleteTimeTable,
    updateTimetable,
    timetable,
  };
});
