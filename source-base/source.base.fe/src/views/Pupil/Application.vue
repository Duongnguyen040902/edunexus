<template>
  <div class="container-fluid flex-grow-1 container-p-y">
    <div class="card mb-4">
      <h5 class="card-header d-flex justify-content-between align-items-center">
        Danh sách đơn
        <button @click="handleShowModal(true, false)" class="btn btn-primary">Thêm đơn</button>
      </h5>
      <hr class="m-0" />
      <div class="card-body">
        <div class="row">
          <!-- Contextual List group -->
          <div :class="classApplicationSelected.title ? 'col-lg-6 mb-4 mb-xl-0' : 'col-lg-12 mb-4 mb-xl-0'">
            <small class="text-light fw-semibold">Danh sách đơn</small>
            <div class="demo-inline-spacing mt-3 list-container">
              <ul class="list-group" v-if="classApplication.length > 0">
                <li
                  v-for="application in classApplication"
                  :key="application.id"
                  @click="selectClassApplication(application.id)"
                  class="list-group-item d-flex justify-content-between align-items-center"
                  :class="{
                    'list-group-item-primary': selectedApplicationId === application.id,
                  }"
                >
                  <div class="d-flex flex-column" style="width: 50%; font-size: larger">
                    <span>{{ application.title }}</span>
                    <small class="text-muted">{{ truncatedDescription(application.description) }}</small>
                  </div>
                  <div class="d-flex flex-column align-items-end" style="width: 50%">
                    <small class="text-muted">{{ application.donorName }}</small>
                    <small>{{ formatDate(application.createDate, 'dd/mm/yyyy') }}</small>
                    <small
                      class="badge"
                      :class="
                        application.status === ApplicationStatus.PENDING
                          ? 'bg-label-dark'
                          : application.status === ApplicationStatus.REJECTED
                            ? 'bg-label-danger'
                            : 'bg-label-success'
                      "
                      style="width: 65px; font-size: 8px"
                    >
                      {{ application.statusName }}
                    </small>
                  </div>
                  <div class="btn-group" style="margin-left: 5px">
                    <button
                      type="button"
                      class="btn btn-icon  "
                      data-bs-toggle="dropdown"
                      aria-expanded="false"
                    >
                      <i class="bx bx-dots-vertical-rounded"></i>
                    </button>
                    <ul class="dropdown-menu dropdown-menu-end" style="position: absolute;">
                      <li>
                        <a class="dropdown-item" v-if="application.status === ApplicationStatus.PENDING" @click="handleShowModal(true, true)"
                        >Chỉnh sửa</a
                        >
                      </li>
                      <li>
                        <a class="dropdown-item" v-if="application.status === ApplicationStatus.PENDING" @click="handleDeleteModal(application.id)" href="javascript:void(0);"
                        >Xóa</a
                        >
                      </li>
                    </ul>
                  </div>
                </li>
              </ul>
              <div v-else class="text-center text-muted">Chưa có đơn nào</div>
            </div>
          </div>

          <div class="col-lg-6 mb-4 mb-xl-0" v-if="classApplicationSelected.title">
            <PupilApplicationDetailComponent :class-application="classApplicationSelected" />
          </div>
          <ModalCreateAndUpdateApplication
            :showModal="isShowModal"
            :isUpdate="isUpdate"
            :category="listCategory"
            :applicationDetail="isUpdate ? classApplicationSelected : null"
            @update-showModal="isShowModal = $event"
            @update-isUpdate="isUpdate = $event"
            @close="handleShowModal(false, false)"
            @update-list="updateListApplication"
            @update-errors="errorCreateAndUpdate = $event"
          />
          <ModalConfirmDeleteComponent
            :is-show-modal="isShowDeleteModal"
            :id="id"
            @confirm-action="handleDeleteConfirm"
            @close-modal="isShowDeleteModal = $event"
          >
          </ModalConfirmDeleteComponent>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from 'vue';
import PupilApplicationDetailComponent from '@/components/views/pupil/ApplicationDetail.vue';
import { usePupilApplicationComposable } from '@/composables/pupil-application';
import ModalCreateAndUpdateApplication from '@/components/modals/class-application/CreateAndUpdateApplication.vue';
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';
import { formatDate } from '@/helpers/formatDate';
import { truncatedDescription } from '@/helpers/truncated.ts';
import { ApplicationStatus } from '@/types/model/class-application.ts';
export default defineComponent({
  name: 'PupilApplication',
  computed: {
    ApplicationStatus() {
      return ApplicationStatus;
    },
  },
  components: {
    PupilApplicationDetailComponent,
    ModalCreateAndUpdateApplication,
    ModalConfirmDeleteComponent,
  },
  setup() {
    const {
      isShowDeleteModal,
      listCategory,
      getPupilApplication,
      fetchCurrentSemester,
      classApplication,
      currentSemester,
      classApplicationSelected,
      selectedApplicationId,
      handleSelectClassApplication,
      updateListApplication,
      isShowModal,
      isUpdate,
      handleShowModal,
      handleDelete,
      errorCreateAndUpdate,
      id,
      handleDeleteModal,
      handleDeleteConfirm,
    } = usePupilApplicationComposable();

    onMounted(async () => {
      await fetchCurrentSemester();
      await getPupilApplication(currentSemester.id);
      if (classApplication.value.length > 0) {
        handleSelectClassApplication(classApplication.value[0].id);
      }
    });

    return {
      id,
      handleDeleteModal,
      listCategory,
      classApplication,
      selectClassApplication: handleSelectClassApplication,
      classApplicationSelected,
      selectedApplicationId,
      updateListApplication,
      handleShowModal,
      handleDeleteConfirm,
      isShowModal,
      isUpdate,
      errorCreateAndUpdate,
      truncatedDescription,
      isShowDeleteModal,
      formatDate,
    };
  },
});
</script>

<style>
.demo-inline-spacing .list-group-item img {
  width: 100%;
  height: auto;
  flex-direction: row;
}

.demo-inline-spacing .list-group-item {
  display: flex;
  flex-direction: row;
  align-items: center;
  text-align: left;
}

.list-container {
  max-height: 400px;
  /* Adjust the height as needed */
  overflow-y: auto;
}

@media (max-width: 800px) {
  .list-container {
    max-height: calc(3 * 80px);
    /* Adjust the height to show approximately 3 notifications */
  }
}

.list-group-item-primary {
  border-color: aliceblue;
}
</style>