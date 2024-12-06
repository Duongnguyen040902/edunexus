<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <h4 class="fw-bold py-3 mb-4"><a @click="gotoSchoolYear" class="text-muted fw-light" >Quản lí năm học /</a> Quản lí kỳ học</h4>
    <div class="card">
      <div class="card-header border-bottom d-flex justify-content-between align-items-center">
                <h5 class="card-title mb-0">Danh sách kì học</h5>
                <button class="btn btn-secondary add-new btn-primary" tabindex="0" aria-controls="DataTables_Table_0" type="button" @click="handleOpenModalCreate">
                    <span>
                      <i class="bx bx-plus bx-sm me-0 me-sm-2"></i>
                      <span class="d-none d-sm-inline-block">Thêm học kỳ</span>
                    </span>
                  </button>
            </div>
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="table-container" >
            <TableSemesterComponent :dataSemester="dataSemester.value" @openModalEdit="handleOpenModalEdit" 
            @openModalDelete="handleDelete" @isActive="isActive"/>
          </div>
          <div class="row">
            <div class="col-sm-12 col-md-6">
              <ModalEditSemesterComponent :isShowModal="isShowModalEdit" :requestSemesterUpdate="requestSemesterUpdate" :isCreateSemester="isCreateSemester"
                @confirmCreate="handleConfirmCreate" @closeModal="handleCloseModalEdit" @confirmAction="handleConfirmEdit" />
              <ModalConfirmDeleteComponent :isShowModal="isShowModalDelete" :id="semesterId"
                @closeModal="handleCloseModalDelete" @confirmAction="handleConfirmDelete" />
            </div>
          </div>
          <div style="width: 50%"></div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from "vue";
import TableSemesterComponent from "@/components/views/school-admin/TableSemester.vue";
import { useSemesterComposable } from "@/composables/semester";
import ModalEditSemesterComponent from '@/components/modals/school-year/EditSemesterModal.vue';
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';

export default defineComponent({
  name: "Semester",
  components: {
    TableSemesterComponent,
    ModalEditSemesterComponent,
    ModalConfirmDeleteComponent,
  },
  setup() {
    const semesterComposable = useSemesterComposable();
    const {
      dataSemester,
      handleGetSemesterIndex,
      requestSemesterUpdate,
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
      isCreateSemester,
      gotoSchoolYear,
    } = semesterComposable;
    const semesterId = ref<number | string | undefined>(undefined);

    const handleDelete = (semester:any) => {
      if (semester.isActive) {
        alert('Không thể xóa kỳ học đang hoạt động');
        return;
      }
      semesterId.value = semester.id;
      handleOpenModalDelete();
    };

    const isActive = (semester: any) => {
      if (semester.isActive) {
        alert('Kỳ học này đang hoạt động');
        return;
      }
      requestSemesterUpdate.id = semester.id;
      requestSemesterUpdate.startDate = semester.startDate;
      requestSemesterUpdate.endDate = semester.endDate;
      requestSemesterUpdate.semesterName = semester.semesterName;
      requestSemesterUpdate.semesterCode= semester.semesterCode;
      requestSemesterUpdate.schoolYearId = semester.schoolYearId;
      requestSemesterUpdate.isActive = true;
      handleConfirmEdit();
    };
    onMounted(async () => {
      await handleGetSemesterIndex();
    });

    return {
      isActive,
      gotoSchoolYear,
      requestSemesterUpdate,
      isShowModalEdit,
      handleConfirmCreate,
      isCreateSemester,
      isShowModalDelete,
      handleDelete,
      handleConfirmEdit,
      handleOpenModalEdit,
      handleOpenModalCreate,
      dataSemester,
      handleCloseModalEdit,
      handleGetSemesterIndex,
      handleOpenModalDelete,
      handleCloseModalDelete,
      handleConfirmDelete,
      semesterId,
    };
  },
});
</script>

<style scoped>
.table-container {
  height: 600px; /* Adjust the height as needed */
  overflow-y: auto;
}
</style>