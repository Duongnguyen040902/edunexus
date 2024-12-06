import { ResponseGetSemesterInterface } from '@/types/model/semester';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { mapErrorKeys } from '@/helpers/state';
import { useClassStore } from '@/stores/class';
import {
  PupilClassesInterface,
  ResponseGetAssignedClassInterface,
  ResponseGetClassDetailInterface,
} from '@/types/model/class';
import { reactive, ref } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useSemesterStore } from '@/stores/semester';
import { ROUTER_PATHS } from '@/constants/api/router-paths';

export const useClassDetailComposable = () => {
  const classStore = useClassStore();
  const { currentSemester, fetchCurrentSemester } = useSemesterStore();
  const classDetail = ref<ResponseGetClassDetailInterface[]>([]);
  const assignedClass = reactive<ResponseGetAssignedClassInterface>({
    Id: 0,
    ClassName: '',
    SchoolId: 0,
    SchoolName: '',
    Status: 0,
  });

  const router = useRouter();
  const route = useRoute();
  const classId = ref<number>(parseInt(route.query.classId as string));
  const pupilClasses = ref<PupilClassesInterface[]>([]);
  const fetchClassDetail = async (classId: number, semesterId: number) => {
    try {
      await classStore.getClassDetail(
        { classId, semesterId },
        res => {
          classDetail.value = res;
        },
        err => {
          console.error('Error fetching class detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching class detail:', error);
    }
  };

  const fetchAssignedClass = async () => {
    try {
      await classStore.getAssignedClass(
        res => {
          // Gán toàn bộ đối tượng res vào đối tượng reactive assignedClass
          Object.assign(assignedClass, {
            Id: res.id,
            ClassName: res.className,
            SchoolId: res.schoolId,
            SchoolName: res.schoolName,
            Status: res.status,
          });
        },
        err => {
          Object.assign(assignedClass, null);
          console.error('Error fetching assigned class:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching assigned class:', error);
    }
  };

  const fetchEnrolledClass = async () => {
    try {
      await classStore.getEnrollmentClass(
        res => {
          Object.assign(assignedClass, {
            Id: res.id,
            ClassName: res.className,
            SchoolId: res.schoolId,
            SchoolName: res.schoolName,
            Status: res.status,
          });
        },
        err => {
          console.error('Error fetching enrolled class:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching enrolled class:', error);
    }
  };

  const fetchPupilClasses = async () => {

    try {
      await classStore.getPupilClasses(
        res => {
          pupilClasses.value = res.data as PupilClassesInterface[] || [];
        },
        err => {
          pupilClasses.value = [];
          // notifyError(err.message);
        },
      );
    } catch (error) {
      pupilClasses.value = [];
      console.error('Error fetching pupil classes:', error);
    }
  };

  const fetchPupilClassDetail = async (classId: number, semesterId: number) => {
    try {
      await classStore.getPupilClassDetail(
        { classId, semesterId },
        res => {
          classDetail.value = res;
        },
        err => {
          console.error('Error fetching class detail:', err);
        },
      );
    } catch (error) {
      console.error('Error fetching class detail:', error);
    }
  };



  const goToTimeTable = (classId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.TIME_TABLE, query: { classId } });
  };

  const goToTimeTableOfPupil = (classId: number) => {
    router.push({ path: ROUTER_PATHS.PUPIL.TIMETABLE, query: { classId } });
  };

  const goToClassDetail = (classId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.CLASS_DETAIL, query: { classId } });
  };

  const goToClassApplication = (classId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.CLASS_APPLICATION, query: { classId } });
  };

  const goToNotification = (classId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.NOTIFICATIONS_MANAGEMENT, query: { classId } });
  };

  const goToClassDetailOfPupil = (classId: number) => {
    router.push({ path: ROUTER_PATHS.PUPIL.CLASS_DETAIL, query: { classId } });
  };

  const goToNotificationOfPupil = (classId: number) => {
    router.push({ path: ROUTER_PATHS.PUPIL.NOTIFICATIONS, query: { classId } });
  };
  const gotoFeedback = (classId: number) => {
    router.push({ path: ROUTER_PATHS.TEACHER.FEEDBACK, query: { classId } });
  };
  return {
    assignedClass,
    goToNotificationOfPupil,
    classDetail,
    classId,
    goToNotification,
    currentSemester,
    fetchClassDetail,
    fetchAssignedClass,
    goToTimeTable,
    goToClassDetail,
    fetchCurrentSemester,
    goToClassApplication,
    fetchEnrolledClass,
    goToClassDetailOfPupil,
    goToTimeTableOfPupil,
    gotoFeedback,
    fetchPupilClasses,
    pupilClasses,
    fetchPupilClassDetail,
  };
};
