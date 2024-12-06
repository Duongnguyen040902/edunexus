<template>
  <div class="col-lg-3 col-12 invoice-actions">
    <div class="card mb-6">
      <div class="card-body">
        <button
          :disabled="isComplete.value"
          class="btn btn-primary d-grid w-100 mb-4"
          data-bs-target="#sendInvoiceOffcanvas"
          data-bs-toggle="offcanvas"
          @click="handleSubmitCreateOrUpdateInvoice"
        >
          <span class="d-flex align-items-center justify-content-center text-nowrap"
            ><i class="bx bx-paper-plane bx-xs me-2"></i>{{ isUpdate.value ? 'Cập nhật hóa đơn' : 'Gửi hóa đơn' }}</span
          >
        </button>
        <button
          v-if="isUpdate.value"
          :disabled="isComplete.value"
          class="btn btn-label-secondary d-grid w-100 mb-4"
          @click="handleShowModalCreatePayment"
        >
          Tạo thanh toán
        </button>
      </div>
    </div>
  </div>
  <ModalCreatePaymentComponent
    :is-show-modal="isShowModalCreatePayment"
    @closeModal="isShowModalCreatePayment = false"
    @confirmAction="handleConfirmCreatePayment"
  />
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager.ts';
import ModalCreatePaymentComponent from '@/components/modals/invoice/ModalCreatePayment.vue';

export default defineComponent({
  name: 'RightSideBarInvoiceDetailComponent',
  components: {
    ModalCreatePaymentComponent,
  },
  props: {},
  emits: [],
  setup(props, { emit }) {
    const subscription = useInvoiceManagerComposable();
    const {
      isShowModalCreatePayment,
      isComplete,
      isUpdate,
      handleSubmitCreateOrUpdateInvoice,
      handleCreatePayment,
      handleShowModalCreatePayment,
    } = subscription;

    const handleConfirmCreatePayment = async () => {
      await handleCreatePayment();
    };
    return {
      isShowModalCreatePayment,
      isComplete,
      isUpdate,
      handleSubmitCreateOrUpdateInvoice,
      handleShowModalCreatePayment,
      handleConfirmCreatePayment,
    };
  },
});
</script>
<style scoped></style>
