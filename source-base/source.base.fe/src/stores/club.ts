import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { ClubRepository } from '@/repositories/repository-club';
import {
  RequestCreateClubInterface,
  RequestGetClubInterface,
  ResponseClubInterface,
  ResponseGetClubDetailInterface,
  ResponseGetClubInterface,
} from '@/types/model/club';

export const useClubStore = defineStore('club', () => {
  const clubFactory = RepositoryFactory.create('club') as ClubRepository;
  const requestGetClub = reactive<RequestGetClubInterface>({
    pageNumber: 1,
    pageSize: 20,
    status: null,
    keyword: null,
  });
  const getClubDetail = async (
    params: { clubId: number },
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.getClubDetail(
      params,
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetClubDetailInterface,
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

  const getAssignedClub = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await clubFactory.getAssignedClub(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetClubInterface[],
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

  const getEnrolledClub = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await clubFactory.getEnrolledClub(
      res => {
        const successResponse: SuccessResponse = {
          data: res.data as ResponseGetClubInterface[],
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

  const dataClub = reactive<{ value: ResponseClubInterface }>({
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
  const dataClubDetail = reactive<ResponseGetClubInterface>({
    id: 0,
    name: '',
    description: '',
    status: 0,
    schoolId: 0,
  });

  const getAllClubsForAdmin = async () => {
    await clubFactory.getAllClubs(
      requestGetClub,
      res => {
        dataClub.value = res.data;
      },
      (err: ErrorResponse) => {
        console.error('Error fetching clubs:', err);
      },
    );
  };

  const getClubDetailForAdmin = async (id: number) => {
    await clubFactory.getClubDetailForAdmin(
      { clubId: id },
      (res: SuccessResponse) => {
        dataClubDetail.value = res.data;
        console.log('Club Detail:', dataClubDetail.value);
      },
      (err: ErrorResponse) => {
        console.error('Error fetching club detail:', err);
      },
    );
  };

  /**
   * Create a new club
   */
  const createClub = async (
    params: RequestCreateClubInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.createClub(
      params,
      res => {
        success(res);
      },
      err => {
        console.error('Error creating club:', err);
        error(err);
      },
    );
  };
  /**
   * Update an existing club
   */
  const updateClub = async (
    id: number,
    params: RequestCreateClubInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.updateClub(
      { clubId: id },
      params,
      res => {
        success(res);
      },
      err => {
        console.error('Error updating club:', err);
        error(err);
      },
    );
  };

  const deleteClub = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await clubFactory.deleteClub(
      { clubId: id },
      res => {
        success(res);
      },
      err => {
        console.error('Error deleting club:', err);
      },
    );
  };
  return {
    getClubDetail,
    getAssignedClub,
    getEnrolledClub,
    dataClub,
    dataClubDetail,
    requestGetClub,
    getAllClubsForAdmin,
    getClubDetailForAdmin,
    createClub,
    updateClub,
    deleteClub,
  };
});
