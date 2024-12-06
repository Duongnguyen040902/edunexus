<template>
  <ModalComponent
    :modelValue="showModal"
    :title="isUpdateMode ? 'Cập nhật điểm' : 'Tạo hồ sơ điểm'"
    :width="'80%'"
    @update:modelValue="$emit('update:showModal', $event)"
    :before-close="handleClose"
  >
    <p>
        <div v-if="errorScore.score" class="text-danger">
            {{ errorScore.score[0] }}
        </div>
        </p>
    <div class="card">
      <div class="table-responsive text-nowrap">
        <table class="table">
          <thead>
            <tr>
              <th>STT</th>
              <th>Họ và Tên</th>
              <th>Ảnh</th>
              <th>Điểm</th>
            </tr>
          </thead>
          <tbody class="table-border-bottom-0">
            <tr v-for="(pupil, index) in Scores">
              <td>{{ index+1 }}</td>
              <td>
                <strong>{{ pupil.pupilName }}</strong>
              </td>
              <td>
                <ul
                  class="list-unstyled users-list m-0 avatar-group d-flex align-items-center v-infinite-scroll"
                >
                  <li
                    data-bs-toggle="tooltip"
                    data-popup="tooltip-custom"
                    data-bs-placement="top"
                    class="avatar avatar-xl pull-up"
                    :title="pupil.pupilName"
                  >
                    <img
                      :src="`${apiUrl}${pupil.image}`"
                      alt="Avatar"
                      class="rounded-circle"
                    />
                  </li>
                </ul>
              </td>
              <td>
                <div class=" mb-2">
                  <input class="form-control" type="number" v-model="pupil.score" />
                  
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button type="button" class="btn btn-outline-secondary" @click="handleClose">HỦY</button>
        <button
          style="margin-left: 10px"
          type="button"
          @click="handleModalConfirm"
          class="btn btn-primary ml-3"
        >
          {{ isUpdateMode ? "Cập nhật" : "Thêm" }}
        </button>
      </div>
    </template>
  </ModalComponent>
</template>
<script lang="ts">
import { useScoreComposable } from "@/composables/score";
import { ResponseGetScore } from "@/types/model/score";
import { defineComponent, Prop, PropType } from "vue";
import ModalComponent from "@/components/common/Modal.vue";
export default defineComponent({
  name: "CreateAndUpdateClassScoreComponent",
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    Scores: {
      type: Array as PropType<ResponseGetScore[]>,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
  },
  emits: [
    "update:showModal",
    "confirmUpdate",
    "confirm",
    "update:isUpdateMode",
    "refreshList",
  ],
  setup(props, { emit }) {
    const apiUrl = import.meta.env.VITE_APP_API_URL;
    const { errorScore, handleCloseModal, handleCreateScore, handleUpdateScore } = useScoreComposable();
    const handleModalConfirm = async () => {
      if (props.isUpdateMode) {
          await handleUpdateScore(props.Scores as ResponseGetScore[], emit);
      } else {
          await handleCreateScore(props.Scores as ResponseGetScore[], emit);
      }
    };
    const handleClose = () => {
      handleCloseModal(emit);
    };
    return { handleClose, handleModalConfirm, apiUrl ,errorScore};
  },
});
</script>
