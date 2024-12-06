import { useTeacherStore } from '@/stores/teacher-profile';
import { ref, reactive, computed } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import { RequestUpdateDataTeacherInterface, ErrorResponseUpdateTeacher } from '@/types/model/teacher';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';

export const useTeacherComposable = () => {
  const teacherStore = useTeacherStore();
  const isEditMode = ref(false);
  const { updateTeacher, getTeacherProfile } = teacherStore;
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const requestDataUpdateTeacher = ref<RequestUpdateDataTeacherInterface>({
    id: 0,
    firstName: '',
    lastName: '',
    gender: true,
    address: '',
    dateOfBirth: formattedDateOfBirth,
    email: '',
    phoneNumber: '',
    image: new Blob(),
    listSubject: {
      name: '',
    },
  });
  const apiUrl = import.meta.env.VITE_APP_API_URL;

  const handleFetchTeacher = async () => {
    startLoading();
    await getTeacherProfile(
      (res: RequestUpdateDataTeacherInterface) => {
        if (res) {
          requestDataUpdateTeacher.value = res;
          requestDataUpdateTeacher.value.dateOfBirth = formatDate(res.dateOfBirth);
          requestDataUpdateTeacher.value.id = res.id;
        } else {
          throw new Error('Không tìm thấy hồ sơ giáo viên');
        }
      },
      (err: any) => {
        notifyError('Lấy hồ sơ giáo viên thất bại');
      },
    );
    endLoading();
  };

  const handleUpdateTeacher = async (data: RequestUpdateDataTeacherInterface) => {
    startLoading();
    await updateTeacher(
      data,
      (res: any) => {
        notifySuccess('Cập nhật giáo viên thành công');
        clearErrorKeys(errorUpdateKeys, errorsUpdateTeacher);
        handleFetchTeacher();
        isEditMode.value = false;
      },
      (err: any) => {
        handleErrors(err);
        notifyError('Cập nhật giáo viên thất bại');
      },
    );
    endLoading();
  };

  const enableEditMode = () => {
    isEditMode.value = true;
  };

  const disableEditMode = () => {
    handleFetchTeacher();
    clearErrorKeys(errorUpdateKeys, errorsUpdateTeacher);
    isEditMode.value = false;
  };

  const formatDate = (date: string) => {
    const d = new Date(date);
    const month = `0${d.getMonth() + 1}`.slice(-2);
    const day = `0${d.getDate()}`.slice(-2);
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
  };

  const errorsUpdateTeacher = reactive(<ErrorResponseUpdateTeacher>{
    FirstName: [],
    LastName: [],
    PhoneNumber: [],
    Email: [],
    Image: [],
    DateOfBirth: [],
    Address: [],
  });

  const errorUpdateKeys: (keyof ErrorResponseUpdateTeacher)[] = [
    'FirstName',
    'LastName',
    'PhoneNumber',
    'Email',
    'Image',
    'DateOfBirth',
    'Address',
  ];

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseUpdateTeacher;
    mapErrorKeys(errorUpdateKeys, errorsUpdateTeacher, errorsResponse);
  };

  const imageFile = ref<{ name: string; url: string } | null>(null);
  const imageUrl = computed(() => {
    return imageFile.value ? imageFile.value.url : `${apiUrl}${requestDataUpdateTeacher.value.image}`;
  });

  const handleFileChange = (event: Event) => {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      requestDataUpdateTeacher.value.image = file;
      imageFile.value = { name: file.name, url: URL.createObjectURL(file) };
    }
  };
  return {
    handleFetchTeacher,
    imageUrl,
    enableEditMode,
    handleFileChange,
    disableEditMode,
    apiUrl,
    isEditMode,
    errorsUpdateTeacher,
    requestDataUpdateTeacher,
    handleUpdateTeacher,
  };
};
