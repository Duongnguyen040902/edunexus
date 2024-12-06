<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="card">
      <SearchAdminSchoolComponent />
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="row">
            <div class="col-md-2">
              <div class="ms-n2">
                <div id="DataTables_Table_0_length" class="dataTables_length">
                  <button
                    aria-controls="DataTables_Table_0"
                    class="btn btn-secondary add-new btn-primary"
                    @click="handleOpenModalCreate"
                  >
                    <span>
                      <i class="bx bx-plus bx-sm me-0 me-sm-2"></i>
                      <span class="d-none d-sm-inline-block">Tạo trường</span>
                    </span>
                  </button>
                </div>
              </div>
            </div>
          </div>
          <TableAdminSchoolComponent :dataSchoolAdmin="dataSchoolAdmin.value.data" />
          <div class="row">
            <div class="col-sm-12 col-md-6"></div>
            <div class="col-sm-12 col-md-6">
              <div
                v-if="dataSchoolAdmin.value.totalRecords > dataSchoolAdmin.value.pageSize"
                id="DataTables_Table_0_paginate"
                class="dataTables_paginate paging_simple_numbers"
              >
                <PaginationComponent
                  :current-page="dataSchoolAdmin.value.pageNumber"
                  :page-size="dataSchoolAdmin.value.pageSize"
                  :total-records="dataSchoolAdmin.value.totalRecords"
                  @update:currentPage="currentPage = $event"
                  @page-changed="handlePageChange"
                />
              </div>
            </div>
          </div>
          <div style="width: 1%"></div>
        </div>
      </div>
    </div>
    <ModalCreateAdminSchool
      :isShowModal="isShowModalCreate"
      @closeModal="handleCloseModalCreate"
      @confirmAction="handleConfirmCreateSchoolAdmin"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import HeaderManagerAdminSchoolComponent from '@/components/views/admin/HeaderManagerAdminSchool.vue';
import SearchAdminSchoolComponent from '@/components/views/admin/SearchAdminSchool.vue';
import TableAdminSchoolComponent from '@/components/views/admin/TableAdminSchool.vue';
import PaginationComponent from '@/components/common/Pagination.vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import ModalCreateAdminSchool from '@/components/modals/school-admin/ModalCreateAdminSchool.vue';

export default defineComponent({
  name: 'SchoolAccount',
  components: {
    HeaderManagerAdminSchoolComponent,
    TableAdminSchoolComponent,
    SearchAdminSchoolComponent,
    PaginationComponent,
    ModalCreateAdminSchool,
  },
  setup() {
    const adminSchoolComposable = useAdminSchoolComposable();
    const {
      isShowModalCreate,
      requestCreateSchoolAdmin,
      dataSchoolAdmin,
      handleOpenModalCreate,
      handleCloseModalCreate,
      handleGetAllAcccountSchoolAdmin,
      handlePageChange,
      handleConfirmCreateSchoolAdmin,
    } = adminSchoolComposable;

    onMounted(async () => {
      await handleGetAllAcccountSchoolAdmin();
    });

    return {
      isShowModalCreate,
      requestCreateSchoolAdmin,
      dataSchoolAdmin,
      handleOpenModalCreate,
      handleCloseModalCreate,
      handleGetAllAcccountSchoolAdmin,
      handlePageChange,
      handleConfirmCreateSchoolAdmin,
    };
  },
});
</script>
