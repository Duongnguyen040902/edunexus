import { useSchoolYearStore } from "@/stores/school-year";
import { startLoading, endLoading } from '@/helpers/mixins';
import router from '@/router';
import { ref } from 'vue';
import { notifyError, notifySuccess } from '@/helpers/notify.ts';
import { clearErrorKeys } from '@/helpers/state';
import { ResponseSchoolYearDetail } from '@/types/school-year'; // Adjust the import path as necessary
import { ROUTER_PATHS } from '@/constants/api/router-paths';

export const useSchoolYearComposable = () => {
    const schoolYearStore = useSchoolYearStore();
    const {
        errorSchoolYear,
        errorSchoolYearKeys,
        requestSchoolYearIndex,
        dataSchoolYear,
        requestSchoolYearUpdate,
        schoolYearDetail,
        deleteSchoolYear,
        getSchoolYearIndex,
        getSchoolYearDetail,
        createSchoolYear,
        updateSchoolYear,
    } = schoolYearStore;
    const isShowModalEdit = ref(false);
    const isCreateSchoolYear = ref(false);
    const isShowModalDelete = ref(false);
    const handleLoading = async (action: () => Promise<void>) => {
        startLoading();
        await action();
        endLoading();
    };

    const formatDate = (date: string | Date) => {
        const d = new Date(date);
        const month = `${d.getMonth() + 1}`.padStart(2, '0');
        const day = `${d.getDate()}`.padStart(2, '0');
        const year = d.getFullYear();
        return `${year}-${month}-${day}`;
    };

    const updateRequesSchoolYearUpdate = (schoolYearDetail: any) => {
        requestSchoolYearUpdate.id = schoolYearDetail.id;
        requestSchoolYearUpdate.name = schoolYearDetail.name;
        requestSchoolYearUpdate.startDate = formatDate(schoolYearDetail.startDate);
        requestSchoolYearUpdate.endDate = formatDate(schoolYearDetail.endDate);
        requestSchoolYearUpdate.isActive = schoolYearDetail.isActive;
    };

    const handleGetSchoolYearIndex = async () => {
        await handleLoading(getSchoolYearIndex);
    };

    const handleGetSchoolYearDetail = async (id: number) => {
        await handleLoading(() => getSchoolYearDetail(id, () => {}, () => {}));
    };

    const handlePageChange = (page: number) => {
        requestSchoolYearIndex.pageNumber = page;
        handleGetSchoolYearIndex();
    };

    const handleClearState = () => {
        requestSchoolYearUpdate.id = 0;
        requestSchoolYearUpdate.name = '';
        requestSchoolYearUpdate.startDate = formatDate(new Date());
        requestSchoolYearUpdate.endDate = formatDate(new Date());
        requestSchoolYearUpdate.isActive = false;
    };

    const handleOpenModalEdit = (schoolYear: ResponseSchoolYearDetail) => {
        updateRequesSchoolYearUpdate(schoolYear);
        isShowModalEdit.value = true;
        isCreateSchoolYear.value = false;
    };

    const handleOpenModalCreate = () => {
        handleClearState();
        isShowModalEdit.value = true;
        isCreateSchoolYear.value = true;
    };

    const handleCloseModalEdit = () => {
        isShowModalEdit.value = false;
        isCreateSchoolYear.value = false;
        handleClearState();
        clearErrorKeys(errorSchoolYearKeys, errorSchoolYear);
    };

    const handleConfirmEdit = async () => {
        await updateSchoolYear(
            async () => {
                notifySuccess('Cập nhật thông tin năm học thành công');
                await handleGetSchoolYearIndex();
                handleClearState();
                handleCloseModalEdit();
            },
            (err:any) => {
                notifyError('Cập nhật thông tin năm học thất bại');
                // Handle error response and update errorSchoolYear
                if (err.errors) {
                    Object.keys(err.errors).forEach(key => {
                        errorSchoolYear[key] = err.errors[key];
                    });
                }
            },
        );
    };

    const handleConfirmCreate = async () => {
        await createSchoolYear(
            async () => {
                notifySuccess('Tạo thông tin năm học thành công');
                await handleGetSchoolYearIndex();
                handleClearState();
                handleCloseModalEdit();
            },
            (err:any) => {
                notifyError('Tạo thông tin năm học thất bại');
                // Handle error response and update errorSchoolYear
                if (err.errors) {
                    Object.keys(err.errors).forEach(key => {
                        errorSchoolYear[key] = err.errors[key];
                    });
                }
            }
        );
    };

    const handleOpenModalDelete = () => {
        isShowModalDelete.value = true;
    };

    const handleCloseModalDelete = () => {
        isShowModalDelete.value = false;
    };

    const handleConfirmDelete = async (id: number) => {
        await deleteSchoolYear(id,
            async () => {
                isShowModalDelete.value = false;
                notifySuccess('Xóa thông tin năm học thành công');
                await handleGetSchoolYearIndex();   
            },
            (err) => {
                notifyError('Xóa thông tin năm học thất bại',);
            },
        );
    };

    const gotosemester = (id: number) => {
        router.push({
            path: ROUTER_PATHS.SCHOOL_ADMIN.SEMESTER_OF_SCHOOLYEAR,
            query: { schoolYearId: id },
        });
    }
    return {
        gotosemester,
        errorSchoolYear,
        isCreateSchoolYear,
        errorSchoolYearKeys,
        requestSchoolYearIndex,
        dataSchoolYear,
        isShowModalDelete,
        handleOpenModalCreate,
        handleConfirmCreate,
        handleOpenModalDelete,
        handleCloseModalDelete,
        handleConfirmDelete,
        formatDate,
        requestSchoolYearUpdate,
        schoolYearDetail,
        deleteSchoolYear,
        getSchoolYearIndex,
        getSchoolYearDetail,
        createSchoolYear,
        updateSchoolYear,
        handleGetSchoolYearIndex,
        handleGetSchoolYearDetail,
        handlePageChange,
        isShowModalEdit,
        handleOpenModalEdit,
        handleCloseModalEdit,
        handleConfirmEdit,
    };
};