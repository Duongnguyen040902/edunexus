<template>
  <ModalComponent
    :modelValue="showAssignPupilModal"
    @update:modelValue="$emit('update:showModal', $event)"
    :width="`60%`"
  >
    <div class="modal-header">
      <h3>Danh sách học sinh</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã học sinh" clearable class="search-input" />
    <el-form class="table-container">
      <el-table
        :data="filteredPupils"
        class="pupil-table"
        :empty-text="'Không có học sinh nào trong kì này bị thiếu xe!'"
      >
        <el-table-column label="Hình ảnh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <el-image
              :src="`${apiUrl}${row.image}`"
              alt="Hình ảnh học sinh"
              style="width: 40px; height: 40px; object-fit: cover; border-radius: 50%"
            />
          </template>
        </el-table-column>

        <el-table-column
          label="Mã học sinh"
          prop="username"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }"
        >
          <template #default="{ row }">
            <span :style="row.supervisorId ? { color: 'green', fontWeight: 'bold' } : {}">
              {{ row.username }}
              <span v-if="row.supervisorId" style="color: red; font-size: 16px">*</span>
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Tên học sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.supervisorId ? { color: 'green', fontWeight: 'bold' } : {}">
              {{ row.firstName + ' ' + row.lastName }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Giới tính" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.supervisorId ? { color: 'green', fontWeight: 'bold' } : {}">
              {{ row.gender ? 'Nam' : 'Nữ' }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Ngày sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.supervisorId ? { color: 'green', fontWeight: 'bold' } : {}">
              {{ formatDate(row.dateOfBirth) }}
            </span>
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

    <!-- Chọn điểm dừng cho tất cả học sinh đã chọn -->
    <el-form-item v-if="dataBusStopSelect.length > 0" label="Chọn điểm dừng">
      <el-select v-model="selectedBusStop" placeholder="Chọn điểm dừng" :disabled="selectedStudentIds.length === 0">
        <el-option v-for="busStop in dataBusStopSelect" :key="busStop.id" :label="busStop.name" :value="busStop.id" />
      </el-select>
    </el-form-item>
    <el-form-item v-else label="Thông báo:">
      <p>Tuyến xe chưa có điểm dừng ! </p>
    </el-form-item>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
        <el-button
          type="primary"
          @click="handleModalConfirm"
          class="custom-confirm-button"
          :disabled="selectedBusStop === null || selectedStudentIds.length === 0"
        >
          Lưu
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>
<script lang="ts">
import { defineComponent, PropType, ref, computed, onMounted, watch } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { PupilDTOResponse } from '@/types/model/bus-enrollment';

export default defineComponent({
  name: 'BusEnrollmentModal',
  components: {
    ModalComponent,
  },
  props: {
    showAssignPupilModal: {
      type: Boolean,
    },
    pupils: {
      type: Array as PropType<PupilDTOResponse[]>,
      default: () => [],
    },
    busId: {
      type: Number,
    },
    semesterId: {
      type: Number,
    },
    dataBusStop: {
      type: Object,
      default: () => [],
    },
    apiUrl: {
      type: Object,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const selectedStudentIds = ref<number[]>([]);
    const searchTerm = ref('');
    const selectedBusStop = ref<number | null>(null);
    const dataBusStopSelect = computed(() => props.dataBusStop.value.data || []);
    const filteredPupils = computed(() => {    
      if (props.pupils && props.pupils.value) {
        const pupils = Array.isArray(props.pupils.value.data) ? props.pupils.value.data : [];
        return pupils.filter(
          pupil =>
            (pupil.firstName + ' ' + pupil.lastName).toLowerCase().includes(searchTerm.value.toLowerCase()) ||
            pupil.id.toString().includes(searchTerm.value),
        );
      }
      return [];
    });

    watch(dataBusStopSelect, newData => {
      if (newData.length > 0 && selectedBusStop.value === null) {
        selectedBusStop.value = newData[0].id;
      }
    });

    watch(
            () => props.showAssignPupilModal,
            (newValue) => {
                if (!newValue) {
                    resetForm();
                }
            }
        );

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    const handleModalConfirm = () => {
      const pupilsToEnroll = selectedStudentIds.value.map(pupilId => ({
        pupilId: pupilId,
        busId: props.busId,
        semesterId: props.semesterId,
        busStopId: selectedBusStop.value,
      }));
      emit('confirm', pupilsToEnroll);
      emit('update:showModal', false);
      resetForm();
    };

    // Close the modal without saving
    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    // Reset form and selections
    const resetForm = () => {
      selectedStudentIds.value = [];
      searchTerm.value = '';
      selectedBusStop.value = null;
    };

    return {
      formatDate,
      selectedBusStop,
      dataBusStopSelect,
      selectedStudentIds,
      handleModalConfirm,
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>
