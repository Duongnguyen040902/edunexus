<template>
  <ModalComponent :modelValue="showModal" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <el-form>
      <el-form-item v-if="teacherList.length > 0">
        <h3 class="teacher-title">Danh sách giáo viên</h3>

        <!-- Search Input -->
        <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã giáo viên" clearable />

        <!-- Teacher List Table -->
        <table class="teacher-table">
          <thead>
            <tr>
              <th>Ảnh</th>
              <th>Mã giáo viên</th>
              <th>Họ và tên</th>
              <th>Lớp học đang giảng dạy</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="teacher in filteredTeachers" :key="teacher.classEnrollmentId">
              <td>
                <img :src="`${apiUrl}${teacher.image}`" alt=" " class="teacher-image" />
              </td>
              <td>{{ teacher.userName }}</td>
              <td>{{ teacher.teacherName }}</td>
              <td>{{ teacher.className}}</td>
              <td>
                <el-button type="primary" size="small" @click="swapTeacher(teacher)"> Đổi </el-button>
              </td>
            </tr>
          </tbody>
        </table>
      </el-form-item>
      <p v-else>Không có giáo viên nào để hiển thị.</p>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="$emit('update:showModal', false)">ĐÓNG</el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { ResponseTeacherSwapInterface } from '@/types/model/class-enrollment';

export default defineComponent({
  name: 'SwapTeacher',
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
    },
    isUpdate: {
      type: Boolean,
    },
    teacherList: {
      type: Array as PropType<ResponseTeacherSwapInterface[]>,
    },
    classId: {
      type: Number,
    },
    semesterId: {
      type: Number,
    },
    apiUrl: {
      type: Object,
    },
  },
  emits: ['update:showModal', 'swapTeacher', 'modeModel'],
  setup(props, { emit }) {
    const searchTerm = ref('');
    const filteredTeachers = computed(() => {
      return props.teacherList.filter(teacher => {
        const search = searchTerm.value.toLowerCase();
        return (
          teacher.teacherName?.toLowerCase().includes(search) ||
          teacher.userName?.toLowerCase().includes(search)
        );
      });
    });

    const swapTeacher = (teacher: ResponseTeacherSwapInterface) => {
      emit('swapTeacher', {
        classEnrollmentId: teacher.classEnrollmentId,
      });
      emit('update:showModal', false);
    };

    return {
      swapTeacher,
      searchTerm,
      filteredTeachers,
    };
  },
});
</script>

<style scoped>
.teacher-title {
  font-size: 18px;
  font-weight: bold;
  margin-bottom: 10px;
}

.teacher-table {
  width: 100%;
  border-collapse: collapse;
  margin-top: 10px;
}

.teacher-table th,
.teacher-table td {
  padding: 8px 12px;
  border: 1px solid #e0e0e0;
  text-align: left;
}

.teacher-table th {
  background-color: #f5f5f5;
  font-weight: 600;
  color: #333;
}

.teacher-table tr:hover {
  background-color: #f0f8ff;
}

.dialog-footer {
  display: flex;
  justify-content: flex-end;
  padding: 10px 0;
  border-top: 1px solid #e0e0e0;
  margin-top: 20px;
}

.el-button {
  border-radius: 4px;
}
.teacher-image {
  width: 50px;
  height: auto;
}
</style>
