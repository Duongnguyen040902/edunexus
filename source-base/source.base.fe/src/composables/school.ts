import { computed, reactive, ref } from 'vue';
import { endLoading, startLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify';
import { useSchoolStore } from '@/stores/school';
import { ErrorResponseUpdateSchoolInfo, SchoolDashboardDTO, SchoolInfoResponseInterface } from '@/types/model/school';
import { getDistrictsByProvinceCode, getProvinces, getWardsByDistrictCode } from 'vn-provinces';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state';

export const useSchoolDetailComposable = () => {
  const schoolStore = useSchoolStore();
  const apiUrl = import.meta.env.VITE_APP_API_URL;
  const imagePath = ref();
  const isEditable = ref(false);
  const provinces = ref<any[]>([]);
  const districts = ref<any[]>([]);
  const wards = ref<any[]>([]);
  const isShowModal = ref(false);
  const schoolDetail = reactive<SchoolInfoResponseInterface>({
    id: 0,
    name: '',
    province: '',
    district: '',
    ward: '',
    standardCode: '',
    address: '',
    phoneNumber: '',
    principalName: '',
    principalPhone: '',
    websiteLink: '',
    image: '',
    imageFile: null,
    dateOfEstablishment: new Date(),
    fax: '',
  });

  const dashboard = ref<SchoolDashboardDTO>({
    countActiveClass: 0,
    countActiveClub: 0,
    countActiveBusRoute: 0,
    countActiveBus: 0,
    countActivePupil: 0,
    countActiveTeacher: 0,
    countActiveSupervisor: 0,
    totalActiveAccount: 0,
    semesterName: '',
    schoolYearName: '',
  });

  const errorsUpdateSchool = reactive<ErrorResponseUpdateSchoolInfo>({
    Name: [],
    Address: [],
    PhoneNumber: [],
    PrincipalName: [],
    PrincipalPhone: [],
    WebsiteLink: [],
    ImageFile: [],
    StandardCode: [],
    DateOfEstablishment: [],
    FAX: [],
  });

  const errorUpdateKeys: (keyof ErrorResponseUpdateSchoolInfo)[] = [
    'Name',
    'Address',
    'PhoneNumber',
    'PrincipalName',
    'PrincipalPhone',
    'WebsiteLink',
    'ImageFile',
    'StandardCode',
    'DateOfEstablishment',
    'FAX',
  ];

  const toggleEditMode = () => {
    isEditable.value = !isEditable.value;
  };

  const fetchProvinces = () => {
    provinces.value = getProvinces();
  };

  const fetchDistricts = (selectedCity: number) => {
    districts.value = getDistrictsByProvinceCode(selectedCity.toString());
  };

  const fetchWards = (selectedDistrict: number) => {
    wards.value = getWardsByDistrictCode(selectedDistrict.toString());
  };

  const fetchSchoolDetail = async () => {
    startLoading();
    try {
      await schoolStore.getSchoolInfo(
        [],
        res => {
          Object.assign(schoolDetail, res);
          imageUrl.value = imageFile.value?.url || `${apiUrl}${schoolDetail.image}`;
        },
        err => {
          console.error('Lấy thông tin trường học thất bại:', err);
        },
      );
    } catch (error) {
      console.error('Xảy ra lỗi:', error);
    } finally {
      endLoading();
    }
  };

  const fetchUpdateSchool = async (school: SchoolInfoResponseInterface): Promise<boolean> => {
    startLoading();
    try {
      await schoolStore.updateSchoolInfo(
        school,
        () => {
          notifySuccess('Cập nhật thông tin nhà trường thành công');
          clearErrorKeys(errorUpdateKeys, errorsUpdateSchool);
        },
        err => {
          handleErrors(err);
          console.error('Error updating school detail:', err);
          throw new Error('Update failed');
        },
      );
      return true;
    } catch (error) {
      return false;
    } finally {
      endLoading();
    }
  };

  const saveChanges = async () => {
    try {
      schoolDetail.imageFile;
      if (!districts.value.find(district => district.code === schoolDetail.district)) {
        alert('Bạn chưa chọn lại Quận/Huyện và Xã/Phường');
        return;
      }
      if (!wards.value.find(ward => ward.code === schoolDetail.ward)) {
        alert('Bạn chưa chọn lại Xã/Phường');
        return;
      }
      const check = await fetchUpdateSchool(schoolDetail);
      if (check) {
        isShowModal.value = false;
        await fetchSchoolDetail();
      }
    } catch (error) {
      console.error('Error saving changes:', error);
    }
  };

  const notSave = async () => {
    if (confirm('Thông tin thay đổi sẽ không được ghi nhận! Bạn có chắc chắn không?')) {
      isShowModal.value = false;
      clearErrorKeys(errorUpdateKeys, errorsUpdateSchool);
      await fetchSchoolDetail();
      fetchData();
    }
    return;
  };

  const handleErrors = (err: any) => {
    endLoading();
    const errorsResponse = err.errors as ErrorResponseUpdateSchoolInfo;
    mapErrorKeys(errorUpdateKeys, errorsUpdateSchool, errorsResponse);
  };

  const onProvinceChange = async () => {
    if (schoolDetail?.province) {
      districts.value = [];
      wards.value = [];
      await fetchDistricts(schoolDetail.province);
    } else {
      console.error('Thành phố không được để trống.');
    }
  };

  const onDistrictChange = async () => {
    if (schoolDetail?.district) {
      wards.value = [];
      await fetchWards(schoolDetail.district);
    } else {
      console.error('Thông tin cập nhật không đúng!');
    }
  };

  const fetchData = async () => {
    await fetchProvinces();
    if (schoolDetail.province) {
      await fetchDistricts(schoolDetail.province);
    }
    if (schoolDetail.district) {
      await fetchWards(schoolDetail.district);
    }
  };
  const isShow = async () => {
    isShowModal.value = true;
  };

  const imageFile = ref<{ name: string; url: string } | null>(null);
  const imageUrl = ref('');
  const handleFileChange = event => {
    const file = event.target.files[0];
    if (file) {
      if (imageFile.value) {
        URL.revokeObjectURL(imageFile.value.url);
      }
      schoolDetail.imageFile = file;
      imageFile.value = { name: file.name, url: URL.createObjectURL(file) };
      imageUrl.value = imageFile.value.url;
    }
  };

  const fetchDashboard = async () => {
    startLoading();
    try {
      await schoolStore.getDashboard(
        res => {
          dashboard.value = res.data as SchoolDashboardDTO;
        },
        err => {
          console.error('Lấy thông tin trường học thất bại:', err);
        },
      );
    } catch (error) {
      console.error('Xảy ra lỗi:', error);
    } finally {
      endLoading();
    }
  };

  return {
    isShowModal,
    schoolDetail,
    imageUrl,
    imageFile,
    imagePath,
    apiUrl,
    errorsUpdateSchool,
    provinces,
    districts,
    wards,
    isEditable,
    isShow,
    onProvinceChange,
    onDistrictChange,
    notSave,
    saveChanges,
    fetchSchoolDetail,
    fetchData,
    handleFileChange,
    toggleEditMode,
    fetchProvinces,
    fetchDistricts,
    fetchWards,
    fetchUpdateSchool,
    fetchDashboard,
    dashboard,
  };
};
