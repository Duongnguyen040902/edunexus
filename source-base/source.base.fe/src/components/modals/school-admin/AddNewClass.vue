<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật thông tin lớp' : 'Thêm lớp'"
    @update:modelValue="$emit('update:showModal', $event)"
    :beforeClose="handleClose"
  >
    <el-form label-position="top">
      <!-- Chọn khối -->
      <el-form-item v-if="!isUpdateMode" label="Chọn khối *">
        <el-select v-model="localClass.block" placeholder="Chọn khối">
          <el-option v-for="grade in gradeOptions" :key="grade.value" :label="grade.label" :value="grade.value" />
        </el-select>
      </el-form-item>

      <!-- Tên lớp -->
      <el-form-item label="Nhập tên lớp *">
        <el-input v-model="localClass.name" placeholder="Nhập tên lớp (Ví dụ: A1)" />
        <div v-if="errorsAddNewClass.Name" class="text-danger">{{ errorsAddNewClass.Name[0] }}</div>
      </el-form-item>
    </el-form>

    <!-- Footer buttons -->
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="handleClose">HỦY</el-button>
        <el-button
          type="primary"
          @click="handleModalConfirm"
          class="custom-confirm-button"
          :disabled="localClass.block === 0 || !localClass.name"
        >
          {{ isUpdateMode ? 'Cập nhật' : 'Thêm' }}
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, watch, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import {
  AddNewClassRequestInterface,
  ErrorResponseAddNewClass,
  updateClassRequestInterface,
} from '@/types/model/class';
import { ViewClassAdminResponseInterface } from '@/types/model/admin-school';

export default defineComponent({
  name: 'CreateAndUpdateClass',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    dataClassDetail: {
      type: Object,
      required: true,
      default: [],
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    errorsAddNewClass: {
      type: Object as PropType<ErrorResponseAddNewClass>,
      required: true,
    },
  },
  emits: ['update:showModal', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const localClass = ref<updateClassRequestInterface>({
      id: 0,
      name: '',
      block: '1',
    });
    const id = ref<number>();
    const gradeOptions = ref([
      { label: 'Khối 1', value: '1' },
      { label: 'Khối 2', value: '2' },
      { label: 'Khối 3', value: '3' },
      { label: 'Khối 4', value: '4' },
      { label: 'Khối 5', value: '5' },
    ]);

    watch(
      () => props.showModalAdd,
      newValue => {
        if (!newValue) {
          handleResetForm();
          localClass.value = { id: 0, name: '', block: '1' };
        }
      },
    );

    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.dataClassDetail.value) {
          localClass.value.id = props.dataClassDetail.value.id;
          localClass.value.name = props.dataClassDetail.value.className.substring(1);
          localClass.value.block = props.dataClassDetail.value.block;
        } else {
          localClass.value = { id: 0, name: '', block: '1' };
        }
      },
      { immediate: true },
    );

    const handleModalConfirm = () => {
      handleResetForm();
      emit('confirm', localClass.value);
    };

    const handleClose = () => {
      handleResetForm();
      localClass.value = { id: 0, name: '', block: '1' };
      emit('update:showModal', false);
      emit('closeUpdateModal', false);
    };

    const handleResetForm = () => {
      props.errorsAddNewClass.Name = [];
    };

    return {
      localClass,
      gradeOptions,
      handleModalConfirm,
      handleClose,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  display: flex;
  justify-content: flex-end;
  padding: 10px 0;
  border-top: 1px solid #e8e8e8;
  margin-top: 20px;
}

.dialog-footer .el-button {
  margin-left: 10px;
}

.custom-confirm-button {
  background-color: #409eff;
  border-color: #409eff;
  color: #fff;
}

.custom-confirm-button:hover {
  background-color: #66b1ff;
  border-color: #66b1ff;
  color: #fff;
}

.text-danger {
  color: #f56c6c;
  font-size: 14px;
  margin-top: 5px;
}
</style>
