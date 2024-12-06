<template>
  <ModalComponent :modelValue="isShowModal" :before-close="close" width="400px">
    <template #default>
      <div class="delete-confirm-body">
        <IconWarningComponent width="48" height="48" fill="#f44336" />
        <p>{{ message }}</p>
      </div>
    </template>

    <template #footer>
      <div class="modal-footer">
        <el-button type="danger" @click="confirmAction" class="confirm-delete-button">Đồng ý</el-button>
        <el-button @click="close(false)" class="cancel-button">Hủy</el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, toRefs } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import IconWarningComponent from '@/components/svg/IconWarning.vue';

export default defineComponent({
  name: 'ModalConfirmDeleteComponent',
  components: {
    ModalComponent,
    IconWarningComponent,
  },
  props: {
    isShowModal: {
      type: Boolean,
      default: false,
    },
    id: {
      type: [Number, String],
      default: null,
    },
    message: {
      type: String,
      default: 'Bạn có chắc muốn xóa dữ liệu này không?',
    },
    title: {
      type: String,
      default: '',
    },
  },
  emits: ['closeModal', 'confirmAction'],
  setup(props, { emit }) {
    const { isShowModal, id, message, title } = toRefs(props);

    const close = (value: boolean) => {
      emit('closeModal', value);
    };

    const confirmAction = () => {
      emit('confirmAction', id.value);
    };

    return {
      isShowModal,
      message,
      title,
      close,
      confirmAction,
    };
  },
});
</script>

<style scoped>
.delete-confirm-body {
  text-align: center;
  margin: 20px 0;
}

.confirm-delete-button {
  background-color: #f44336 !important;
  border-color: #f44336 !important;
  color: #fff !important;
  transition: background-color 0.3s, border-color 0.3s;
}

.confirm-delete-button:hover {
  background-color: #d32f2f !important;
  border-color: #d32f2f !important;
}

.cancel-button {
  transition: background-color 0.3s, border-color 0.3s;
}

.cancel-button:hover {
  background-color: #e0e0e0 !important;
  border-color: #e0e0e0 !important;
}

.delete-confirm-body p {
  font-size: 16px;
  font-weight: bold;
  color: var(--bs-card-title-color);
  margin-top: 10px;
}

.modal-footer {
  display: flex;
  justify-content: center;
  gap: 10px; 
}
</style>