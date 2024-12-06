<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <ClassSidebarComponent />

    <div class="card mt-5">
      <div class="card" style="margin-bottom: 0">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="card-header border-bottom">
            <h5 class="card-title mb-0">Tổng hợp phản hồi</h5>
          </div>
          <table class="table" style="margin-bottom: 0">
            <thead>
            <tr>
              <th class="col-1">STT</th>
              <th class="col-2">Họ và tên</th>
              <th class="col-1">Ảnh</th>
              <th class="col-2">Phụ huynh</th>
              <th class="col-3">Phản hồi</th>
              <th class="col-1">Hành động</th>
            </tr>
            </thead>
            <tbody>
            <tr v-if="classFeedback.length === 0">
              <td colspan="6" class="text-center">Lớp chưa có học sinh nào trong kỳ này</td>
            </tr>
            <tr v-else v-for="(feedback, index) in classFeedback" :key="index">
              <td>{{ index + 1 }}</td>
              <td>{{ feedback.pupilName }}</td>
              <td>
                <ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">
                  <li
                    data-bs-toggle="tooltip"
                    data-popup="tooltip-custom"
                    data-bs-placement="top"
                    class="avatar avatar-xl pull-up"
                    :title="feedback.pupilName"
                  >
                    <img :src="`${apiUrl}${feedback.image}`" alt="Avatar" class="rounded-circle" />
                  </li>
                </ul>
              </td>
              <td>{{ feedback.donorName }}</td>
              <td>{{ feedback.description ?? 'Chưa phản hồi' }}</td>
              <td>
                <div class="d-flex justify-content-around align-items-center">
                  <div
                    v-if="feedback.description === null"
                    @click="handleOpenModal(feedback, false)"
                    class="action-icon"
                  >
                    <i class="bx bx-plus" title="Tạo phản hồi"></i>
                  </div>
                  <div
                    v-if="feedback.description != null"
                    class="action-icon"
                    @click="handleOpenModal(feedback, true)"
                  >
                    <i class="bx bx-edit" title="Sửa phản hồi"></i>
                  </div>
                  <div
                    v-if="feedback.description != null"
                    @click="handleOpenDeleteModal(feedback.pupilId, feedback.semesterId)"
                    class="action-icon"
                  >
                    <i class="bx bx-trash" title="Xóa phản hồi"></i>
                  </div>
                </div>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <ModalCreateAndUpdateFeedback
    :show-modal="isShowModal"
    :is-update-mode="isUpdate"
    :pupil-feedback="selected"
    @update-show-modal="isShowModal = $event"
    @reload-list="fetchClassFeedback()"
  />
  <DeleteFeedbackModal
    :show-modal="isShowDeleteModal"
    :pupil-id="selectedPupilId ?? 0"
    :semester-id="selectedSemesterId ?? 0"
    @update-show-modal="isShowDeleteModal = $event"
    @reload-list="fetchClassFeedback()"
  />
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import ClassSidebarComponent from '@/components/views/class/ClassSiderbar.vue';
import { useFeedbackComposable } from '@/composables/feedback.ts';
import { PupilFeedback } from '@/types/model/feedback.ts';
import ModalCreateAndUpdateFeedback from '@/components/modals/feedback/CreateAndUpdateFeedbackModal.vue';
import DeleteFeedbackModal from '@/components/modals/feedback/DeleteFeedbackModal.vue';

export default defineComponent({
  name: 'FeedbackManagement',
  components: { ClassSidebarComponent, ModalCreateAndUpdateFeedback, DeleteFeedbackModal },
  setup() {
    const { fetchClassFeedback, isShowDeleteModal, classFeedback, isShowModal, isUpdate } = useFeedbackComposable();
    const apiUrl = import.meta.env.VITE_APP_API_URL;
    const selected = ref<PupilFeedback | null>(null);

    const selectedPupilId = ref<number | null>(null);
    const selectedSemesterId = ref<number | null>(null);

    const handleOpenModal = (item: PupilFeedback, isUpdateMode: boolean) => {
      selected.value = { ...(item as PupilFeedback) };
      isShowModal.value = true;
      isUpdate.value = isUpdateMode;
    };

    const handleOpenDeleteModal = (pupilId: number, semesterId: number) => {
      selectedPupilId.value = pupilId;
      selectedSemesterId.value = semesterId;
      isShowDeleteModal.value = true;
    };

    onMounted(async () => {
      await fetchClassFeedback();
    });

    return {
      classFeedback,
      apiUrl,
      isShowModal,
      isUpdate,
      handleOpenModal,
      selected,
      isShowDeleteModal,
      handleOpenDeleteModal,
      selectedPupilId,
      selectedSemesterId,
      fetchClassFeedback,
    };
  },
});
</script>

<style>
.action-icon {
  cursor: pointer;
  padding: 3px;
}

.action-icon:hover {
  color: #ff4444;
  border-radius: 5px;
}
</style>