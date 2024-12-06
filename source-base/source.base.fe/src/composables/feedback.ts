import { ErrorFeedback, FeedbackDetail, PupilFeedback } from '@/types/model/feedback.ts';
import { reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useFeedbackStore } from '@/stores/feedback.ts';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state.ts';
import { notifySuccess } from '@/helpers/notify.ts';

export const useFeedbackComposable = () => {
  const router = useRouter();
  const route = useRoute();
  const feedbackStore = useFeedbackStore();
  const classFeedback = ref<PupilFeedback[]>([]);
  const feedback = ref<PupilFeedback | null>(null);
  const classId = ref<number>(parseInt(route.query.classId as string));
  const isShowModal = ref<boolean>(false);
  const isUpdate = ref<boolean>(false);
  const isActive = 1;
  const errorFeedback = reactive<ErrorFeedback>({
    PupilId: [],
    Description: [],
    SemesterId: [],
    Status: [],
  });
  const isShowDeleteModal = ref<boolean>(false);

  const pupilFeedbacks = ref<FeedbackDetail[]>([]);

  const errorFeedbackKey: (keyof ErrorFeedback)[] = ['PupilId', 'Description', 'SemesterId', 'Status'];

  const fetchClassFeedback = async () => {
    try {
      await feedbackStore.getClassFeedback(
        { classId: classId.value },
        res => {
          classFeedback.value = res.data as PupilFeedback[];
        },
        err => {
          console.error('Error fetching class feedback:', err);
          classFeedback.value = [];
        },
      );
    } catch (error) {
      console.error('Error fetching class feedback:', error);
    }
  };

  const createFeedback = async (emit: any, data: PupilFeedback) => {
    try {
      data.status = isActive;
      await feedbackStore.createFeedback(
        data,
        res => {
          clearErrorKeys(errorFeedbackKey, errorFeedback);
          isShowModal.value = false;
          fetchClassFeedback();
          emit('reload-list', classFeedback.value);
          emit('update-showModal', false);
          notifySuccess("Tạo phản hồi thành công");
        },
        err => {
          console.error('Error creating feedback:', err);
          handleErrors(err);
        },
      );
    } catch (error) {
      console.error('Error creating feedback:', error);
    }
  };

  const handleErrors = (err: any) => {
    const errors = err.errors as ErrorFeedback;
    mapErrorKeys(errorFeedbackKey, errorFeedback, errors);
  };

  const updateFeedback = async (emit: any, data: PupilFeedback) => {
    try {
      await feedbackStore.updateFeedback(
        data,
        res => {
          clearErrorKeys(errorFeedbackKey, errorFeedback);
          isShowModal.value = false;
          isUpdate.value = false;
          fetchClassFeedback();
          emit('reload-list', classFeedback.value);
          emit('update-showModal', false);
          notifySuccess("Cập nhật phản hồi thành công");
        },
        err => {
          handleErrors(err);
        },
      );
    } catch (error) {
      console.error('Error updating feedback:', error);
    }
  };

  const deleteFeedback = async (emit: any, pupilId: number, semesterId: number) => {
    try {
      await feedbackStore.deleteFeedback(
        { pupilId, semesterId },
        res => {
          isShowModal.value = false;
          fetchClassFeedback();
          emit('reload-list', classFeedback.value);
          emit('update-showModal', false);
          notifySuccess("Xóa phản hồi thành công");
        },
        err => {
          console.error('Error deleting feedback:', err);
        },
      );
    } catch (error) {
      console.error('Error deleting feedback:', error);
    }
  };

  const fetchPupilFeedbackList = async () => {
    try {
      await feedbackStore.getListFeedbackOfPupil(
        res => {
          pupilFeedbacks.value = res.data as FeedbackDetail[];
        },
        err => {
          console.error('Error fetching pupil feedback:', err);
          pupilFeedbacks.value = [];
        },
      );
    } catch (error) {
      console.error('Error fetching pupil feedback:', error);
    }
  };

  const handleCloseModal = (emit: any) => {
    emit('update-showModal', false);
    clearErrorKeys(errorFeedbackKey, errorFeedback);
  };

  return {
    isShowDeleteModal,
    classFeedback,
    feedback,
    classId,
    isShowModal,
    isUpdate,
    errorFeedback,
    pupilFeedbacks,
    fetchClassFeedback,
    createFeedback,
    handleErrors,
    updateFeedback,
    handleCloseModal,
    deleteFeedback,
    fetchPupilFeedbackList,
  };
};
