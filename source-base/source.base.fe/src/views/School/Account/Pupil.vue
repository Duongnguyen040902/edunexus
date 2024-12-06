<template>
<div class="flex-grow-1 container-p-y">
    <div class="layout-container">
        <div class="layout-page">
            <div class="content-wrapper">
                <div class="container-xl flex-grow-1 container-p-y">
                <h4 class="fw-bold py-3 mb-4">
                    <span class="text-muted fw-light">Quản lý Tài khoản /</span> Học sinh
                </h4>
                <div class="card">
                    <SearchFiltersPupilComponent 
                    @updateSearch="(value) => updateRequestPupils('searchKey', value)" 
                    @updateStatus="(value) => updateRequestPupils('accountStatus', value)" 
                    @updateRecord="(value) => updateRequestPupils('pageSize', value)" 
                    />
                    <div class="dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-6 mb-md-0 mt-n6 mt-md-0 gap-md-4">
                        <div class="dt-buttons btn-group flex-wrap">
                            <button 
                                class="btn btn-secondary add-new btn-danger" 
                                type="button" 
                                style="margin-right: 10px" 
                                :disabled="!isEmitValue" 
                                @click="handleShowModalDelete"
                                >
                                    <span>
                                        <i class="bx bx-trash bx-sm me-md-2"></i>
                                        <span class="d-md-inline-block d-none">Xóa tất cả</span>
                                    </span>
                            </button>
                            <button class="btn btn-secondary add-new btn-primary" style="margin-right: 10px;" @click="handleOpen">
                                Thêm học sinh
                            </button>
                            <button class="btn btn-secondary add-new btn-primary" type="button" style="margin-right: 10px;" @click="openImportModal">
                                Nhập excel
                            </button>
                        </div>
                    </div>
                    <PupilTableComponent 
                    :pupilsData="listPupilResponse.data" 
                    @showDetail="handleShowPupilDetail" 
                    :apiUrl="apiUrl" 
                    :formatDate="formatDate"
                    @delete-pupils="handleDeletePupils"
                    />
                    <div class="pagination-container">
                        <PaginationComponent 
                        :total-records="listPupilResponse.totalRecords" 
                        :page-size="params.pageSize" 
                        :current-page="params.pageNumber" 
                        @update:currentPage="(value) => updateRequestPupils('pageNumber', value)" 
                        @page-changed="handlePageChange"
                        />
                    </div>
                </div>
                <CRUDPupilModal 
                :isShowCreate="isShowCreate" 
                :isCreatePupil="isCreatePupil" 
                :isUpdatePupil="isUpdatePupil" 
                :requestDataCreatePupil="requestDataCreatePupil" 
                :errorsPupil="errorsPupil" 
                :apiUrl="apiUrl" 
                @handleSave="handleSave" 
                @handleClose="handleClose()" 
                @handleEditPupil="handleEditPupil()" 
                @openEditModal="openEditModal" 
                />
                <ImportExcelModal 
                :isImportModalVisible="isImportModalVisible" 
                @handleImportExcelPupilAccount="handleImportExcelPupilAccount" 
                @closeImportModal="closeImportModal" 
                @onFileChange="onFileChange"
                />
            </div>
        </div>
    </div>
    </div>
</div>
<ModalDeleteMultipleComponent 
:isShowModal="isCheckedDelete" 
@closeModal="handleCloseModalDelete" 
@confirmAction="handleConfirmDelete" 
/>
</template>

    
<script lang="ts">
import {
    defineComponent,
    onMounted,
    ref,
} from 'vue';
import PaginationComponent from '@/components/common/Pagination.vue';
import SearchFiltersPupilComponent from '@/components/views/pupil/SearchFiltersPupil.vue';
import PupilTableComponent from '@/components/views/pupil/PupilTable.vue';
import {
    usePupilAccountComposable
} from '@/composables/pupil-account';
import CRUDPupilModal from '@/components/modals/pupil-account/CRUDPupilModal.vue';
import ModalDeleteMultipleComponent from '@/components/common/ModalConfirmDelete.vue';
import ImportExcelModal from '@/components/modals/pupil-account/ImportExcelModal.vue';
export default defineComponent({
    name: 'PupilAccount',
    props: {},
    components: {
        SearchFiltersPupilComponent,
        PupilTableComponent,
        PaginationComponent,
        CRUDPupilModal,
        ModalDeleteMultipleComponent,
        ImportExcelModal,
    },
    setup(props, {
        emit
    }) {

        onMounted(async () => {
            await handleGetListPupil();
        });

        const {
            params,
            listPupilResponse,
            errorsPupil,
            isCreatePupil,
            isUpdatePupil,
            requestDataCreatePupil,
            isShowCreate,
            apiUrl,
            openEditModal,
            handleOpen,
            handleClose,
            handleGetListPupil,
            updateRequestPupils,
            handleSave,
            handlePageChange,
            handleShowPupilDetail,
            handleEditPupil,
            formatDate,
            requestDeletePupil,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            requestImportExcelPupil,
            handleImportExcelPupilAccount,
            isImportModalVisible,
            onFileChange,
            closeImportModal,
            openImportModal,
        } = usePupilAccountComposable();

        const isEmitValue = ref(false);
        const handleDeletePupils = async (ids: number[]) => {
            if (ids.length === 0) {
                isEmitValue.value = false;
                return;
            }
            isEmitValue.value = true;
            requestDeletePupil.ids = ids;
        };
        
        return {
            params,
            listPupilResponse,
            isShowCreate,
            requestDataCreatePupil,
            isCreatePupil,
            isUpdatePupil,
            apiUrl,
            errorsPupil,
            handleOpen,
            handleSave,
            handleClose,
            handleShowPupilDetail,
            openEditModal,
            handleEditPupil,
            updateRequestPupils,
            formatDate,
            requestDeletePupil,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            isEmitValue,
            handleDeletePupils,
            handlePageChange,
            requestImportExcelPupil,
            handleImportExcelPupilAccount,
            isImportModalVisible,
            onFileChange,
            closeImportModal,
            openImportModal,
        }
    }
})
</script>

    
<style scoped>
.form-content {
    padding: 10px;
}

.form-content .el-input,
.form-content .el-select,
.form-content .el-date-picker,
.form-content .el-upload {
    font-size: 12px;
    height: 30px;
}

.dialog-footer {
    text-align: right;
}

.pagination-container {
    display: flex;
    justify-content: center;
    margin-top: 20px;
}
</style>
