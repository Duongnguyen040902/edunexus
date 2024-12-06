<template>
  <ModalComponent
    :modelValue="showConfirmPupilToClub"
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
        empty-text="Không có học sinh đăng kí tham gia câu lạc bộ trong kì này!"
      >
        <el-table-column
          label="Mã học sinh"
          prop="username"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }"
        >
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}">
              {{ row.pupilUsername }}
              <span v-if="row.clubId" style="color: red; font-size: 16px">*</span>
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Tên học sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}">
              {{ row.pupilName }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Trạng thái đơn" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}"> Đăng ký </span>
          </template>
        </el-table-column>
        <el-table-column label="Ngày đăng kí" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span :style="row.clubId ? { fontWeight: 'bold' } : {}">
              {{ formatDate(row.createdDate) }}
            </span>
          </template>
        </el-table-column>
        <el-table-column label="Duyệt đơn" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <el-select v-model="row.status" placeholder="Chọn trạng thái" size="small" style="width: 120px">
              <el-option :label="'Lựa chọn'" :value="1" />
              <el-option :label="'Chấp nhận'" :value="2" />
              <el-option :label="'Từ chối'" :value="3" />
            </el-select>
          </template>
        </el-table-column>
      </el-table>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="closeModal()">HỦY</el-button>
        <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button" :disabled="filteredPupils.every(pupil => pupil.status === 1)"> Lưu </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ClubEnrollment, PupilDTOResponse } from '@/types/model/club-enrollment';
import { StatusClubEnrollment } from '@/constants/enums/statuses';

export default defineComponent({
  name: 'AssignPupilToClubModal',
  components: {
    ModalComponent,
  },
  props: {
    showConfirmPupilToClub: {
      type: Boolean,
    },
    pupilRegisterClub: {
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
    const message = ref();
    const filteredPupils = computed(() => {
      if (props.pupilRegisterClub) {
        const pupils = Array.isArray(props.pupilRegisterClub.value) ? props.pupilRegisterClub.value : [];
        return pupils.filter(
          pupil =>
            pupil.pupilName.toLowerCase().includes(searchTerm.value.toLowerCase()) ||
            pupil.id.toString().includes(searchTerm.value) ||
            pupil.pupilUsername.toLowerCase().includes(searchTerm.value.toLowerCase()),
        );
      }
      return [];
    });

    const formatDate = (dateString: string) => {
      const date = new Date(dateString);
      return date.toLocaleDateString('vi-VN');
    };

    const handleModalConfirm = () => {
      const pupilsToUpdate = filteredPupils.value.map(pupil => ({
        id: pupil.id,
        pupilId: pupil.pupilId,
        clubId: props.clubId,
        semesterId: props.semesterId,
        status: pupil.status,
      }));
      emit('confirm', pupilsToUpdate);
      emit('update:showModal', false);
      resetForm();
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      selectedPupilIds.value = [];
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
