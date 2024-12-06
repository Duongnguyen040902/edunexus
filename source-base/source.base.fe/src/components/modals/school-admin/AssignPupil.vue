<template>
  <ModalComponent :modelValue="showAssignPupilModal" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <div class="modal-header">
      <h3>Danh sách học sinh</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã học sinh" clearable />
    <el-form class="table-container">
      <el-table :data="filteredPupils" class="pupil-table" empty-text="Không có học sinh chưa tham gia trong kì này!">
        <el-table-column label="Mã học sinh" prop="username" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }" />
        <el-table-column label="Tên học sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            {{ row.firstName + ' ' + row.lastName }}
          </template>
        </el-table-column>
        <el-table-column label="Giới tính" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            {{ row.gender ? 'Nam' : 'Nữ' }}
          </template>
        </el-table-column>
        <el-table-column label="Ngày sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            {{ formatDate(row.dateOfBirth) }}
          </template>
        </el-table-column>
        <el-table-column
          label="Chọn"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }"
        >
          <template #default="{ row }">
            <el-checkbox v-model="selectedStudentIds" :value="row.id" class="hidden-label"></el-checkbox>
          </template>
        </el-table-column>
      </el-table>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button" v-if="selectedStudentIds.length > 0"> Lưu </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ListPupilAssignRequestInterface } from '@/types/model/pupil';

export default defineComponent({
  name: 'AssignPupilModal',
  components: {
    ModalComponent,
  },
  props: {
    showAssignPupilModal: {
      type: Boolean,
    },
    pupils: {
      type: Array as PropType<ListPupilAssignRequestInterface[]>,
    },
    classId: {
      type: Number,
    },
    semesterId: {
      type: Number,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const selectedStudentIds = ref<number[]>([]);
    const searchTerm = ref('');
    const filteredPupils = computed(() => {
      return props.pupils.filter(
        pupil =>
          (pupil.firstName + ' ' + pupil.lastName).toLowerCase().includes(searchTerm.value.toLowerCase()) ||
          pupil.id.toString().includes(searchTerm.value),
      );
    });
    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    const handleModalConfirm = () => {
      const pupilsToAssign = selectedStudentIds.value.map(pupilId => ({
        pupilId: pupilId,
        classId: props.classId,
        semesterId: props.semesterId,
      }));
      emit('confirm', pupilsToAssign);
      emit('update:showModal', false);
      resetForm();
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      selectedStudentIds.value = [];
      searchTerm.value = '';
    };

    return {
      formatDate,
      selectedStudentIds,
      handleModalConfirm,
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>

<style scoped>
.modal-header h3 {
  font-size: 20px;
  font-weight: bold;
  color: #333;
  margin-bottom: 15px;
  text-align: center;
}

.table-container {
  overflow-y: auto;
  max-height: 300px;
}

.pupil-table {
  width: 100%;
  border: 1px solid #ebeef5;
}

.pupil-table th {
  background-color: #f5f5f5;
  color: #606b76;
  font-weight: bold;
  text-align: left;
}

.pupil-table th,
.pupil-table td {
  padding: 10px;
  border-bottom: 1px solid #ebeef5;
}

.pupil-table tr:hover {
  background-color: #f0f8ff;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  padding-top: 10px;
}

.custom-confirm-button {
  background-color: #409eff;
  color: #fff;
  border-radius: 4px;
  padding: 6px 15px;
}

.custom-confirm-button:hover {
  background-color: #66b1ff;
}
.search-input {
  margin-bottom: 15px;
  width: 40%;
}
.hidden-label .el-checkbox__label {
  display: none;
}
</style>
