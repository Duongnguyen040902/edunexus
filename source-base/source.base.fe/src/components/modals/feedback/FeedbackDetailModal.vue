<template>
  <ModalComponent
    :modelValue="showModal"
    :title="`Chi tiết phản hồi`"
    @update:modelValue="$emit('update-showModal', $event)"
    @before-close="handleClose()"
  >
    <div class="modal-body">
      <div class="d-flex justify-content-between mt3" style="margin-top: 10px">
        <div>
          <label class="form-label">Kỳ: </label>
          <p>{{ pupilFeedback?.semester.semesterName }} - {{ pupilFeedback?.semester.schoolYearName }}</p>
        </div>
        <div>
          <label class="form-label">Ngày tạo:</label>
          <p>{{ formatDate(pupilFeedback?.createdDate) }}</p>
        </div>
      </div>
      <div class="mt3" style="margin-top: 10px">
        <label class="form-label">Nội dung:</label>
        <p>{{ pupilFeedback?.description }}</p>
      </div>
    </div>

    <template #footer>
      <div class="dialog-footer">
        <button class="btn btn-outline-secondary" style="margin-right: 10px" @click="handleClose()">ĐÓNG</button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { FeedbackDetail } from '@/types/model/feedback.ts';
import { formatDate } from '@/helpers/formatDate';

export default defineComponent({
  name: 'ModalViewFeedbackDetail',
  components: { ModalComponent },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    pupilFeedback: {
      type: Object as PropType<FeedbackDetail | null>,
    },
  },
  emits: ['update-showModal'],
  setup(props, { emit }) {
    const handleClose = () => {
      emit('update-showModal', false);
    };

    return { formatDate, handleClose };
  },
});
</script>