<template>
  <ModalComponent :modelValue="showUpSemester" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <div class="modal-header">
      <h3>Danh sách thành viên</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã" clearable class="search-input" />
    <el-form class="table-container">
      <el-table :data="filteredPupils" class="bus-enrollment-table" empty-text="Không có dữ liệu!">
        <!-- Mã học sinh hoặc giáo viên -->
        <el-table-column label="Mã số" prop="pupilCode" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span v-if="row.busSupervisorId" :style="row.busId ? { fontWeight: 'bold' } : {}">
              {{ row.busSupervisorCode }}</span
            >
            <span v-else :style="row.busId ? { fontWeight: 'bold' } : {}"> {{ row.pupilCode }}</span>
          </template>
        </el-table-column>

        <!-- Tên thành viên -->
        <el-table-column label="Tên thành viên" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.busId ? { fontWeight: 'bold' } : {}">
              <span v-if="row.busSupervisorId">{{ row.busSupervisorName }}</span>
              <span v-else>{{ row.pupilName }}</span>
            </span>
          </template>
        </el-table-column>

        <!-- Tên xe buýt -->
        <el-table-column label="Tên xe buýt" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.busId ? { fontWeight: 'bold' } : {}">
              {{ row.busName }}
            </span>
          </template>
        </el-table-column>

        <!-- Điểm dừng -->
        <el-table-column label="Điểm dừng" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.busId ? { fontWeight: 'bold' } : {}">
              {{ row.busStopName }}
            </span>
          </template>
        </el-table-column>

        <!-- Kỳ học -->
        <el-table-column label="Kỳ học" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.busId ? { fontWeight: 'bold' } : {}">
              {{ row.semesterName }}
            </span>
          </template>
        </el-table-column>

        <!-- Năm học -->
        <el-table-column label="Năm học" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.busId ? { fontWeight: 'bold' } : {}">
              {{ row.academicYear }}
            </span>
          </template>
        </el-table-column>
        <el-table-column
          label="Thành viên được chọn"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }"
        >
          <template #default="{ row }">
            <el-checkbox v-model="selectedPupilIds" :value="row.id" class="hidden-label"></el-checkbox>
          </template>
        </el-table-column>
      </el-table>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button">Xác nhận</el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ClubEnrollment, PupilDTOResponse } from '@/types/model/club-enrollment';
import { BusEnrollment } from '@/types/model/bus-enrollment';

export default defineComponent({
  name: 'UpSemesterBusModal',
  components: {
    ModalComponent,
  },
  props: {
    showUpSemester: {
      type: Boolean,
    },
    pupilInBus: {
      type: Array as PropType<BusEnrollment[]>,
      default: () => [],
    },
    pupilInBusNextSemester: {
      type: Array as PropType<BusEnrollment[]>,
      default: () => [],
    },
    busId: {
      type: Number,
    },
    semesterId: {
      type: Number,
    },
    apiUrl: {
      type: String,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const selectedPupilIds = ref<number[]>([]);
    const searchTerm = ref('');
    const selectedStatus = ref<number[]>([]);

    const notInNextSemester = computed(() => {
      const pupilInNext = Array.isArray(props.pupilInBusNextSemester) ? props.pupilInBusNextSemester : [];

      return Array.isArray(props.pupilInBus)
        ? props.pupilInBus.filter(pupil => {
            const isInNextSemester = pupilInNext.some(nextPupil => {
              if (pupil.busSupervisorId) {
                return nextPupil.busSupervisorId === pupil.busSupervisorId;
              } else if (pupil.pupilId) {
                return nextPupil.pupilId === pupil.pupilId;
              }
              return false;
            });
            return !isInNextSemester;
          })
        : [];
    });

    const filteredPupils = computed(() => {
      const searchQuery = searchTerm.value.toLowerCase();

      selectedPupilIds.value = notInNextSemester.value.map(pupil => pupil.id);
      // Kiểm tra xem pupilInClassNextSemester có phải là mảng không
      const pupilInNextSemester = Array.isArray(props.pupilInBusNextSemester) ? props.pupilInBusNextSemester : [];
      return props.pupilInBus
        .map(pupil => {
          // Kiểm tra xem học sinh có trong kỳ sau không
          const isInNextSemester = pupilInNextSemester.some(nextPupil => nextPupil.id === pupil.id);

          // Lọc theo tìm kiếm (search)
          const matchesSearch =
            pupil.busSupervisorName?.toLowerCase().includes(searchQuery) ||
            pupil.pupilName?.toLowerCase().includes(searchQuery) ||
            pupil.pupilCode?.toLowerCase().includes(searchQuery) ||
            (pupil.busSupervisorId ? 'người giám sát' : 'học sinh').includes(searchQuery);

          return {
            ...pupil,
            isInNextSemester, // Thêm thông tin để xác định xem học sinh có trong kỳ sau hay không
            matchesSearch, // Thêm thông tin để kiểm tra tìm kiếm
          };
        })
        .filter(pupil => pupil.matchesSearch); // Lọc theo tìm kiếm
    });

    const isPupilInNextSemester = row => {
      // Kiểm tra xem pupilInClassNextSemester có phải là mảng không
      const pupilInNextSemester = Array.isArray(props.pupilInBusNextSemester) ? props.pupilInBusNextSemester : [];

      // Kiểm tra xem học sinh có trong kỳ sau không (dựa trên pupilId)
      return pupilInNextSemester.some(nextPupil => nextPupil.pupilId === row.pupilId);
    };

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    const handleModalConfirm = () => {   
      const selectedPupils = filteredPupils.value.filter(pupil => selectedPupilIds.value.includes(pupil.id));

      const updatedPupils = selectedPupils.map(pupil => ({
        ...pupil,
        semesterId: props.semesterId,
      }));
      emit('confirm', updatedPupils);
      emit('update:showModal', false);
      resetForm();
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      searchTerm.value = '';
    };

    return {
      formatDate,
      selectedPupilIds,
      isPupilInNextSemester,
      handleModalConfirm,
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>
