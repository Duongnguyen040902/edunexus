import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { SemesterRepository } from '@/repositories/repository-semester';
import { RequestGetSemesterInterface,RequestListSemesterInterface, ResponseGetSemesterInterface,ListSemesterResponseInterface, RequestCreateAndUpdateSemester,ResponseSemesterDetail,
  ErrorResponseSemester,
} from '@/types/model/semester';
import { de, pa } from 'element-plus/es/locale';
import { defineStore } from 'pinia';
import { reactive,ref } from 'vue';
import { useRoute } from 'vue-router';
import { mapErrorKeys, clearErrorKeys } from '@/helpers/state';
export const useSemesterStore = defineStore('semester', () => {
  const semesterFactory = RepositoryFactory.create('semester') as SemesterRepository;
  const currentSemester = reactive<ResponseGetSemesterInterface>({
    id: 0,
    semesterName: '',
    semesterCode: '',
    startDate: '',
    endDate: '',
    isActive: false,
    schoolYearId: 0,
    schoolYearName: '',
  });

  const listSemester= reactive<{ value: ListSemesterResponseInterface | null }>({ value: null });
  const semester = reactive<{ value: ResponseGetSemesterInterface[] }>({
    value: [],
  });

  const fetchSemester = async (
    success: (res: ResponseGetSemesterInterface[]) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await semesterFactory.getSemester(
      res => {
        semester.value = res.data as ResponseGetSemesterInterface[];
        return success(semester.value);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };
  const fetchCurrentSemester = async (
    success: (res: SuccessResponse) => void = () => {},
    error: (err: ErrorResponse) => void = () => {},
  ) => {
    await semesterFactory.getCurrentSemester(
      res => {
        const response = res.data as ResponseGetSemesterInterface;
        currentSemester.id = response.id;
        currentSemester.semesterName = response.semesterName;
        currentSemester.semesterCode = response.semesterCode;
        currentSemester.startDate = response.startDate;
        currentSemester.endDate = response.endDate;
        currentSemester.isActive = response.isActive;
        currentSemester.schoolYearId = response.schoolYearId;
        currentSemester.schoolYearName = response.schoolYearName;
        return success(res);
      },
      err => {
        console.log('error', err);
        error(err);
      },
    );
  };


  
  const getListSemeter= async (
    params: RequestListSemesterInterface,
    success: (res: ListSemesterResponseInterface) => void,
    error: (err: ErrorResponse) => void
) => {
    await semesterFactory.getListSemester(
        params,
        res => {
            if (res.data) {  
                listSemester.value = res.data; 
                return success(listSemester.value); 
            }   
        },
        (err: ErrorResponse) => {
            console.error('Error fetching teacher assignments:', err); 
        }
    );
};
  
//Tân
const route = useRoute();
const schoolYearId = ref<number>(parseInt(route.query.schoolYearId as string) );
const dataSemester = reactive<{ value: ResponseGetSemesterInterface[] }>({
  value: [],
});
const requestSemesterUpdate = reactive<RequestCreateAndUpdateSemester>({
  id: 0,
  semesterName: '',
  semesterCode: '',
  startDate: '',
  endDate: '',
  isActive: false,
  schoolYearId: schoolYearId.value,
});

const semesterDetail = ref<ResponseSemesterDetail>({
  id: 0,
  semesterName: '',
  semesterCode: '',
  startDate: '',
  endDate: '',
  isActive: false,
  schoolYearId: schoolYearId.value,
});

const errorSemester = reactive<ErrorResponseSemester>({
  SemesterName: [],
  SemesterCode: [],
  StartDate: [],
  EndDate: [],
});
const errorSemesterKeys: (keyof ErrorResponseSemester)[] = ['SemesterName', 'SemesterCode', 'StartDate', 'EndDate'];

const getSemesterBySchoolYear = async () => {
  await semesterFactory.getSemestersBySchoolYearId(
    { schoolYearId: schoolYearId.value },
    (res) => {
      dataSemester.value = res.data as ResponseGetSemesterInterface[];
    },
    (err: ErrorResponse) => {
      dataSemester.value = [];
    },
  );
};

  const getSemesterDetail = async (id: number) => {
    await semesterFactory.getSemesterDetail(id,
      (res: SuccessResponse) => {
        semesterDetail.value = res.data as ResponseSemesterDetail;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const createSemester = async (success: (res: SuccessResponse) => void,
  error: (err: ErrorResponse) => void) => {
    // // +1 day vì lúc gửi lên server nó sẽ bị trừ 1 ngày(do sử dụng el-date-picker chứ ko dùng input)
    // const startDate = new Date(requestSemesterUpdate.startDate);
    // startDate.setDate(startDate.getDate() + 1);
    // requestSemesterUpdate.startDate = startDate.toISOString();
    // const endDate = new Date(requestSemesterUpdate.endDate);
    // endDate.setDate(endDate.getDate() + 1);
    // requestSemesterUpdate.endDate = endDate.toISOString();
    // requestSemesterUpdate.startDate = new Date(requestSemesterUpdate.startDate +1).toISOString();
    // requestSemesterUpdate.endDate = new Date(requestSemesterUpdate.endDate +1).toISOString();
    await semesterFactory.createSemester(requestSemesterUpdate,
      (res: SuccessResponse) => {
        clearErrorKeys(errorSemesterKeys, errorSemester);
        return success(res);
      },
      (err: ErrorResponse) => {
        mapErrorKeys(errorSemesterKeys,errorSemester, err.errors as ErrorResponseSemester);
        error(err);
      },
    );
  };

  const updateSemester = async (success: (res: SuccessResponse) => void,
  error: (err: ErrorResponse) => void) => {
    await semesterFactory.updateSemester(requestSemesterUpdate,
      (res: SuccessResponse) => {
        clearErrorKeys(errorSemesterKeys, errorSemester);
        return success(res);
      },
      (err: ErrorResponse) => {
        mapErrorKeys(errorSemesterKeys,errorSemester, err.errors as ErrorResponseSemester);
        error(err);
      },
    );
  }

  const deleteSemester = async (id: number,success: (res: SuccessResponse) => void,
  error: (err: ErrorResponse) => void) => {
    await semesterFactory.deleteSemester(id,
      (res: SuccessResponse) => {
        return success(res);
      },
      (err: ErrorResponse) => {
        error(err);
      },
    );
  };

  return {
    semester,
    currentSemester,
    schoolYearId,
    fetchSemester,
    fetchCurrentSemester,
    getSemesterBySchoolYear,
    dataSemester,
    requestSemesterUpdate,
    semesterDetail,
    errorSemester,
    errorSemesterKeys,
    getSemesterDetail,
    createSemester,
    updateSemester,
    deleteSemester,
  };
});