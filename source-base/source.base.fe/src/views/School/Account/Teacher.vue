<template>
<div class="flex-grow-1 container-p-y">
    <div class="layout-container">
        <div class="layout-page">
            <div class="content-wrapper">
                <div class="container-xxl flex-grow-1 container-p-y">
                    <h4 class="fw-bold py-3 mb-4">
                        <span class="text-muted fw-light">Quản lý Tài khoản /</span> Giáo viên
                    </h4>
                    <div class="card">
                        <SearchFiltersTeacherComponent 
                        :subjectFilter="listSubjects" 
                        @updateSearch="(value) => updateRequestTeachers('searchKey', value)" 
                        @updateSubject="(value) => updateRequestTeachers('subjectId', value)" 
                        @updateStatus="(value) => updateRequestTeachers('accountStatus', value)" 
                        @updateRecord="(value) => updateRequestTeachers('pageSize', value)" 
                        />
                        <div class="dt-action-buttons text-xl-end text-lg-start text-md-end text-start d-flex align-items-center 
                        justify-content-end flex-md-row flex-column mb-6 mb-md-0 mt-n6 mt-md-0 gap-md-4">
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
                                <button class="btn btn-secondary add-new btn-primary" @click="handleOpen" style="margin-right: 10px;">
                                    Thêm giáo viên
                                </button>
                                <button class="btn btn-secondary add-new btn-primary" type="button" style="margin-right: 10px;" @click="openImportModal">
                                    Nhập excel
                                </button>
                            </div>
                        </div>
                        <TeacherTableComponent 
                        :teachersData="listTeacherResponse.data"
                        @showDetail="handleShowTeacherDetail" 
                        :apiUrl="apiUrl" 
                        :formatDate="formatDate" 
                        @delete-teachers="handleDeleteTeachers" 
                        />
                        <div class="row">
                            <div class="col-sm-12 col-md-6">
                                
                            </div>
                            <div class="col-sm-12 col-md-6">
                                <div id="DataTables_Table_0_paginate" class="dataTables_paginate paging_simple_numbers">
                                    <PaginationComponent 
                                    :current-page="requestTeachers.pageNumber" 
                                    :page-size="requestTeachers.pageSize" 
                                    :total-records="listTeacherResponse.totalRecords" 
                                    @page-changed="handlePageChange"
                                    />
                                </div>
                            </div>
                        </div>
                        
                    </div>
                </div>
                <CRUDTeacherModal 
                :isShowCreate="isShowCreate" 
                :isCreateTeacher="isCreateTeacher" 
                :isUpdateTeacher="isUpdateTeacher" 
                :requestDataCreateTeacher="requestDataCreateTeacher" 
                :errorsCreateTeacher="errorsCreateTeacher" 
                :listSubjects="listSubjects" 
                :apiUrl="apiUrl" 
                @handleSave="handleSave" 
                @handleClose="handleClose()" 
                @handleEditTeacher="handleEditTeacher()" 
                @openEditModal="openEditModal" 
                />  
            </div>
            <ImportExcelModal 
                :isImportModalVisible="isImportModalVisible" 
                @handleImportExcelTeacherAccount="handleImportExcelTeacherAccount" 
                @closeImportModal="closeImportModal" 
                @onFileChange="onFileChange"
            />  
        </div>
    </div>
</div>
<ModalDeleteMultipleComponent 
:isShowModal="isCheckedDelete" 
@confirmAction="handleConfirmDelete" 
/>
</template>

<script lang="ts">
import {
    defineComponent,
    onMounted,
    ref,
} from 'vue';
import SearchFiltersTeacherComponent from '@/components/views/account-teacher/SearchFiltersTeacher.vue';
import TeacherTableComponent from '@/components/views/account-teacher/TeacherTable.vue';
import {
    useTeachersComposable
} from '@/composables/teacher-account';
import PaginationComponent from '@/components/common/Pagination.vue';
import CRUDTeacherModal from '@/components/modals/teacher-account/CRUDTeacherModal.vue';
import ModalDeleteMultipleComponent from '@/components/common/ModalConfirmDelete.vue';
import ImportExcelModal from '@/components/modals/teacher-account/ImportExcelModal.vue';
export default defineComponent({
    name: 'TeacherAccountManage',
    components: {
        SearchFiltersTeacherComponent,
        TeacherTableComponent,
        PaginationComponent,
        CRUDTeacherModal,
        ModalDeleteMultipleComponent,
        ImportExcelModal,
    },
    setup() {
        onMounted(async () => {
            await handleFetchTeachers();
        });
        const {
            listTeacherResponse,
            requestTeachers,
            totalRecords,
            currentPage,
            requestDataCreateTeacher,
            isCreateTeacher,
            isUpdateTeacher,
            errorsCreateTeacher,
            listSubjects,
            apiUrl,
            isShowCreate,
            handleFetchTeachers,
            handlePageChange,
            updateRequestTeachers,
            handleSave,
            handleOpen,
            handleClose,
            openEditModal,
            handleShowTeacherDetail,
            handleEditTeacher,
            formatDate,
            requestDeleteTeacher,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            requestImportExcelTeacher,
            handleImportExcelTeacherAccount,
            isImportModalVisible,
            onFileChange,
            closeImportModal,
            openImportModal,
        } = useTeachersComposable();

        const isEmitValue = ref(false);
        const handleDeleteTeachers = async (ids: number[]) => {
            if (ids.length === 0) {
                isEmitValue.value = false;
                return;
            }
            isEmitValue.value = true;
            requestDeleteTeacher.ids = ids;
        };


        return {
            totalRecords,
            currentPage,
            requestTeachers,
            isShowCreate,
            listSubjects,
            requestDataCreateTeacher,
            isCreateTeacher,
            isUpdateTeacher,
            listTeacherResponse,
            apiUrl,
            errorsCreateTeacher,
            handleFetchTeachers,
            updateRequestTeachers,
            handlePageChange,
            handleSave,
            handleOpen,
            handleClose,
            handleShowTeacherDetail,
            handleEditTeacher,
            openEditModal,
            formatDate,
            handleDeleteTeachers,
            handleShowModalDelete,
            handleCloseModalDelete,
            handleConfirmDelete,
            isCheckedDelete,
            isEmitValue,
            requestImportExcelTeacher,
            handleImportExcelTeacherAccount,
            isImportModalVisible,
            onFileChange,
            closeImportModal,
            openImportModal,
        };
    }
});
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
</style>
