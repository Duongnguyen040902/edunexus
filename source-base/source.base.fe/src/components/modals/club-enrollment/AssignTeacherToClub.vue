<template>
  <ModalComponent
    :modelValue="showAssignTeacherModal"
    @update:modelValue="$emit('update:showModal', $event)"
    :width="`50%`"
  >
    <div class="modal-header">
      <h3>Danh sách giáo viên</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã giáo viên" clearable class="search-input" />
    <el-form class="table-container">
      <el-table
        :data="filteredTeachers"
        class="teacher-table"
        empty-text="Không có giáo viên chưa tham gia trong kì này!"
      >
        <el-table-column label="Hình ảnh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <el-image
              :src="`${apiUrl}${row.image}`"
              alt="Hình ảnh giáo viên"
              style="width: 40px; height: 40px; object-fit: cover; border-radius: 50%"
            />
          </template>
        </el-table-column>
        <el-table-column label="Họ và tên" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.name }}</span>
          </template>
        </el-table-column>
        <el-table-column label="Số điện thoại" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
          <template #default="{ row }">
            <span>{{ row.username }}</span>
          </template>
        </el-table-column>
        <el-table-column
          label="Hành động"
          :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }"
        >
          <template #default="{ row }">
            <el-button type="primary" @click="handleModalConfirm(row.id)" class="assign-button">Phân công</el-button>
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
import { TeacherDTOResponse } from '@/types/model/club-enrollment';

export default defineComponent({
  name: 'AssignTeacherToClubModal',
  components: {
    ModalComponent,
  },
  props: {
    showAssignTeacherModal: {
      type: Boolean,
    },
    teachers: {
      type: Array as PropType<TeacherDTOResponse[]>,
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
    const searchTerm = ref('');
    const filteredTeachers = computed(() => {
      const teachers = Array.isArray(props.teachers.value) ? props.teachers.value : [];
      return teachers.filter(
        teacher =>
          (teacher.firstName + ' ' + teacher.lastName).toLowerCase().includes(searchTerm.value.toLowerCase()) ||
          teacher.id.toString().includes(searchTerm.value),
      );
    });

    const handleModalConfirm = (id: number) => {
      if (id) {
        const assignment = [
          {
            teacherId: id,
            clubId: props.clubId,
            semesterId: props.semesterId,
            status: 5,
          },
        ];
        emit('confirm', assignment);
        emit('update:showModal', false);
        resetForm();
      }
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      searchTerm.value = '';
    };

    return {
      searchTerm,
      filteredTeachers,
      handleModalConfirm,
      closeModal,
    };
  },
});
</script>
