<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <!-- Invoice List Table -->
    <div class="card">
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="row">
            <div
              class="col-12 col-md-6 d-flex align-items-center justify-content-center justify-content-md-start gap-2"
            >
              <div id="DataTables_Table_0_length" class="dataTables_length"></div>
              <div class="dt-action-buttons text-xl-end text-lg-start text-md-end text-start">
                <div class="dt-buttons btn-group flex-wrap"></div>
              </div>
              <div class="dt-action-buttons text-xl-end text-lg-start text-md-end text-start">
                <div class="dt-buttons btn-group flex-wrap">
                  <button
                    :disabled="!isEmitValue"
                    aria-controls="DataTables_Table_0"
                    class="btn btn-danger"
                    tabindex="0"
                    type="button"
                    @click="handleShowModalDelete"
                  >
                    <span>
                      <i class="bx bx-trash bx-sm me-md-2"></i>
                      <span class="d-md-inline-block d-none">Xóa tất cả</span>
                    </span>
                  </button>
                </div>
              </div>
            </div>
            <FilterInvoiceIndexComponent />
          </div>
          <TableIndexComponent
            :invoices="responseInvoiceManagerIndex.value.data"
            @delete-invoices="handleDeleteInvoices"
          />
          <div class="row">
            <div class="col-sm-12 col-md-6">
            </div>
            <div class="col-sm-12 col-md-6">
              <div
                v-if="responseInvoiceManagerIndex.value.totalRecords > responseInvoiceManagerIndex.value.pageSize"
                id="DataTables_Table_0_paginate"
                class="dataTables_paginate paging_simple_numbers"
              >
                <PaginationComponent
                  :current-page="responseInvoiceManagerIndex.value.pageNumber"
                  :page-size="responseInvoiceManagerIndex.value.pageSize"
                  :total-records="responseInvoiceManagerIndex.value.totalRecords"
                  @update:currentPage="currentPage = $event"
                  @page-changed="handlePageChange"
                />
              </div>
            </div>
          </div>
          <div style="width: 1%"></div>
        </div>
      </div>
      <ModalDeleteMultipleComponent
        :isShowModal="isCheckedDelete"
        @closeModal="handleCloseModalDelete"
        @confirmAction="handleConfirmDelete"
      />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager.ts';
import TableIndexComponent from '@/components/views/admin/Invoice/TableIndex.vue';
import FilterInvoiceIndexComponent from '@/components/views/admin/Invoice/FilterInvoiceIndex.vue';
import PaginationComponent from '@/components/common/Pagination.vue';
import ModalDeleteMultipleComponent from '@/components/common/ModalConfirmDelete.vue';
import router from '@/router';

export default defineComponent({
  name: 'InvoiceManager',
  components: {
    TableIndexComponent,
    FilterInvoiceIndexComponent,
    PaginationComponent,
    ModalDeleteMultipleComponent,
  },
  setup() {
    const invoiceManagerComposable = useInvoiceManagerComposable();
    const {
      selectAll,
      isEmitValue,
      requestDeleteInvoiceManager,
      responseInvoiceManagerIndex,
      isCheckedDelete,
      handleGetAllInvoiceManager,
      handlePageChange,
      handleShowModalDelete,
      handleCloseModalDelete,
      handleConfirmDelete,
    } = invoiceManagerComposable;
    const handleDeleteInvoices = async (ids: number[]) => {
      if (ids.length === 0) {
        isEmitValue.value = false;
        return;
      }
      isEmitValue.value = true;
      requestDeleteInvoiceManager.ids = ids;
    };

    onMounted(async () => {
      await handleGetAllInvoiceManager();
    });

    const handleRedirectCreatePage = () => {
      router.push({ name: 'CreateInvoice' });
    };

    return {
      isEmitValue,
      isCheckedDelete,
      responseInvoiceManagerIndex,
      handleGetAllInvoiceManager,
      handlePageChange,
      handleDeleteInvoices,
      handleShowModalDelete,
      handleCloseModalDelete,
      handleConfirmDelete,
      handleRedirectCreatePage,
    };
  },
});
</script>

<style scoped>
.dt-buttons.btn.btn-danger:disabled {
  background-color: #d9534f;
  opacity: 0.65;
  cursor: not-allowed !important;
  text-decoration: line-through;
}
</style>
