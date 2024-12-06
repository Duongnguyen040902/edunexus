import { RequestDeleteBusSupervisor, RequestGetBusSupervisorDetailInterface, ResponseBusSupervisorDetailInterface } from './../types/model/bus-supervisor';
import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { BusSupervisorRepository } from '@/repositories/repository-bus-supervisor';
import { reactive } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { RequestCreateBusSupervisorInterface, RequestListBusSupervisorInterface, ResponseListBusSupervisorInterface, UpdateProfileBusSupervisor } from '@/types/model/bus-supervisor';
import { RequestImportExcelInterface } from '@/types/model/teacher-account';

export const useBusSupervisorStore = defineStore('busSupervisor', () => {
  const busSupervisorFactory = RepositoryFactory.create('busSupervisor') as BusSupervisorRepository;
  const busSupervisorProfile = reactive<{ value: UpdateProfileBusSupervisor | null }>({ value: null });
  const requestDeleteBusSupervisor = reactive<RequestDeleteBusSupervisor>({
    ids: [],
  });
  const requestImportExcelBusSupervisor = reactive<RequestImportExcelInterface>({
    file: new Blob(),
  });

  const getBusSupervisorProfile = async (
    success: (res: UpdateProfileBusSupervisor) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busSupervisorFactory.getBusSupervisorProfile(
      res => {
        busSupervisorProfile.value = res.data as UpdateProfileBusSupervisor;
        return success(res.data as UpdateProfileBusSupervisor);
      },
      err => {
        error(err);
      }
    );
  };

  const updateBusSupervisor = async (
    params: UpdateProfileBusSupervisor,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busSupervisorFactory.updateBusSupervisor(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const getListBusSupervisorAccount = async (
    params: RequestListBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) => {
    busSupervisorFactory.getAllBusSupervisors(
        params,
        (res) => {
            const successResponse: SuccessResponse = {
                data: res.data as ResponseListBusSupervisorInterface,
                message: res.message,
                succeeded: res.succeeded
            };
            return success(successResponse);
        },
        (err) => {
            error(err);
        }
    );
  };

  const createBusSupervisor = async (
    params: RequestCreateBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void) => {
    await busSupervisorFactory.createBusSupervisor(
        params,
        res => {
            success(res);
        },
        err => {
            console.log('error', err);
            error(err);
        }
    );
  };
  const getBusSupervisorDetail = async (
    params: RequestGetBusSupervisorDetailInterface,
    success: (res: ResponseBusSupervisorDetailInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await busSupervisorFactory.getBusSupervisorDetail(
        params,
        res => {
            console.log('res-store', res);
            res.data as ResponseBusSupervisorDetailInterface;
            return success(res.data as ResponseBusSupervisorDetailInterface);
        },
        err => {
            console.log('error', err);
            error(err);
        }
    );
  };
  const updateBusSupervisorAccount = async (
    busSupervisorId: number,
    params: RequestCreateBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
    ) => {
      console.log('params-store', params);

      await busSupervisorFactory.updateBusSupervisorAccount(
        busSupervisorId,
          params,
          res => {
              success(res);
          },
          err => {
              console.log('error', err);
              error(err);
          }
      );
  };

const deleteBusSupervisors = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await busSupervisorFactory.deleteBusSupervisor(
        requestDeleteBusSupervisor,
        res => {
            requestDeleteBusSupervisor.ids = [];
            success(res);
        },
        err => {
            console.log(err);
            error(err);
        },
    );
};

const importExcelBusSupervisorAccount = async (
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
) => {
    await busSupervisorFactory.importExcelBusSupervisor(
        requestImportExcelBusSupervisor,
        res => {
            success(res);
        },
        err => {
            if (err.data?.fileName && err.data?.fileContent) {
                const binaryContent = atob(err.data?.fileContent);
                const uint8Array = new Uint8Array([...binaryContent].map(char => char.charCodeAt(0)));
                const decoder = new TextDecoder('utf-8');
                const decodedContent = decoder.decode(uint8Array);
                const blob = new Blob([decodedContent], { type: 'text/plain;charset=utf-8' });
                const link = document.createElement('a');
                link.href = URL.createObjectURL(blob);
                link.download = err.data?.fileName;
                link.click();
            }
            const errorResponse: ErrorResponse = {
                ...err,
                errors: err.errors,
            };
            return error(errorResponse);
        },
    );
};
  return {
    busSupervisorProfile,
    requestImportExcelBusSupervisor,
    requestDeleteBusSupervisor,
    updateBusSupervisor,
    getBusSupervisorProfile,
    getListBusSupervisorAccount,
    createBusSupervisor,
    getBusSupervisorDetail,
    updateBusSupervisorAccount,
    deleteBusSupervisors,
    importExcelBusSupervisorAccount,
  };
});