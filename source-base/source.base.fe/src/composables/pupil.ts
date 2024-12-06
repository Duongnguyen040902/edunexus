import { usePupilStore } from '@/stores/pupil-profile';
import { ref, reactive, computed } from 'vue';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifySuccess, notifyError } from '@/helpers/notify';
import {
  RequestUpdateDataPupilInterface,
  ErrorResponseUpdatePupil,
} from '@/types/model/pupil';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';
import axios from "axios";
export const usePupilComposable = () => {
  const pupilStore = usePupilStore();
  const isEditMode = ref(false);
  const { updatePupil, getPupilProfile } = pupilStore;
  const formattedDateOfBirth = new Date().toISOString().split('T')[0];
  const requestDataUpdatePupil = ref<RequestUpdateDataPupilInterface>({
    id: 0,
    firstName: '',
    lastName: '',
    gender: true,
    donorName: '',
    donorPhoneNumber: '',
    address: '',
    dateOfBirth: formattedDateOfBirth,
    email: '',
    image: new Blob(),
  });
  const apiUrl = import.meta.env.VITE_APP_API_URL;

  const handleFetchPupil = async () => {
    startLoading();
    await getPupilProfile(
      (res: RequestUpdateDataPupilInterface) => {
        if (res) {
          requestDataUpdatePupil.value = res;
          requestDataUpdatePupil.value.dateOfBirth = formatDate(res.dateOfBirth);
          requestDataUpdatePupil.value.id = res.id;
        } else {
          throw new Error('Không tìm thấy hồ sơ học sinh');
        }
      },
      (err: any) => {
        notifyError('Không tìm thấy hồ sơ học sinh');
      },
    );
    endLoading();
  };

  const handleUpdatePupil = async (data: RequestUpdateDataPupilInterface) => {
    startLoading();
    await updatePupil(
      data,
      (res: any) => {
        notifySuccess('Cập nhật học sinh thành công');
        clearErrorKeys(errorUpdateKeys, errorsUpdatePupil);
        handleFetchPupil();
        isEditMode.value = false;
      },
      (err: any) => {
        handleErrors(err);
        notifyError('Cập nhật học sinh thất bại');
      },
    );
    endLoading();
  };

  const handleSave = async (data: RequestUpdateDataPupilInterface) => {
    await handleUpdatePupil(data);
  };

  const enableEditMode = () => {
    isEditMode.value = true;
  };

  const disableEditMode = () => {
    handleFetchPupil();
    clearErrorKeys(errorUpdateKeys, errorsUpdatePupil);
    isEditMode.value = false;
  };

  const formatDate = (date: string) => {
    const d = new Date(date);
    const month = `0${d.getMonth() + 1}`.slice(-2);
    const day = `0${d.getDate()}`.slice(-2);
    const year = d.getFullYear();
    return `${year}-${month}-${day}`;
  };

  const errorsUpdatePupil = reactive<ErrorResponseUpdatePupil>({
    FirstName: [],
    LastName: [],
    Email: [],
    DonorName: [],
    DonorPhoneNumber: [],
    Image: [],
    DateOfBirth: [],
    Address: [],
  });

  const errorUpdateKeys: (keyof ErrorResponseUpdatePupil)[] = [
    'FirstName',
    'LastName',
    'Email',
    'DonorName',
    'DonorPhoneNumber',
    'Image',
    'DateOfBirth',
    'Address',
  ];

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseUpdatePupil;
    mapErrorKeys(errorUpdateKeys, errorsUpdatePupil, errorsResponse);
  };

  const imageFile = ref<{ name: string; url: string } | null>(null);
  const imageUrl = computed(() => {
    return imageFile.value
      ? imageFile.value.url
      : `${apiUrl}${requestDataUpdatePupil.value.image}`;
  });

  const handleFileChange = (event: Event) => {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      requestDataUpdatePupil.value.image = file;
      imageFile.value = { name: file.name, url: URL.createObjectURL(file) };
    }
};


// const imageFile = ref<{ name: string; url: string } | null>(null);
// const imageUrl = ref<string | null>(null);

// const fetchImageWithAuth = async (url: string, token: string | null) => {
//   if (!url || typeof url !== 'string') {
//     console.error('URL không hợp lệ:', url);
//     return null;
//   }

//   if (!token) {
//     console.error('Token không tồn tại!');
//     return null;
//   }

//   try {
//     const response = await axios.get(url, {
//       responseType: 'blob', // Tải dữ liệu dạng Blob (image)
//       headers: {
//         Authorization: `Bearer ${token}`, // Truyền token
//       },
//     });
//     return URL.createObjectURL(response.data); // Trả về URL Blob
//   } catch (err) {
//     console.error('Lỗi khi gọi API:', err);
//     return null;
//   }
// };



// const loadImage = async () => {
//   if (imageFile.value) {
//     imageUrl.value = imageFile.value.url;
//   } else {
//     const url = `${apiUrl}${requestDataUpdatePupil.value.image}`;
//     console.log('url', url);
//     const token = localStorage.getItem("accessToken"); // Lấy token từ localStorage
//     console.log('token', token);
//     imageUrl.value = await fetchImageWithAuth('http://localhost:5000/Resources/PupilImage/56f27494-361a-4c77-908f-c6901e035a10_BT.png', token); // Gọi hàm tải ảnh
//   }
// };

// const handleFileChange = (event: Event) => {
//   const file = (event.target as HTMLInputElement).files?.[0];
//   if (file) {
//     requestDataUpdatePupil.value.image = file;
//     imageFile.value = { name: file.name, url: URL.createObjectURL(file) };
//     imageUrl.value = imageFile.value.url; // Cập nhật đường dẫn ảnh
//   }
// };

  

  return {
    handleFetchPupil,
    handleSave,
    imageUrl,
    enableEditMode,
    handleFileChange,
    disableEditMode,
    apiUrl,
    isEditMode,
    errorsUpdatePupil,
    requestDataUpdatePupil,
    handleUpdatePupil,
  };
};