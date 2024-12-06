<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <h4 class="fw-bold py-3 mb-4">
      Quản lí năm học 
    </h4>
    <div class="card">
      <div class="card-header border-bottom d-flex justify-content-between align-items-center">
        <h5 class="card-title mb-0">Danh sách năm học</h5>
        <button class="btn btn-secondary add-new btn-primary" tabindex="0" aria-controls="DataTables_Table_0" type="button" @click="handleOpenModalCreate">
                    <span>
                      <i class="bx bx-plus bx-sm me-0 me-sm-2"></i>
                      <span class="d-none d-sm-inline-block">Thêm năm học</span>
                    </span>
                  </button>
      </div>
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <TableSchoolYearComponent :dataSchoolYear="dataSchoolYear.value.data" @openModalEdit="handleOpenModalEdit" 
          @openModalDelete="handleDelete" @isActive="isActive"/>
          <div class="row">
            <div class="col-sm-12">
              <div v-if="dataSchoolYear.value.totalRecords / dataSchoolYear.value.pageSize >1" class="d-flex justify-content-center">
                <div class="dataTables_paginate paging_simple_numbers" id="DataTables_Table_0_paginate">
                  <PaginationComponent :total-records="dataSchoolYear.value.totalRecords"
                    :page-size="dataSchoolYear.value.pageSize"
                    :current-page="dataSchoolYear.value.pageNumber"
                    @update:currentPage="currentPage = $event" @page-changed="handlePageChange" />
                </div>
              </div>
              <ModalEditSchoolYearComponent :isShowModal="isShowModalEdit" :requestSchoolYearUpdate="requestSchoolYearUpdate" :isCreateSchoolYear="isCreateSchoolYear"
                @confirmCreate="handleConfirmCreate" @closeModal="handleCloseModalEdit" @confirmAction="handleConfirmEdit" />
              <ModalConfirmDeleteComponent :isShowModal="isShowModalDelete" :id="schoolYearId"
                @closeModal="handleCloseModalDelete" @confirmAction="handleConfirmDelete" />
            </div>
          </div>
          <div style="width: 1%"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import TableSchoolYearComponent from "@/components/views/school-admin/TableSchoolYear.vue";
import PaginationComponent from "@/components/common/Pagination.vue";
import { useSchoolYearComposable } from "@/composables/school-year.ts";
import ModalEditSchoolYearComponent from '@/components/modals/school-year/EditSchoolYearModal.vue';
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';

export default defineComponent({
  name: "SchoolYear",
  components: {
    TableSchoolYearComponent,
    PaginationComponent,
    ModalEditSchoolYearComponent,
    ModalConfirmDeleteComponent,
  },
  setup() {
    const schoolYearComposable = useSchoolYearComposable();
    const {
      dataSchoolYear,
      handleGetSchoolYearIndex,
      handlePageChange,
      requestSchoolYearUpdate,
      handleOpenModalEdit,
      isShowModalDelete,
      isShowModalEdit,
      handleCloseModalEdit,
      handleConfirmEdit,
      handleOpenModalDelete,
      handleCloseModalDelete,
      handleConfirmDelete,
      handleOpenModalCreate,
      handleConfirmCreate,
      isCreateSchoolYear,
    } = schoolYearComposable;

    const schoolYearId = ref<number | string | undefined>(undefined);

    const handleDelete = (schoolYear: any) => {
      if (schoolYear.isActive) {
        alert('Không thể xóa kì đang hoạt động');
        return;
      }
      schoolYearId.value = schoolYear.id;
      handleOpenModalDelete();
    };

    const isActive = (semester: any) => {
      if (semester.isActive) {
        alert('Kỳ học này đang hoạt động');
        return;
      }
      requestSchoolYearUpdate.id = semester.id;
      requestSchoolYearUpdate.startDate = semester.startDate;
      requestSchoolYearUpdate.endDate = semester.endDate;
      requestSchoolYearUpdate.name = semester.name;
      requestSchoolYearUpdate.isActive = true;
      handleConfirmEdit();
    };
    
    onMounted(async () => {
      await handleGetSchoolYearIndex();
    });

    return {
      isActive,
      requestSchoolYearUpdate,
      isShowModalEdit,
      handleConfirmCreate,
      isCreateSchoolYear,
      isShowModalDelete,
      handleDelete,
      handleConfirmEdit,
      handleOpenModalEdit,
      handleOpenModalCreate,
      dataSchoolYear,
      handleCloseModalEdit,
      handleGetSchoolYearIndex,
      handlePageChange,
      handleOpenModalDelete,
      handleCloseModalDelete,
      handleConfirmDelete,
      schoolYearId,
    };
  },
});
</script>