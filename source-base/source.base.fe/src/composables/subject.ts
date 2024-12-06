import { reactive, ref } from 'vue';
import { endLoading, startLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { useSubjectStore } from '@/stores/subject';
import { ResponseGetSubjectInterface } from '@/types/model/subject';

export const useSubjectComposable = () => {
  const subjectStore = useSubjectStore();
  const { requestCreateSubject, createSubject, updateSubject, deleteSubject, errorSubject, getSubjects } = subjectStore;
  const responseSubject = reactive<{ data: ResponseGetSubjectInterface[] }>({ data: [] });
  const isShowModalSubject = ref(false);
  const handleGetAllSubject = async () => {
    startLoading();
    try {
      await getSubjects(
        res => {
          responseSubject.data = res;
        },
        () => {
          notifyError('Lấy Môn học thất bại');
        },
      );
    } catch (error) {
      notifyError('An error occurred while fetching current subscription');
    } finally {
      endLoading();
    }
  };

  const handleOpenModalSubject = () => {
    isShowModalSubject.value = true;
  };

  const handleCloseModalSubject = () => {
    handleClearState();
  };

  const isEditSubject = ref(false);
  const handleSwapValueClickEdit = (subject: ResponseGetSubjectInterface) => {
    isEditSubject.value = true;
    isShowModalSubject.value = true;
    requestCreateSubject.id = subject.id;
    requestCreateSubject.name = subject.name;
    requestCreateSubject.code = subject.code;
    requestCreateSubject.schoolId = subject.schoolId;
  };

  const handleClearState = () => {
    isShowModalSubject.value = false;
    isEditSubject.value = false;
    requestCreateSubject.id = 0;
    requestCreateSubject.name = '';
    requestCreateSubject.code = '';
    requestCreateSubject.schoolId = 0;
  };

  const handleCreateSubject = async () => {
    startLoading();
    await createSubject(
      () => {
        notifySuccess('Tạo môn học thành công');
        endLoading();
        handleClearState();
        handleGetAllSubject();
      },
      () => {
        notifyError('Tạo môn học thất bại');
        endLoading();
      },
    );
  };

  const handleUpdateSubject = async (id: number) => {
    startLoading();
    await updateSubject(
      id,
      () => {
        notifySuccess('Cập nhật môn học thành công');
        endLoading();
        handleClearState();
        handleGetAllSubject();
      },
      () => {
        notifyError('Cập nhật môn học thất bại');
        endLoading();
      },
    );
  };

  const handleDeleteSubject = async () => {
    startLoading();
    await deleteSubject(
      idDelete.value,
      () => {
        notifySuccess('Xóa môn học thành công');
        endLoading();
        handleCloseModalDelete();
        handleGetAllSubject();
      },
      () => {
        notifyError('Xóa môn học thất bại');
        handleCloseModalDelete();
        endLoading();
      },
    );
  };

  const isShowModalDelete = ref(false);
  const idDelete = ref(0);
  const handleOpenModalDelete = (id: number) => {
    isShowModalDelete.value = true;
    idDelete.value = id;
  };
  const handleCloseModalDelete = () => {
    idDelete.value = 0;
    isShowModalDelete.value = false;
  };
  const handleConfirmWithStateSubject = async () => {
    if (isEditSubject.value) {
      await handleUpdateSubject(requestCreateSubject.id);
      return;
    }

    await handleCreateSubject();
  };
  return {
    idDelete,
    errorSubject,
    isShowModalDelete,
    isShowModalSubject,
    isEditSubject,
    responseSubject,
    requestCreateSubject,
    handleGetAllSubject,
    handleOpenModalSubject,
    handleCloseModalSubject,
    handleSwapValueClickEdit,
    handleCreateSubject,
    handleUpdateSubject,
    handleDeleteSubject,
    handleOpenModalDelete,
    handleCloseModalDelete,
    handleConfirmWithStateSubject,
  };
};
