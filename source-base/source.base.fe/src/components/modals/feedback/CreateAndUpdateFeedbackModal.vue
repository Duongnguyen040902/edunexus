<template>
  <ModalComponent
    :modelValue="showModal"
    :title="
      isUpdateMode
        ? `Cập nhật phản hồi tới ${pupilFeedback?.pupilName}`
        : `Thêm phản hồi tới ${pupilFeedback?.pupilName}`
    "
    @update:modelValue="$emit('update-showModal', $event)"
    @before-close="handleClose()"
  >
    <div class="modal-body">
      <div class="mt3" style="margin-top: 10px">
        <label class="form-label">Nội dung:</label>
        <textarea class="form-control" v-model="pupilFeedback.description"></textarea>
        <div v-if="errorFeedback.Description" class="text-danger">
          {{ errorFeedback.Description[0] }}
        </div>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose()">HỦY</button>
        <button class="btn btn-primary" @click="handleModalConfirm()">
          {{ isUpdateMode ? 'Cập nhật' : 'Thêm' }}
        </button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { PupilFeedback } from '@/types/model/feedback.ts';
import { useFeedbackComposable } from '@/composables/feedback.ts';

export default defineComponent({
  name: 'ModalCreateAndUpdateFeedback',
  components: { ModalComponent },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    pupilFeedback: {
      type: Object as PropType<PupilFeedback | null>,
    },
  },
  emits: ['update-showModal', 'confirm', 'update-isUpdateMode', 'reload-list'],
  setup(props, { emit }) {
    const { createFeedback, updateFeedback, errorFeedback, handleCloseModal } = useFeedbackComposable();

    const handleModalConfirm = async () => {
      if (props.isUpdateMode) {
        await updateFeedback(emit, props.pupilFeedback);
      } else {
        console.log('createFeedback', props.pupilFeedback);
        await createFeedback(emit, props.pupilFeedback as PupilFeedback);
      }
    };
    const handleClose = () => {
      handleCloseModal(emit);
    };

    return { handleModalConfirm, errorFeedback, handleCloseModal, handleClose };
  },
});
</script>


