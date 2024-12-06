import { IndividualScore } from './../types/model/score';

import { ErrorCodes, reactive, ref } from 'vue';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { endLoading, startLoading } from '@/helpers/mixins';
import { useRoute, useRouter } from 'vue-router';
import { useSemesterStore } from '@/stores/semester';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';
import { useScoreStore } from '@/stores/score';
import { ErrorScore, ResponseClassScoreListDTO, ResponseGetScore } from '@/types/model/score';
import { useSubjectStore } from '@/stores/subject';

export function useScoreComposable() {
  const scoreStore = useScoreStore();
  const { getClassScore, getPupilForCreate, getPupilScores, updatePupilScores, createPupilScores } = scoreStore;
  const { currentSemester, fetchCurrentSemester, semester, fetchSemester } = useSemesterStore();
  const { subjects, getSubjects } = useSubjectStore();
  const router = useRouter();
  const route = useRoute();
  const classId = ref<number>(parseInt(route.query.classId as string));
  const semesterId = ref<number>();
  const subjectId = ref<number>();
  const entityId = ref<number>();

  const scores = ref<ResponseGetScore[]>([]);
  const isShowModal = ref<boolean>(false);
  const isUpdate = ref<boolean>(false);

  const classScore = ref<ResponseClassScoreListDTO>();
  const pupilScores = ref<ResponseGetScore[]>([]);
  const individualScore = ref<IndividualScore[]>([]);

  const errorScore = reactive<ErrorScore>({
    subjectId: [],
    pupilId: [],
    score: [],
    semesterId: [],
    status: [],
  });

  const errorScoreKeys: (keyof ErrorScore)[] = ['subjectId', 'pupilId', 'score', 'semesterId', 'status'];

  const fetchClassScore = async (params: { classId: number; semesterId: number }) => {
    startLoading();
    try {
      await getClassScore(
        params,
        (res: SuccessResponse) => {
          classScore.value = res.data as ResponseClassScoreListDTO;
        },
        (err: ErrorResponse) => {
          classScore.value = undefined;
          notifyError(err.message);
        },
      );
    } finally {
      endLoading();
    }
  };

  const fetchPupilForCreate = async (params: { entityId: number; semesterId: number; subjectId: number }) => {
    try {
      await getPupilForCreate(
        params,
        (res: SuccessResponse) => {
          pupilScores.value = res.data as ResponseGetScore[];
        },
        (err: ErrorResponse) => {
          pupilScores.value = [];
          notifyError(err.message);
          setTimeout(() => {
            isUpdate.value = false;
            isShowModal.value = false;
          }, 200);
        },
      );
    } catch (error) {
      pupilScores.value = [];
    }
  };

  const handleCreateScore = async (params: ResponseGetScore[], emit: any) => {
    startLoading();
    try {
      await createPupilScores(
        params,
        (res: SuccessResponse) => {
          notifySuccess(res.message);

          clearErrorKeys(errorScoreKeys, errorScore);
          emit('update:showModal', false);
          emit('refreshList');
        },
        (err: ErrorResponse) => {
          handleErrors(err);
        },
      );
    } finally {
      endLoading();
    }
  };

  const handleErrors = (err: ErrorResponse) => {
    const errors = err.errors as ErrorScore;
    mapErrorKeys(errorScoreKeys, errorScore, errors);
  };

  const fetchPupilScores = async (params: { entityId: number; semesterId: number; subjectId: number }) => {
    startLoading();
    try {
      await getPupilScores(
        params,
        (res: SuccessResponse) => {
          pupilScores.value = res.data as ResponseGetScore[];
        },
        (err: ErrorResponse) => {
          pupilScores.value = [];
        },
      );
    } finally {
      endLoading();
    }
  };
  const handleUpdateScore = async (params: ResponseGetScore[], emit: any) => {
    startLoading();
    try {
      await updatePupilScores(
        params,
        (res: SuccessResponse) => {
          notifySuccess(res.message);
          clearErrorKeys(errorScoreKeys, errorScore);
          emit('update:showModal', false);
          emit('update:isUpdateMode', false);
          emit('refreshList');
        },
        (err: ErrorResponse) => {
          handleErrors(err);
        },
      );
    } finally {
      endLoading();
    }
  };

  const fetchIndividualScore = async (params: { semesterId: number }) => {
    startLoading();
    try {
      await scoreStore.getIndividualScore(
        params,
        (res: SuccessResponse) => {
          individualScore.value = res.data as IndividualScore[];
          console.log(individualScore.value);
        },
        (err: ErrorResponse) => {
          individualScore.value = [];
        },
      );
    } finally {
      endLoading();
    }
  };

  const handleCloseModal = (emit: any) => {
    clearErrorKeys(errorScoreKeys, errorScore);
    emit('update:showModal', false);
    emit('update:isUpdateMode', false);
  };

  return {
    fetchIndividualScore,
    individualScore,
    errorScore,
    handleUpdateScore,
    pupilScores,
    fetchPupilScores,
    fetchPupilForCreate,
    handleCreateScore,
    handleCloseModal,
    scores,
    isShowModal,
    isUpdate,
    classScore,
    classId,
    semesterId,
    subjectId,
    entityId,
    fetchClassScore,
    currentSemester,
    fetchCurrentSemester,
    subjects,
    getSubjects,
    semester,
    fetchSemester,
  };
}
