<template>
  <ModalComponent :modelValue="showUpLevel" @update:modelValue="$emit('update:showModal', $event)" :width="`60%`">
    <div class="modal-header">
      <h3>Danh sách thành viên</h3>
    </div>
    <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã" clearable class="search-input" />
    <el-form v-if="checkUpClass && listClass.value && Object.keys(listClass.value).length" class="class-selection">
      <el-select v-model="selectedClassId" placeholder="Chọn lớp lên lớp" clearable>
        <el-option
          v-for="(classItem, key) in listClass.value"
          :key="key"
          :label="classItem.className"
          :value="classItem.id"
        ></el-option>
      </el-select>
    </el-form>
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
            <el-tooltip
              v-if="isPupilInNextSemester(row)"
              class="item"
              effect="dark"
              :content="row.teacherId ? 'Không thể lên lớp cho giáo viên!':'Học sinh hoặc giáo viên này đã được thêm vào lớp học trong kỳ tiếp theo!'"
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
        <el-button
          type="primary"
          @click="handleModalConfirm"
          class="custom-confirm-button"
          v-if="!checkUpClass || (checkUpClass && selectedClassId !== null)"
        >
          Xác nhận
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed, onMounted } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { BusEnrollment } from '@/types/model/bus-enrollment';
import { MemberInClassDTO } from '@/types/model/class-enrollment';

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
    pupilInClassNextSemester: {
      type: Array as PropType<MemberInClassDTO[]>,
      default: () => [],
    },
    classId: {
      type: Number,
    },
    checkUpClass: {
      type: Boolean,
    },
    listClass: {
      type: Array,
    },
    semesterId: {
      type: Number,
    },
  },
  emits: ['update:showModal', 'confirm'],
  setup(props, { emit }) {
    const searchTerm = ref('');
    const selectedPupilIds = ref<number[]>([]);
    const selectedClassId = ref<number | null>(null);
    const notInNextSemester = computed(() => {
      const pupilInNext = Array.isArray(props.pupilInClassNextSemester) ? props.pupilInClassNextSemester : [];

      return Array.isArray(props.pupilInClass)
        ? props.pupilInClass.filter(pupil => {
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

    const alreadyInNextSemester = computed(() => {
      const pupilInNext = Array.isArray(props.pupilInClassNextSemester) ? props.pupilInClassNextSemester : [];
      return Array.isArray(props.pupilInClass)
        ? props.pupilInClass.filter(pupil => {
            return pupilInNext.some(nextPupil => {
              if (pupil.teacherId) {
                return nextPupil.teacherId === pupil.teacherId;
              } else if (pupil.pupilId) {
                return nextPupil.pupilId === pupil.pupilId;
              }
              return false;
            });
          })
        : [];
    });

    const filteredPupils = computed(() => {
      const searchQuery = searchTerm.value.toLowerCase();

      selectedPupilIds.value = notInNextSemester.value.map(pupil => pupil.id);
      // Kiểm tra xem pupilInClassNextSemester có phải là mảng không
      const pupilInNextSemester = Array.isArray(props.pupilInClassNextSemester) ? props.pupilInClassNextSemester : [];
      return props.pupilInClass
        .map(pupil => {
          // Kiểm tra xem học sinh có trong kỳ sau không
          const isInNextSemester = pupilInNextSemester.some(nextPupil => nextPupil.id === pupil.id);

          // Lọc theo tìm kiếm (search)
          const matchesSearch =
            pupil.teacherName?.toLowerCase().includes(searchQuery) ||
            pupil.pupilName?.toLowerCase().includes(searchQuery) ||
            pupil.teacherCode?.toLowerCase().includes(searchQuery) ||
            pupil.pupilCode?.toLowerCase().includes(searchQuery) ||
            (pupil.teacherCode ? 'giáo viên' : 'học sinh').includes(searchQuery);

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
      const pupilInNextSemester = Array.isArray(props.pupilInClassNextSemester) ? props.pupilInClassNextSemester : [];

      if(row.teacherId != null){
        return true;
      }
      return pupilInNextSemester.some(nextPupil => nextPupil.pupilId === row.pupilId);
    };

    onMounted(() => {
      selectedPupilIds.value = notInNextSemester.value.map(pupil => pupil.id);
    });

    const handleModalConfirm = () => {
      const targetClassId = props.checkUpClass ? selectedClassId.value : props.classId;
      // Lọc ra danh sách các `pupil` được chọn dựa trên `selectedPupilIds`
      const selectedPupils = props.pupilInClass.filter(pupil => {
        // So sánh `teacherId` hoặc `pupilId` tùy thuộc vào loại đối tượng
        if (pupil.teacherId) {
          return selectedPupilIds.value.includes(pupil.id);
        } else if (pupil.pupilId) {
          return selectedPupilIds.value.includes(pupil.id);
        }
        return false;
      });

      // Cập nhật `semesterId` cho danh sách đã chọn
      const updatedPupils = selectedPupils.map(pupil => ({
        ...pupil,
        semesterId: props.semesterId,
      }));

      // Lọc ra giáo viên từ danh sách đã chọn và tạo payload
      const teachers = updatedPupils.filter(pupil => pupil.teacherId); // Chỉ chọn giáo viên

      const request = teachers.map(teacher => ({
        classId: targetClassId,
        teacherId: teacher.teacherId,
        semesterId: props.semesterId,
      }));

      // Lọc ra học sinh từ danh sách đã chọn và tạo payload
      const students = updatedPupils
        .filter(pupil => pupil.pupilId) // Chỉ chọn học sinh
        .map(student => ({
          pupilId: student.pupilId, // Sử dụng `pupilId`
          classId: targetClassId,
          semesterId: props.semesterId,
        }));

      // Gửi dữ liệu đã xử lý qua các sự kiện
      emit('confirm', request, students);
      emit('update:showModal', false);
      resetForm();
    };

    const closeModal = () => {
      emit('update:showModal', false);
      resetForm();
    };

    const resetForm = () => {
      searchTerm.value = '';
      selectedClassId.value = null;
    };

    return {
      searchTerm,
      isPupilInNextSemester,
      filteredPupils,
      selectedPupilIds,
      handleModalConfirm,
      selectedClassId,
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
