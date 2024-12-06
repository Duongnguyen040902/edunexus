<template>
<div class="flex-grow-1 container-p-y">
    <div class="layout-container">
        <div class="layout-page">
            <div class="content-wrapper">
                <div class="container-xl flex-grow-1 container-p-y">
                    <h4 class="fw-bold py-3 mb-4">
                        <span class="text-muted fw-light">Quản lý Tài khoản /</span> Người phụ trách xe tuyến
                    </h4>
                    <div class="card">
                        <SearchFiltersBusSupervisorComponent 
                        @updateSearch="(value) => updateRequestBusSupervisors('searchKey', value)" 
                        @updateStatus="(value) => updateRequestBusSupervisors('accountStatus', value)" 
                        @updateRecord="(value) => updateRequestBusSupervisors('pageSize', value)" 
                        />
                        <div class="dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center justify-content-end flex-md-row flex-column mb-6 mb-md-0 mt-n6 mt-md-0 gap-md-4">
                            <div class="dt-buttons btn-group flex-wrap">
                                <button class="btn btn-secondary add-new btn-danger" type="button" style="margin-right: 10px" 
                                :disabled="!isEmitValue" 
                                @click="handleShowModalDelete"
                                >
                                    <span>
                                        <i class="bx bx-trash bx-sm me-md-2"></i>
                                        <span class="d-md-inline-block d-none">Xóa tất cả</span>
                                    </span>
                                </button>
                                <button class="btn btn-secondary add-new btn-primary" style="margin-right: 10px;" @click="handleOpen">
                                    Thêm người dùng
                                </button>
                                <button class="btn btn-secondary add-new btn-primary" type="button" style="margin-right: 10px;" @click="openImportModal">
                                    Nhập excel
                                </button>
                            </div>
                        </div>
                        <BusSupervisorTableComponent 
                        :listBusSupervisorResponse="listBusSupervisorResponse.data" 
                        @showDetail="handleShowBusSupervisorDetail" 
                        :apiUrl="apiUrl" 
                        :formatDate="formatDate" 
                        @delete-busSupervisor="handleDeleteBusSupervisors" 
                        />
                        <div class="pagination-container">
                            <PaginationComponent 
                            :total-records="listBusSupervisorResponse.totalRecords" 
                            :page-size="params.pageSize" 
                            :current-page="params.pageNumber" 
                            @update:currentPage="(value) => updateRequestBusSupervisors('pageNumber', value)" 
                            @page-changed="handlePageChange" 
                            />
                        </div>
                    </div>
                    <CRUDBusSupervisorModal 
                    :isShowCreate="isShowCreate" 
                    :isCreateBusSupervisor="isCreateBusSupervisor" 
                    :isUpdateBusSupervisor="isUpdateBusSupervisor" 
                    :requestDataCreateBusSupervisor="requestDataCreateBusSupervisor" 
                    :errorsBusSupervisor="errorsBusSupervisor" 
                    :apiUrl="apiUrl" 
                    @handleSave="handleSave" 
                    @handleClose="handleClose()" 
                    @handleEditBusSupervisor="handleEditBusSupervisor()" 
                    @openEditModal="openEditModal" 
                    />
                    <ImportExcelModal 
                    :isImportModalVisible="isImportModalVisible" 
                    @handleImportExcelBusSupervisorAccount="handleImportExcelBusSupervisorAccount" 
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
import SearchFiltersBusSupervisorComponent from '@/components/views/busSupervisor/SearchFiltersBusSupervisor.vue';
import BusSupervisorTableComponent from '@/components/views/busSupervisor/BusSupervisorTable.vue';

import { useBusSupervisorComposable } from '@/composables/bus-supervisor';
import CRUDBusSupervisorModal from '@/components/modals/busSupervisor-account/CRUDBusSupervisorModal.vue';
import ModalDeleteMultipleComponent from '@/components/common/ModalConfirmDelete.vue';
import ImportExcelModal from '@/components/modals/busSupervisor-account/ImportExcelModal.vue';
export default defineComponent({
    name: 'BusSupervisorAccount',
    props: {},
    components: {
        SearchFiltersBusSupervisorComponent,
        BusSupervisorTableComponent,
        PaginationComponent,
        CRUDBusSupervisorModal,
        ModalDeleteMultipleComponent,
        ImportExcelModal,
    },
    setup(props, {
        emit
    }) {

        onMounted(async () => {
            await handleGetListBusSupervisor();
        });

        const {
            params,
            listBusSupervisorResponse,
            apiUrl,
            openEditModal,
            handleOpen,
            isShowCreate,
            isCreateBusSupervisor,
            isUpdateBusSupervisor,
            handleClose,
            requestDataCreateBusSupervisor,
            handleGetListBusSupervisor,
            errorsBusSupervisor,
            updateRequestBusSupervisors,
            handleSave,
            handlePageChange,
            handleShowBusSupervisorDetail,
            handleEditBusSupervisor,
            formatDate,
            requestDeleteBusSupervisor,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            handleImportExcelBusSupervisorAccount,
            onFileChange,
            isImportModalVisible,
            closeImportModal,
            openImportModal,
        } = useBusSupervisorComposable();

        const isEmitValue = ref(false);
        const handleDeleteBusSupervisors = async (ids: number[]) => {
            if (ids.length === 0) {
                isEmitValue.value = false;
                return;
            }
            isEmitValue.value = true;
            requestDeleteBusSupervisor.ids = ids;
        };

        return {
            params,
            listBusSupervisorResponse,
            isShowCreate,
            isCreateBusSupervisor,
            isUpdateBusSupervisor,
            apiUrl,
            errorsBusSupervisor,
            handleOpen,
            handleSave,
            handleClose,
            handleShowBusSupervisorDetail,
            openEditModal,
            handleEditBusSupervisor,
            updateRequestBusSupervisors,
            formatDate,
            requestDataCreateBusSupervisor,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            isEmitValue,
            handleDeleteBusSupervisors,
            handlePageChange,
            handleImportExcelBusSupervisorAccount,
            onFileChange,
            closeImportModal,
            openImportModal,
            isImportModalVisible,
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
