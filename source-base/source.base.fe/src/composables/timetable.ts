import { ref } from 'vue';
import { notifySuccess, notifyError } from '@/helpers/notify';
import { startLoading, endLoading } from '@/helpers/mixins';
import { ResponseGetTimetableInterface, RequestCreateAndUpdateTimetableInterface } from '@/types/model/timetable';
import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { useTimetableStore } from '@/stores/timetable';
import { useTimeSlotStore } from '@/stores/timeslot';
import { useSemesterStore } from '@/stores/semester';
import { useSubjectStore } from '@/stores/subject';
import { ResponseGetSubjectInterface } from '@/types/model/subject';
import { RequestGetTimeSlotInterface } from '@/types/model/timeslot';
import { useRoute, useRouter } from 'vue-router';

export const useTimetableComposable = () => {
  const route = useRoute();
  const router = useRouter();
  const timetableStore = useTimetableStore();
  const timeSlotStore = useTimeSlotStore();
  const subjectStore = useSubjectStore();
  const semesterStore = useSemesterStore();

  const timetableData = ref<ResponseGetTimetableInterface[]>([]);
  const timeSlotData = ref<RequestGetTimeSlotInterface[]>([]);
  const subjectData = ref<ResponseGetSubjectInterface[]>([]);
  const semesterData = ref<ResponseGetSemesterInterface[]>([]);
  const selectedSemesterId = ref<number | null>(null);
  const selectedSemester = ref<ResponseGetSemesterInterface | null>(null);

  const classId = ref<number>(parseInt(route.query.classId as string));

  const showModal = ref(false);
  const isUpdateMode = ref(false);
  const showDeleteModal = ref(false);
  const selectedTimeSlotId = ref<number | null>(null);
  const selectedDayOfWeek = ref<number | null>(null);
  const selectedSubjectId = ref<number | null>(null);

  const handleFetchTimetable = async (classId: number, semesterId: number) => {
    startLoading();
    try {
      await timetableStore.fetchTimetable(
        { classId, semesterId },
        res => {
          timetableData.value = res;
        },
        err => {
          timetableData.value = [];
        },
      );
    } catch (error) {
      timetableData.value = [];
    } finally {
      endLoading();
    }
  };

  const handleFetchTimeSlots = async () => {
    startLoading();
    try {
      await timeSlotStore.getTimeSlots(

        res => {
          timeSlotData.value = res;
        },
        err => {
          console.error('Error fetching time slots:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching time slots:', error);
    } finally {
      endLoading();
    }
  };

  const handleFetchSubjects = async () => {
    startLoading();
    try {
      await subjectStore.getSubjects(
        res => {
          subjectData.value = res;
        },
        err => {
          console.error('Error fetching subjects:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching subjects:', error);
    } finally {
      endLoading();
    }
  };

  const handleFetchSemester = async () => {
    startLoading();
    try {
      await semesterStore.fetchSemester(
        res => {
          semesterData.value = res;
        },
        err => {
          console.error('Error fetching semesters:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching semesters:', error);
    } finally {
      endLoading();
    }
  };

  const handleCreateTimetable = async (data: RequestCreateAndUpdateTimetableInterface) => {
    startLoading();
    try {
      await timetableStore.createTimetable(
        data,
        res => {
          timetableData.value.push(res);
          notifySuccess("Thêm thời khóa biểu thành công");
        },
        err => {
          notifyError("Thêm thời khóa biểu thất bại");
        },
      );
    } catch (error) {
      console.error('Error creating timetable:', error);
    } finally {
      endLoading();
    }
  };

  const handleDeleteTimetable = async (data: RequestCreateAndUpdateTimetableInterface) => {
    startLoading();
    try {
      await timetableStore.deleteTimeTable(
        data,
        res => {
          notifySuccess('Xóa thời khóa biểu thành công');
        },
        err => {
          notifyError('Xóa thời khóa biểu thất bại');
        },
      );
    } catch (error) {
      notifyError('Xóa thời khóa biểu thất bại');
    } finally {
      endLoading();
    }
  };

  const handleUpdateTimetable = async (data: RequestCreateAndUpdateTimetableInterface) => {
    startLoading();
    try {
      await timetableStore.updateTimetable(
        data,
        res => {
          notifySuccess('Cập nhật thời khóa biểu thành công');
        },
        err => {
          notifyError('Cập nhật thất bại');
        },
      );
    } catch (error) {
      notifyError('Cập nhật thất bại');
    } finally {
      endLoading();
    }
  };

  const findLatestSemesterWithTimetable = () => {
    const latestSemester = semesterData.value.filter(semester => semester.isActive).sort((a, b) => b.id - a.id)[0];
    return latestSemester ? latestSemester.id : semesterData.value[0].id;
  };

  const handleSemesterSelected = async (semesterId: number, classId: number) => {
    selectedSemesterId.value = semesterId;
    if (selectedSemesterId.value !== null) {
      await handleFetchTimetable(classId, semesterId);
    }
  };

  const selectSemester = (semester: ResponseGetSemesterInterface, emit: any) => {
    selectedSemester.value = semester;
    emit('semesterSelected', semester.id);
  };

  const openModal = (dayIndex: number, timeSlotId: number, subjectId: number | null = null) => {
    selectedTimeSlotId.value = timeSlotId;
    selectedDayOfWeek.value = dayIndex;
    selectedSubjectId.value = subjectId;
    isUpdateMode.value = subjectId !== null;
    showModal.value = true;
  };

  const openDeleteModal = (dayIndex: number, timeSlotId: number, subjectId: number | null = null) => {
    selectedTimeSlotId.value = timeSlotId;
    selectedDayOfWeek.value = dayIndex;
    selectedSubjectId.value = subjectId;
    showDeleteModal.value = true;
  };

  const handleDeleteTimetableConfirm = async (emit: any, semesterId: number) => {
    const data: RequestCreateAndUpdateTimetableInterface = {
      classId: classId.value,
      semesterId: semesterId,
      timeSlotId: selectedTimeSlotId.value ?? 0,
      dayOfWeek: selectedDayOfWeek.value ?? 0,
      subjectId: selectedSubjectId.value ?? 0,
    };
    try {
      await handleDeleteTimetable(data);
      showDeleteModal.value = false;
      await handleFetchTimetable(classId.value, semesterId);
      emit('update:timetables', timetableData.value);
    } catch (error) {
      console.error('Error handling delete modal:', error);
    }
  };

  const updateTimetables = (newTimetables: any) => {
    timetableData.value = newTimetables;
  };

  const handleModalConfirm = async (emit: any, semesterId: number) => {
    const data: RequestCreateAndUpdateTimetableInterface = {
      classId: classId.value,
      semesterId: semesterId,
      timeSlotId: selectedTimeSlotId.value ?? 0,
      dayOfWeek: selectedDayOfWeek.value ?? 0,
      subjectId: selectedSubjectId.value ?? 0,
    };

    try {
      if (isUpdateMode.value) {
        await handleUpdateTimetable(data);
      } else {
        await handleCreateTimetable(data);
      }
      showModal.value = false;
      await handleFetchTimetable(classId.value, semesterId);
      emit('update:timetables', timetableData.value);
    } catch (error) {
      console.error('Error handling modal confirm:', error);
    }
  }

  const getSubject = (timeSlotId: number, dayOfWeek: number) => {
    return timetableData.value.find(t => t.timeSlotId === timeSlotId && t.dayOfWeek === dayOfWeek) || null;
  };

  const gotoClassDetail = () => {
    router.push({ path: '/teacher/class-detail', query: { classId: classId.value } });
  };

  return {
    classId,
    timetableData,
    timeSlotData,
    subjectData,
    semesterData,
    selectedSemesterId,
    selectedSemester,
    showModal,
    showDeleteModal,
    isUpdateMode,
    selectedTimeSlotId,
    selectedDayOfWeek,
    selectedSubjectId,
    handleFetchTimetable,
    handleFetchTimeSlots,
    handleFetchSubjects,
    handleFetchSemester,
    handleCreateTimetable,
    handleDeleteTimetable,
    handleUpdateTimetable,
    findLatestSemesterWithTimetable,
    updateTimetables,
    handleSemesterSelected,
    selectSemester,
    openModal,
    openDeleteModal,
    handleModalConfirm,
    handleDeleteTimetableConfirm,
    getSubject,
    gotoClassDetail,
  };
};
