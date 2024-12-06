import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { PupilProfileRepository } from '@/repositories/repository-pupilProfile';
import { reactive } from 'vue';
import { ErrorResponse,SuccessResponse } from '@/constants/api/responses';
import { RequestUpdateDataPupilInterface } from '@/types/model/pupil';

export const usePupilStore = defineStore('pupil', () => {
  const pupilFactory = RepositoryFactory.create('pupil') as PupilProfileRepository;
  const pupilProfile = reactive<{ value: RequestUpdateDataPupilInterface | null }>({ value: null });

  const getPupilProfile = async (
    success: (res: RequestUpdateDataPupilInterface) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await pupilFactory.getPupilProfile(
      res => {
        pupilProfile.value = res.data as RequestUpdateDataPupilInterface;
        return success(res.data as RequestUpdateDataPupilInterface);
      },
      err => {
        error(err);
      }
    );
  };

  const updatePupil = async (
    params: RequestUpdateDataPupilInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await pupilFactory.updatePupil(
      params,
      res => {
        success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    pupilProfile,
    updatePupil,
    getPupilProfile,
  };
});