<template>
  <ModalComponent :modelValue="showUpLevel" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <div class="modal-header">
      <h3>Danh sách thành viên</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã" clearable class="search-input" />
    <el-form class="table-container">
      <el-table :data="filteredPupils" class="bus-enrollment-table" empty-text="Không có dữ liệu!">
        <!-- Ảnh -->
        <el-table-column label="Ảnh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <img :src="row.avatarUrl" alt="Avatar" class="member-avatar" />
          </template>
        </el-table-column>

        <!-- Họ và tên -->
        <el-table-column label="Họ và tên" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.teacherCode ? row.teacherName : row.pupilName }}</span>
          </template>
        </el-table-column>

        <!-- Chức vụ -->
        <el-table-column label="Chức vụ" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.teacherCode ? 'Giáo viên' : 'Học sinh' }}</span>
          </template>
        </el-table-column>

        <!-- Mã số -->
        <el-table-column label="Mã số" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.teacherCode ? row.teacherCode : row.pupilCode }}</span>
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
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button"> Xác nhận </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed, onMounted } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { BusEnrollment } from '@/types/model/bus-enrollment';
import { MemberInClassDTO } from '@/types/model/class-enrollment';
import { id } from 'element-plus/es/locale';

export default defineComponent({
  name: 'UpSemesterBusModal',
  components: {
    ModalComponent,
  },
  props: {
    showUpLevel: {
      type: Boolean,
    },
    pupilInClass: {
      type: Array as PropType<MemberInClassDTO[]>,
      default: () => [],
    },
    semesterId: {
      type: Number,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const selectedPupilIds = ref<number[]>([]);
    const searchTerm = ref('');

    const filteredPupils = computed(() => {
      if (props.pupilInClass) {
        const pupils = Array.isArray(props.pupilInClass) ? props.pupilInClass : [];
        selectedPupilIds.value = props.pupilInClass
          .filter(pupil => pupil.pupilId != null) // Chỉ chọn các học sinh có pupilId khác null
          .map(pupil => pupil.id); // Lấy danh sách id của các học sinh
        return pupils.filter(pupil => {
          const searchQuery = searchTerm.value.toLowerCase();
          const matchesPupilName = pupil.pupilName && pupil.pupilName.toLowerCase().includes(searchQuery);
          const matchesTeacherName = pupil.teacherName && pupil.teacherName.toLowerCase().includes(searchQuery);
          const matchesId = pupil.id.toString().includes(searchTerm.value);
          const matchesPupilUsername = pupil.pupilCode && pupil.pupilCode.toLowerCase().includes(searchQuery);
          const hasValidPupilId = pupil.pupilId !== null;

          return hasValidPupilId && (matchesPupilName || matchesTeacherName || matchesId || matchesPupilUsername);
        });
      }
      return [];
    });

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    const handleModalConfirm = () => {
      // Lọc ra các học sinh được chọn
      const selectedPupils = props.pupilInClass.filter(pupil => selectedPupilIds.value.includes(pupil.id));

      // Lấy danh sách pupilId của các học sinh được chọn (chuyển sang số nguyên)
      const pupilIds = selectedPupils
        .filter(pupil => pupil.pupilId != null) // Đảm bảo chỉ lấy học sinh có pupilId hợp lệ
        .map(pupil => parseInt(pupil.pupilId, 10)); // Chuyển pupilId sang kiểu số nguyên

      // Phát sự kiện confirm với mảng pupilIds
      emit('confirm', pupilIds);

      // Đóng modal
      emit('update:showModal', false);

      // Reset form
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
      filteredPupils,
      searchTerm,
      closeModal,
    };
  },
});
</script>

<style scoped>
.member-avatar {
  width: 50px;
  height: 50px;
  border-radius: 50%;
  object-fit: cover;
}
</style>
