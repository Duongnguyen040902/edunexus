<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <div class="card mt-5">
      <div class="card table-responsive" style="margin-bottom: 0">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header border-bottom">
            <h5 class="card-title mb-0">Tổng hợp phản hồi của các kỳ</h5>
          </div>
          <table class="table" style="margin-bottom: 0">
            <thead>
            <tr>
              <th class="col-1">STT</th>
              <th class="col-2">Kỳ học</th>
              <th class="col-2">Năm học</th>
              <th class="col-3">Phản hồi</th>
              <th class="col-2">Ngày tạo</th>
              <th class="col-1">Hành động</th>
            </tr>
            </thead>
            <tbody>
            <tr v-if="pupilFeedbacks.length === 0">
              <td colspan="6" class="text-center">Chưa có phản hồi nào</td>
            </tr>
            <tr v-else v-for="(feedback, index) in pupilFeedbacks" :key="index">
              <td>{{ index + 1 }}</td>
              <td>{{ feedback.semester.semesterName }}</td>
              <td>{{ feedback.semester.schoolYearName }}</td>
              <td>{{ feedback.description }}</td>
              <td>{{ formatDate(feedback.createdDate) }}</td>
              <td>
                <i @click="handleOpenModal(feedback)" class="bx bxs-show"></i>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <ModalViewFeedbackDetail
    :show-modal="isShowModal"
    :pupil-feedback="selectedFeedback"
    @update-show-modal="isShowModal = $event"
  />
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue';
import { formatDate } from '@/helpers/formatDate';
import { useFeedbackComposable } from '@/composables/feedback.ts';
import ModalViewFeedbackDetail from '@/components/modals/feedback/FeedbackDetailModal.vue';
import { FeedbackDetail } from '@/types/model/feedback.ts';

export default defineComponent({
  name: 'PupilFeedback',
  components: { ModalViewFeedbackDetail },
  setup() {
    const { pupilFeedbacks, fetchPupilFeedbackList } = useFeedbackComposable();
    const isShowModal = ref(false);
    const selectedFeedback = ref<FeedbackDetail | null>(null);

    const handleOpenModal = (feedback: FeedbackDetail) => {
      selectedFeedback.value = feedback;
      isShowModal.value = true;
    };

    onMounted(async () => {
      await fetchPupilFeedbackList();
    });

    return {
      pupilFeedbacks,
      formatDate,
      isShowModal,
      selectedFeedback,
      handleOpenModal,
    };
  },
});
</script>