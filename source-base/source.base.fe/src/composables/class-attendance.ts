import { reactive, ref } from 'vue';
import { useAttendanceStore } from '@/stores/attendance';
import { RequestGetListAttendance, RequestGetAttendanceRecord, ResponseGetListAttendance, ResponseGetAttendanceRecord, ErrorResponseAttendance, PupilAttendance, } from '@/types/model/class-attendance';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import { endLoading, startLoading } from '@/helpers/mixins';
import { useRoute, useRouter } from 'vue-router';
import { useSemesterStore } from '@/stores/semester';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';


export function useAttendanceComposable() {
  const attendanceStore = useAttendanceStore();
  const { currentSemester, fetchCurrentSemester, semester, fetchSemester } = useSemesterStore();
  const attendanceList = ref<ResponseGetListAttendance[]>([]);
  const attendanceRecords = ref<ResponseGetAttendanceRecord[]>([]);
  const router = useRouter();
  const route = useRoute();
  const classId = ref<number>(parseInt(route.query.classId as string));
  const clubId = ref<number>(parseInt(route.query.clubId as string));
  const busId = ref<number>(parseInt(route.query.busId as string));
  const isShowModal = ref<boolean>(false);
  const isUpdate = ref<boolean>(false);
  const pupilAttendance = ref<PupilAttendance>();
  const errorResponseAttendance = reactive<ErrorResponseAttendance>({
    pupilId: [],
    classId: [],
    clubId: [],
    busId: [],
    isAttend: [],
    attendanceSession: [],
    attendanceType: [],
    feedback: [],
    createdDate: [],
  });

 

  const errorResponseAttendanceKeys:
   (keyof ErrorResponseAttendance)[]
  = [ 'pupilId', 'classId', 'clubId', 'busId', 'isAttend', 'attendanceSession', 'attendanceType', 'feedback', 'createdDate'];

  const fetchListAttendance = async (params: RequestGetListAttendance) => {
    startLoading();
    try {
      await attendanceStore.getListAttendance(
        params,
        (res: SuccessResponse) => {
          attendanceList.value = res.data as ResponseGetListAttendance[];
        },
        (err: ErrorResponse) => {
          attendanceList.value = [];
        }
      );
    } finally {
      endLoading();
    }
  };

  const fetchAttendanceRecord = async (params: RequestGetAttendanceRecord) => {
    startLoading();
    try {
      await attendanceStore.getAttendanceRecord(
        params,
        (res: SuccessResponse) => {
          attendanceRecords.value = res.data as ResponseGetAttendanceRecord[];
        },
        (err: ErrorResponse) => {
          attendanceRecords.value = [];
          notifyError(err.message);
        }
      );
    } finally {
      endLoading();
    }
  };

  const prepareCreateAttendance = async (params: RequestGetListAttendance) => {
    startLoading();
    try {
      await attendanceStore.prepareCreateAttendance(
        params,
        (res: SuccessResponse) => {
          attendanceRecords.value = res.data as ResponseGetAttendanceRecord[];
        },
        (err: ErrorResponse) => {
          notifyError(err.message);
        }
      );
    } finally {
      endLoading();
    }
  };

  const createAttendance = async (data: ResponseGetAttendanceRecord[],emit :any) => {
    startLoading();
    try {
      await attendanceStore.createAttendance(
        data,
        (res: SuccessResponse) => {
          notifySuccess(res.message);
          clearErrorKeys(errorResponseAttendanceKeys, errorResponseAttendance);
          emit("update:showModal", false);
          emit("refreshList");
        },
        (err: ErrorResponse) => {
          handleErrors(err);
        }
      );
    } finally {
      endLoading();
    }
  }

  const handleErrors = (err: ErrorResponse) => {
    const errors = err.errors as ErrorResponseAttendance;
    mapErrorKeys(errorResponseAttendanceKeys,  errorResponseAttendance, errors);
  }

  const handleCloseModal = (emit : any) => {
    clearErrorKeys(errorResponseAttendanceKeys, errorResponseAttendance);
    emit("update:showModal", false);
  };

  const updateAttendance = async (data: ResponseGetAttendanceRecord[],emit:any) => {
    startLoading();
    try {
      await attendanceStore.updateAttendance(
        data,
        (res: SuccessResponse) => {
          notifySuccess(res.message);
          clearErrorKeys(errorResponseAttendanceKeys, errorResponseAttendance);
          emit("update:showModal", false);
          emit("refreshList");
        },
        (err: ErrorResponse) => {
          handleErrors(err);
        }
      );
    } finally {
      endLoading();
    }
  }

  const fetchPullAttendance = async (params: {semesterId: number, date : Date}) => {
    startLoading();
    try {
      await attendanceStore.getPupilAttendance(
        params,
        (res: SuccessResponse) => {
          pupilAttendance.value = res.data as PupilAttendance;
        },
        (err: ErrorResponse) => {
          pupilAttendance.value = undefined;
        }
      );
    } finally {
      endLoading();
    }
  }
  return {
    semester,
    fetchSemester,
    fetchPullAttendance,
    pupilAttendance,
    errorResponseAttendance,
    updateAttendance,
    createAttendance,
    clubId,
    busId,
    isShowModal,
    isUpdate,
    classId,
    currentSemester,
    attendanceList,
    attendanceRecords,
    fetchCurrentSemester,
    fetchListAttendance,
    fetchAttendanceRecord,
    prepareCreateAttendance,
    handleCloseModal
  };
}