<template>
  <ModalComponent :modelValue="showModal" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <el-form>
      <el-form-item v-if="teacherList.length > 0">
        <h3 class="teacher-title">Danh sách giáo viên</h3>

        <!-- Search Input -->
        <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã giáo viên" clearable />

        <!-- Teacher List Table -->
        <table class="teacher-table" empty-text="Không có giáo viên chưa tham gia trong kì này!">
          <thead>
            <tr>
              <th>Ảnh</th>
              <th>Mã giáo viên</th>
              <th>Họ và tên</th>
              <th>Hành động</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="teacher in filteredTeachers" :key="teacher.id">
              <td>
                <img :src="`${apiUrl}${teacher.image}`" alt=" " class="teacher-image" />
              </td>
              <td>{{ teacher.username }}</td>
              <td>{{ teacher.firstName }} {{ teacher.lastName }}</td>
              <td>
                <el-button type="primary" size="small" @click="assignTeacher(teacher)"> Phân công </el-button>
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
import { TeacherInterface } from '@/types/model/teacher';
import { ListTeacherAssignResponseInterface } from '@/types/model/teacher';

export default defineComponent({
  name: 'AssignTeacherToClass',
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
      type: Array as PropType<ListTeacherAssignResponseInterface[]>,
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
  emits: ['update:showModal', 'assignTeacher', 'modeModel'],
  setup(props, { emit }) {
    const searchTerm = ref('');
    const filteredTeachers = computed(() => {
      return props.teacherList.filter(teacher => {
        const search = searchTerm.value.toLowerCase();
        return (
          teacher.firstName?.toLowerCase().includes(search) ||
          teacher.lastName?.toLowerCase().includes(search) ||
          teacher.username?.toLowerCase().includes(search) 
        );
      });
    });

    const assignTeacher = (teacher: TeacherInterface) => {
      emit('assignTeacher', {
        classId: props.classId,
        teacherId: teacher.id,
        semesterId: props.semesterId,
      });
      emit('update:showModal', false);
      emit('modeModel', props.isUpdate);
    };

    return {
      assignTeacher,
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
