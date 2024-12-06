import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { AttendanceRepository } from '@/repositories/repository-attendance';
import { RequestGetListAttendance, RequestGetAttendanceRecord, ResponseGetListAttendance, ResponseGetAttendanceRecord } from '@/types/model/class-attendance';

export const useAttendanceStore = defineStore('attendance', () => {
  const attendanceFactory = RepositoryFactory.create('attendance') as AttendanceRepository;

  const getListAttendance = async (
    params: RequestGetListAttendance,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.getListAttendance(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetListAttendance[],
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

  const getAttendanceRecord = async (
    params: RequestGetAttendanceRecord,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.getAttendanceRecord(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetAttendanceRecord[],
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

  const prepareCreateAttendance = async (
    params: RequestGetListAttendance,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.prepareCreateAttendance(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
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
  }

  const createAttendance = async (
    data: ResponseGetAttendanceRecord[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.createAttendance(
      data,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
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

  const updateAttendance = async (
    data: ResponseGetAttendanceRecord[],
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.updateAttendance(
      data,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
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

  const getPupilAttendance = async (
    params: {semesterId: number, date: Date},
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await attendanceFactory.getPupilAttendance(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data,
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
  }
  return {
    getPupilAttendance,
    createAttendance,
    getListAttendance,
    getAttendanceRecord,
    prepareCreateAttendance,
    updateAttendance
  };
});