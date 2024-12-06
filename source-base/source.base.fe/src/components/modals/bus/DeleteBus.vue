<template>
  <ModalComponent
    :modelValue="showDeleteModal"
    :title="'Xóa xe'"
    @update:modelValue="$emit('update:showDeleteModal', $event)"
  >
    <p>Bạn có chắc chắn muốn xóa xe này không?</p>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="$emit('update:showDeleteModal', false)">HỦY</el-button>
        <el-button type="danger" @click="handleDeleteConfirm" class="custom-confirm-button"> XÓA </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';

export default defineComponent({
  name: 'DeleteBusComponent',
  components: {
    ModalComponent,
  },
  props: {
    showDeleteModal: {
      type: Boolean,
      required: true,
    },
  },
  emits: ['update:showDeleteModal', 'confirm'],
  setup(props, { emit }) {
    const handleDeleteConfirm = () => {
      emit('confirm');
    };

    return {
      handleDeleteConfirm,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}

.custom-confirm-button {
  background-color: #696cff !important;
  border-color: #696cff !important;
}
</style>
