<template>
  <ModalComponent :before-close="close" :modelValue="isShowModal" :width="`800px`">
    <div class="modal-body">
      <div class="text-center mb-6">
        <h4 class="mb-2">Thêm gói</h4>
      </div>
      <form class="row g-6">
        <div class="col-12 col-md-6">
          <label class="form-label" for="name">Tên gói</label>
          <input
            id="name"
            v-model="requestCreateSubscription.name"
            class="form-control"
            name="name"
            placeholder="Nhập tên gói"
            type="text"
          />
          <div v-if="errorSubscription.Name" class="text-danger">{{ errorSubscription.Name[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="website">Mô tả</label>
          <input
            id="website"
            v-model="requestCreateSubscription.description"
            class="form-control"
            name="website"
            placeholder="Nhập mô tả"
            type="text"
          />
          <div v-if="errorSubscription.Description" class="text-danger">{{ errorSubscription.Description[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="phone">Giá</label>
          <input
            id="phone"
            v-model="requestCreateSubscription.price"
            class="form-control"
            name="phone"
            placeholder="Nhập số giá"
            type="number"
          />
          <div v-if="errorSubscription.Price" class="text-danger">{{ errorSubscription.Price[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="address">Ngày hết hạn</label>
          <input
            id="address"
            v-model="requestCreateSubscription.durationDays"
            class="form-control"
            name="address"
            placeholder="Nhập ngày hết hạn"
            type="number"
          />
          <div v-if="errorSubscription.DurationDays" class="text-danger">{{ errorSubscription.DurationDays[0] }}</div>
        </div>
        <div class="col-12 col-md-6">
          <label class="form-label" for="maxAccount">Số lượng</label>
          <input
            id="maxAccount"
            v-model="requestCreateSubscription.maxActiveAccounts"
            class="form-control"
            name="address"
            placeholder="Nhập số lượng tài khoản"
            type="number"
          />
          <div v-if="errorSubscription.MaxActiveAccounts" class="text-danger">{{ errorSubscription.MaxActiveAccounts[0] }}</div>
        </div>
      </form>
    </div>
    <template #footer>
      <div class="modal-footer">
        <div class="col-12 text-center">
          <button class="btn btn-primary me-3" type="submit" @click="confirmAction">{{isDetailSubscription ? "Cập nhật" : "Tạo"}}</button>
          <button aria-label="Close" class="btn btn-label-secondary" @click="close">Thoát</button>
        </div>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, toRefs } from 'vue';
import { useSubscriptionComposable } from '@/composables/subscription.ts';
import ModalComponent from '@/components/common/Modal.vue';

export default defineComponent({
  name: 'ModalEditAdminSchoolComponent',
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
    const subscriptionComposable = useSubscriptionComposable();
    const { requestCreateSubscription, errorSubscription, isDetailSubscription } = subscriptionComposable;
    const { isShowModal } = toRefs(props);
    const close = (value: boolean) => {
      emit('closeModal', value);
    };

    const confirmAction = () => {
      emit('confirmAction', true);
    };
    return {
      isDetailSubscription,
      requestCreateSubscription,
      errorSubscription,
      isShowModal,
      close,
      confirmAction,
    };
  },
});
</script>

<style scoped>
.el-dialog__body {
  padding: 1.5rem !important;
}

.modal-body {
  padding: 1.5rem;
}
</style>