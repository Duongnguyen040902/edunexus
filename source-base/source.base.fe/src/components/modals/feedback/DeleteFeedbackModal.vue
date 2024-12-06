<template>
  <ModalComponent
    :modelValue="showModal"
    title="Xóa phản hồi"
    @update:modelValue="$emit('update-showModal', $event)"
    @before-close="handleClose"
  >
    <div class="modal-body">
      <p>Bạn có chắc chắn muốn xóa phản hồi này không?</p>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose()">HỦY</button>
        <button class="btn btn-danger" @click="handleDeleteConfirm()">XÓA</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { useFeedbackComposable } from '@/composables/feedback.ts';

export default defineComponent({
  name: 'DeleteFeedbackModal',
  components: { ModalComponent },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    pupilId: {
      type: Number,
      required: true,
    },
    semesterId: {
      type: Number,
      required: true,
    },
  },
  emits: ['update-showModal', 'confirm', 'reload-list'],
  setup(props, { emit }) {
    const { deleteFeedback, handleCloseModal } = useFeedbackComposable();

    const handleDeleteConfirm = async () => {
      await deleteFeedback(emit, props.pupilId, props.semesterId);
    };

    const handleClose = () => {
      handleCloseModal(emit);
    };

    return { handleDeleteConfirm, handleClose };
  },
});
</script>
