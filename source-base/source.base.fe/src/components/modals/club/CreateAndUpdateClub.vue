<template>
  <ModalComponent
    :modelValue="showModalAdd"
    :title="isUpdateMode ? 'Cập nhật câu lạc bộ' : 'Thêm câu lạc bộ'"
    @update:modelValue="$emit('update:showModal', $event)"
    :beforeClose="handleClose"
  >
    <el-form label-position="top">
      <!-- Tên câu lạc bộ -->
      <el-form-item label="Tên câu lạc bộ *"> 
        <el-input v-model="localClub.name" placeholder="Nhập tên câu lạc bộ" />
        <div v-if="clubErrors.Name" class="text-danger">{{ clubErrors.Name[0] }}</div>
      </el-form-item>

      <!-- Mô tả -->
      <el-form-item label="Mô tả *">
        <el-input type="textarea" v-model="localClub.description" placeholder="Nhập mô tả câu lạc bộ" :rows="3"/>
        <div v-if="clubErrors.Description" class="text-danger">{{ clubErrors.Description[0] }}</div>
      </el-form-item>

      <!-- Trạng thái -->
      <el-form-item label="Trạng thái">
        <el-select v-model="localClub.status" placeholder="Chọn trạng thái">
          <el-option label="Hoạt động" :value="1" />
          <el-option label="Không hoạt động" :value="0" />
        </el-select>
        <div v-if="clubErrors.Status" class="text-danger">{{ clubErrors.Status[0] }}</div>
      </el-form-item>
    </el-form>

    <!-- Footer buttons -->
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="handleClose">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button">
          {{ isUpdateMode ? 'Cập nhật' : 'Thêm' }}
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, watch, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ErrorResponseCreateClub, RequestCreateClubInterface } from '@/types/model/club';

export default defineComponent({
  name: 'CreateAndUpdateClub',
  components: {
    ModalComponent,
  },
  props: {
    showModalAdd: {
      type: Boolean,
      required: true,
    },
    dataClubDetail: {
      type: Object as PropType<RequestCreateClubInterface>,
      required: true,
      default: [],
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    clubErrors: {
      type: Object as PropType<ErrorResponseCreateClub>,
      required: true,
    },
  },
  emits: ['update:showModal', 'update:club', 'confirm', 'closeUpdateModal'],
  setup(props, { emit }) {
    const localClub = ref<RequestCreateClubInterface>({
      name: '',
      description: '',
      status: 1,
    });
    watch(
      () => props.showModalAdd,
      newValue => {
        if (!newValue) {
          // Reset form and errors when the modal closes.
          localClub.value = {
            name: '',
            description: '',
            status: 1,
          };
          handleResetForm();
        }
      },
    );
    watch(
      () => props.isUpdateMode,
      newValue => {
        if (newValue && props.dataClubDetail.value) {
          localClub.value = { ...props.dataClubDetail.value };
        } else {
          localClub.value = {
            name: '',
            description: '',
            status: 1,
          };
        }
      },
      { immediate: true },
    );

    const handleModalConfirm = () => {
      handleResetForm();
      emit('confirm', localClub.value);
    };

    const handleClose = () => {
      handleResetForm()
        localClub.value = {
          name: '',
          description: '',
          status: 1,
        };
        emit('update:showModal', false);
        emit('closeUpdateModal', false);
    };

    const handleResetForm = () => {
      props.clubErrors.Name = [];
      props.clubErrors.Description = [];
      props.clubErrors.Status = [];
    };
    return {
      localClub,
      handleClose,
      handleModalConfirm,
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
</style>
