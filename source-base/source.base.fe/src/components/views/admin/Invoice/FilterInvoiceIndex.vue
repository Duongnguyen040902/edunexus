<template>
  <div
    class="col-12 col-md-6 d-flex align-items-center justify-content-end flex-column flex-md-row gap-md-4 mt-n6 mt-md-0"
  >
    <form @submit.prevent="handleGetAllInvoiceManager" class="d-flex align-items-center justify-content-end flex-column flex-md-row gap-md-4 mt-n6 mt-md-0">
      <div id="DataTables_Table_0_filter" class="dataTables_filter">
        <label>
          <input
            aria-controls="DataTables_Table_0"
            class="form-control"
            placeholder="Tìm kiếm hóa đơn"
            type="search"
            v-model="requestInvoiceManagerIndex.keyword"
            @change="handleGetAllInvoiceManager"
          />
        </label>
      </div>
      <div class="invoice_status mb-6 mb-md-0">
        <select class="form-select" @change="handleGetAllInvoiceManager" v-model="requestInvoiceManagerIndex.status">
          <option value="">Trạng thái hóa đơn</option>
          <option class="text-capitalize" value="1">Chờ</option>
          <option class="text-capitalize" value="2">Đã thanh toán</option>
          <option class="text-capitalize" value="3">Hủy</option>
          <option class="text-capitalize" value="4">Đã gửi</option>
          <option class="text-capitalize" value="5">Thay đổi</option>
        </select>
      </div>
    </form>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager.ts';

export default defineComponent({
  name: 'FilterInvoiceIndexComponent',
  setup(props, { emit }) {
    const invoiceManagerComposable = useInvoiceManagerComposable();
    const { requestInvoiceManagerIndex, responseInvoiceManagerIndex, handleGetAllInvoiceManager } = invoiceManagerComposable;

    return {
      requestInvoiceManagerIndex,
      responseInvoiceManagerIndex,
      handleGetAllInvoiceManager,
    };
  },
});
</script>

<style scoped></style>