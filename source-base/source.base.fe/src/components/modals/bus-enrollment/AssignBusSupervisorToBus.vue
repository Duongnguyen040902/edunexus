<template>
  <ModalComponent
    :modelValue="showAssignSupervisorModal"
    @update:modelValue="$emit('update:showModal', $event)"
    :width="`60%`"
  >
    <div class="modal-header">
      <h3>Danh sách giáo viên giám sát</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã" clearable class="search-input" />
    <el-form class="table-container">
      <el-table :data="filteredSupervisors" class="supervisor-table" :empty-text="'Không có người giám sát nào trong kì này bị thiếu xe!'">
        <el-table-column label="Hình ảnh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <el-image
              :src="`${apiUrl}${row.image}`"
              alt="Hình ảnh"
              style="width: 40px; height: 40px; object-fit: cover; border-radius: 50%"
            />
          </template>
        </el-table-column>
        <el-table-column label="Họ và tên" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.firstName + ' ' + row.lastName }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Số điện thoại" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.phoneNumber }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Địa chỉ" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.address }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Giới tính" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.gender ? 'Nam' : 'Nữ' }}</span>
          </template>
        </el-table-column>
        <el-table-column
          label="Hành động"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }"
        >
          <template #default="{ row }">
            <el-button type="primary" @click="handleModalConfirm(row.id)" class="assign-button"> Phân công </el-button>
          </template>
        </el-table-column>
      </el-table>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { BusSupervisorDTOResponse } from '@/types/model/bus-enrollment';

export default defineComponent({
  name: 'AssignBusSupervisorModal',
  components: {
    ModalComponent,
  },
  props: {
    showAssignSupervisorModal: {
      type: Boolean,
    },
    supervisors: {
      type: Object as PropType<BusSupervisorDTOResponse[]>,
      default: () => [],
    },
    busId: {
      type: Number,
    },
    dataBusStop: {
      type: Object,
      default: () => [],
    },
    semesterId: {
      type: Number,
    },
    apiUrl: {
      type: Object,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const selectedSupervisorId = ref<number | null>(null);
    const searchTerm = ref('');
    const dataBusStopSelect = computed(() => props.dataBusStop.value.data || []);
    const filteredSupervisors = computed(() => {
      debugger
      if (!Array.isArray(props.supervisors.value.data)) {
        return [];
      }
      return props.supervisors.value.data.filter(
        supervisor =>
          (supervisor.firstName + ' ' + supervisor.lastName).toLowerCase().includes(searchTerm.value.toLowerCase()) ||
          supervisor.id.toString().includes(searchTerm.value),
      );
    });

    const handleModalConfirm = id => {
      debugger
      if (id !== null && props.dataBusStop) {
        const supervisorAssignment = [
          {
            busSupervisorId: id,
            busId: props.busId,
            semesterId: props.semesterId,
            busStopId: props.dataBusStop.value.data[0].id, 
          },
        ];

        emit('confirm', supervisorAssignment);
        emit('update:showModal', false);
        resetForm();
      }
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      selectedSupervisorId.value = null;
      searchTerm.value = '';
    };

    return {
      selectedSupervisorId,
      dataBusStopSelect,
      handleModalConfirm,
      filteredSupervisors,
      searchTerm,
      closeModal,
    };
  },
});
</script>
