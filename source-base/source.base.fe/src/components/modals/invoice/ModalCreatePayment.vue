<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`400px`">
    <div class="modal-body text-center">
      <!-- Tiêu đề -->
      <h4 class="mb-4 fw-bold">Tạo thanh toán</h4>

      <!-- Form -->
      <div class="mb-4">
        <el-select
          v-model="requestCreatePayment.paymentMethod"
          class="w-100"
          filterable
          placeholder="Phương thức thanh toán"
        >
          <el-option v-for="(method, value) in paymentMethod" :key="value" :label="method" :value="method"></el-option>
        </el-select>
        <div v-if="errorPayment.PaymentMethod" class="text-danger mt-2">
          {{ errorPayment.PaymentMethod[0] }}
        </div>
      </div>
    </div>

    <!-- Footer -->
    <template #footer>
      <div class="d-flex justify-content-center gap-3">
        <button class="btn btn-primary px-4" type="submit" @click="confirmAction">Tạo</button>
        <button class="btn btn-outline-secondary px-4" type="button" @click="close">Thoát</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager.ts';
import { PaymentMethod } from '@/constants/enums/statuses.ts';

export default defineComponent({
  name: 'ModalCreatePaymentComponent',
  components: {
    ModalComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['closeModal', 'confirmAction'],
  setup(props, { emit }) {
    const invoiceManager = useInvoiceManagerComposable();
    const { requestCreatePayment, errorPayment } = invoiceManager;
    const paymentMethod = Object.values(PaymentMethod);

    const close = (value: boolean) => {
      emit('closeModal', value);
    };

    const confirmAction = () => {
      emit('confirmAction', true);
    };

    return {
      paymentMethod,
      requestCreatePayment,
      errorPayment,
      confirmAction,
      close,
    };
  },
});
</script>

<style scoped>
.modal-body {
  padding: 20px;
}

h4 {
  font-size: 18px;
}

label {
  font-weight: 500;
  font-size: 14px;
}

.el-select {
  font-size: 14px;
}

.modal-footer {
  padding: 20px;
}

.btn {
  border-radius: 4px;
}
</style>
