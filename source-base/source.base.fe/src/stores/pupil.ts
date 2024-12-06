import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { reactive } from 'vue';
import { ErrorResponse } from '@/constants/api/responses';
import { PupilAssignRepository } from '@/repositories/repository-pupil';
import { ListPupilAssignRequestInterface, RequestGetPupilAssignInterface } from '@/types/model/pupil';

export const usePupilStore = defineStore('pupilAssign', () => {
  const pupilFactory = RepositoryFactory.create('pupilAssign') as PupilAssignRepository;
  const listPupilAssign = reactive<{ value: ListPupilAssignRequestInterface | null }>({ value: null });

  const getPupilAssign = async (
    params: RequestGetPupilAssignInterface,
    success: (res: ListPupilAssignRequestInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await pupilFactory.getListPupilAssign(
      params,
      res => {
        if (res.data) {
          listPupilAssign.value = res.data;
          return success(listPupilAssign.value);
        }
      },
      (err: ErrorResponse) => {
        console.error('Error fetching teacher assignments:', err);
      },
    );
  };

  return {
    getPupilAssign,
  };
});
