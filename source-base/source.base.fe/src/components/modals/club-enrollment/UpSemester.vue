<template>
  <ModalComponent :modelValue="showUpSemester" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <div class="modal-header">
      <h3>Danh sách thành viên</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã" clearable class="search-input" />
    <el-form class="table-container">
      <el-table :data="filteredPupils" class="pupil-table" empty-text="Không có dữ liệu!">
        <el-table-column label="Mã số" prop="username" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span v-if="row.teacherId" :style="row.clubId ? { fontWeight: 'bold' } : {}">
              {{ row.teacherUsername }}</span
            >
            <span v-else :style="row.clubId ? { fontWeight: 'bold' } : {}"> {{ row.pupilUsername }}</span>
            <span v-if="row.clubId" style="color: red; font-size: 16px">*</span>
          </template>
        </el-table-column>

        <el-table-column label="Tên thành viên" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}">
              <span v-if="row.teacherId">{{ row.teacherName }}</span>
              <span v-else>{{ row.pupilName }}</span>
            </span>
          </template>
        </el-table-column>

        <el-table-column label="Ngày tham gia" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}">
              {{ formatDate(row.createdDate) }}
            </span>
          </template>
        </el-table-column>
        <el-table-column
          label="Thành viên được chọn"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }"
        >
          <template #default="{ row }">
            <el-tooltip
              v-if="isPupilInNextSemester(row)"
              class="item"
              effect="dark"
              :content="'Học sinh hoặc giáo viên này đã được thêm vào lớp học trong kỳ tiếp theo!'"
              placement="top"
            >
              <el-checkbox :value="row.id" class="hidden-label" disabled></el-checkbox>
              <!-- Hiển thị checkbox vô hiệu hóa -->
            </el-tooltip>
            <el-checkbox v-else v-model="selectedPupilIds" :value="row.id" class="hidden-label"></el-checkbox>
          </template>
        </el-table-column>
      </el-table>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button" v-if="selectedPupilIds.length > 0">Xác nhận</el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed, onMounted } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ClubEnrollment, PupilDTOResponse } from '@/types/model/club-enrollment';
import { StatusClubEnrollment } from '@/constants/enums/statuses';

export default defineComponent({
  name: 'UpSemesterClubModal',
  components: {
    ModalComponent,
  },
  props: {
    showUpSemester: {
      type: Boolean,
    },
    pupilInClub: {
      type: Array as PropType<ClubEnrollment[]>,
      default: () => [],
    },
    pupilInClubNextSemester: {
      type: Array as PropType<ClubEnrollment[]>,
      default: () => [],
    },
    clubId: {
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

      const pupilInNext = Array.isArray(props.pupilInClubNextSemester) ? props.pupilInClubNextSemester : [];

      return Array.isArray(props.pupilInClub)
        ? props.pupilInClub.filter(pupil => {
            const isInNextSemester = pupilInNext.some(nextPupil => {
              if (pupil.teacherId) {
                return nextPupil.teacherId === pupil.teacherId;
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
      const pupilInNextSemester = Array.isArray(props.pupilInClubNextSemester) ? props.pupilInClubNextSemester : [];
      return props.pupilInClub
        .map(pupil => {
          // Kiểm tra xem học sinh có trong kỳ sau không
          const isInNextSemester = pupilInNextSemester.some(nextPupil => nextPupil.id === pupil.id);

          // Lọc theo tìm kiếm (search)
          const matchesSearch =
            pupil.teacherName?.toLowerCase().includes(searchQuery) ||
            pupil.pupilName?.toLowerCase().includes(searchQuery) ||
            pupil.teacherUsername?.toLowerCase().includes(searchQuery) ||
            pupil.pupilUsername?.toLowerCase().includes(searchQuery) ||
            (pupil.teacherUsername ? 'giáo viên' : 'học sinh').includes(searchQuery);

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
      const pupilInNextSemester = Array.isArray(props.pupilInClubNextSemester) ? props.pupilInClubNextSemester : [];

      // Kiểm tra xem học sinh có trong kỳ sau không (dựa trên pupilId)
      return pupilInNextSemester.some(nextPupil => nextPupil.pupilId === row.pupilId);
    };

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    onMounted(() => {
      selectedPupilIds.value = notInNextSemester.value.map(pupil => pupil.id);
    });
    
    const handleModalConfirm = () => {
      const selectedPupils = props.pupilInClub.filter(pupil => selectedPupilIds.value.includes(pupil.id));

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
      handleModalConfirm,
      isPupilInNextSemester,
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>
