<template>
  <ModalComponent
    :modelValue="isShowPupilInBusStop"
    @update:modelValue="$emit('update:showModal', $event)"
    :width="`60%`"
  >
    <div class="modal-header">
      <h3>Danh sách học sinh</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã học sinh" clearable class="search-input" />
    <el-form class="table-container">
      <el-table :data="filteredPupils" class="pupil-table" :empty-text="'Không có học sinh nào trong điểm dừng này!'">
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
import { defineComponent, PropType, ref, computed, onMounted, watch } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { PupilDTOResponse } from '@/types/model/bus-enrollment';

export default defineComponent({
  name: 'ViewListPupil',
  components: {
    ModalComponent,
  },
  props: {
    isShowPupilInBusStop: {
      type: Boolean,
    },
    pupils: {
      type: Array as PropType<PupilDTOResponse[]>,
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
    const searchTerm = ref('');
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

    watch(
      () => props.isShowPupilInBusStop,
      (newValue, oldValue) => {
        if (newValue && newValue !== oldValue) {
          resetForm();
        }
      },
    );

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    // Close the modal without saving
    const closeModal = () => {
      if (props.isShowPupilInBusStop) {
        emit('update:showModal', false);
        resetForm();
      }
    };

    // Reset form and selections
    const resetForm = () => {
      searchTerm.value = '';
    };

    return {
      formatDate,
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>
