<template>
  <ModalComponent :model-value="showModal" :title="isUpdate ? 'Cập nhật đơn' : 'Thêm Đơn'"
                  @update-modelValue="$emit('update-showModal', $event)" :before-close="handleCloseClick">
    <div class="modal-body">
      <div class="mt3" style="margin-top: 10px;">
        <label class="form-label">Tiêu đề (*):</label>
        <input class="form-control" v-model="localClassApplication.title"></input>
        <div v-if="errorCreateAndUpdate.Title" class="text-danger">
          {{ errorCreateAndUpdate.Title[0] }}
        </div>
      </div>
      <div class="mt3" style="margin-top: 10px;">
        <label class="form-label">Nội dung (*):</label>
        <textarea class="form-control" v-model="localClassApplication.description" rows="5"
                  style="height: 17px"></textarea>
        <div v-if="errorCreateAndUpdate?.Description" class="text-danger">
          {{ errorCreateAndUpdate.Description[0] }}
        </div>
      </div>
      <el-form class="mt3" style="margin-top: 20px;">
        <el-form-item label="Chọn loại đơn (*)">
          <el-select v-model="localClassApplication.applicationCategoryId" >
            <el-option v-for="item in category" :key="item.id" :label="item.name" :value="item.id">
            </el-option>
          </el-select>
        </el-form-item>
      </el-form>
      <div v-if="errorCreateAndUpdate?.ApplicationCategoryId" class="text-danger">
        {{ errorCreateAndUpdate.ApplicationCategoryId[0] }}
      </div>
    </div>
    <template #footer>
      <div class="dialog-footer">
        <button type="button" class="btn btn-outline-secondary" @click="handleCloseClick">
          Hủy
        </button>
        <button style="margin-left: 10px" type="button" @click="handleModalConfirm"
                class="btn btn-primary ml-3">
          Lưu
        </button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from "vue";
import ModalComponent from "@/components/common/Modal.vue";
import {
  ResponseGetClassApplication,
  RequestGetCategory, ApplicationCategory,
} from '@/types/model/class-application';
import { usePupilApplicationComposable } from "@/composables/pupil-application";

export default defineComponent({
  name: "ModalCreateAndUpdateApplication",
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    isUpdate: {
      type: Boolean,
    },
    category: {
      type: Array as PropType<RequestGetCategory[]>,
      required: true,
    },
    applicationDetail: {
      type: Object as PropType<ResponseGetClassApplication | null>,
    },
  },
  emits: ["update-showModal", "success", "update-errors", "update-list","update-isUpdate"],
  setup(props, { emit }) {
    const {
      handleClose,
      handleConfirm,
      errorCreateAndUpdate,
      currentSemester,
      fetchCurrentSemester,
      localClassApplication,
      initializeLocalClassApplication, // Di chuyển lên trước
    } = usePupilApplicationComposable();

    // Thiết lập lại localClassApplication khi applicationDetail thay đổi
    watch(
      () => props.applicationDetail,
      (newVal) => {
        initializeLocalClassApplication(newVal);
        if (!props.isUpdate) {
          localClassApplication.value = {
            title: '',
            description: '',
            applicationCategoryId: props.category.length > 0 ? props.category[0].id : null,
            semesterId: 0,
          };
        }
      },
      { immediate: true }
    );

    const handleModalConfirm = async () => {
      if (props.isUpdate) {
        await handleConfirm(localClassApplication.value as ResponseGetClassApplication, emit, props.isUpdate);
      } else {
        await fetchCurrentSemester();
        localClassApplication.value.semesterId = currentSemester.id;
        await handleConfirm(localClassApplication.value as ResponseGetClassApplication, emit, false);
      }
    };

    const handleCloseClick = async () => {
      localClassApplication.value = {
        title: "",
        description: "",
        applicationCategoryId: props.category.length > 0 ? props.category[0].id : null,
        semesterId: 0,
      };
      handleClose(emit);
    };

    return {
      handleCloseClick,
      errorCreateAndUpdate,
      handleModalConfirm,
      localClassApplication,
      handleClose,
    };
  },
});
</script>
