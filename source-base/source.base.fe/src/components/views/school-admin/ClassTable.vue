<template>
  <div class="content-wrapper">
    <div class="container-xxl flex-grow-1 container-p-y">
      <h4 class="fw-bold py-3 mb-4"><span class="text-muted fw-light"></span> Quản lí lớp học</h4>
      <div v-if="classForSchoolAdmin.value" data-v-d42219ef="" class="row text-nowrap">
        <div data-v-d42219ef="" class="col-md-6 mb-6">
          <div data-v-d42219ef="" class="card h-100">
            <div data-v-d42219ef="" class="card-body">
              <div data-v-d42219ef="" class="card-icon mb-2">
                <div data-v-d42219ef="" class="avatar">
                  <div data-v-d42219ef="" class="avatar-initial rounded bg-label-primary">
                    <i data-v-d42219ef="" class="bx bx-time bx-lg"></i>
                  </div>
                </div>
              </div>
              <div data-v-d42219ef="" class="card-info">
                <h5 data-v-d42219ef="" class="card-title mb-2">
                  Tổng số lớp học: {{ classForSchoolAdmin.value.data.totalRecords }}
                </h5>
                <div data-v-d42219ef="" class="d-flex align-items-baseline gap-1">
                  <h5 data-v-d42219ef="" class="text-primary mb-0"></h5>
                </div>
                <p data-v-d42219ef="" class="mb-0 text-truncate"></p>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="card mb-4">
        <h5 class="card-header">Danh sách lớp học</h5>
        <div class="card-body">
          <div class="d-flex justify-content-between mb-3">
            <button type="button" class="btn btn-primary btn-sm" @click="openModal">Thêm lớp học</button>
          </div>
          <div v-if="classForSchoolAdmin.value" class="table-responsive text-nowrap">
            <table class="table table-hover">
              <thead>
                <tr>
                  <th>Số thứ tự</th>
                  <th>Tên lớp</th>
                  <th>Khối</th>
                  <th>Trạng thái</th>
                  <th>Hành động</th>
                </tr>
              </thead>
              <tbody class="table-border-bottom-0">
                <tr v-for="(classItem, index) in classForSchoolAdmin.value.data.data" :key="classItem.id">
                  <td>
                    <i class="fab fa-angular fa-lg text-danger me-3">{{ index + 1 }}</i>
                  </td>
                  <td>
                    <strong>{{ classItem.className }}</strong>
                  </td>
                  <td>
                    <strong>{{ classItem.block }}</strong>
                  </td>
                  <td>
                    <span
                      :class="classItem.status === 1 ? 'badge bg-label-primary' : 'badge bg-label-danger'" 
                      class="me-1"
                    >
                      {{ classItem.status === 1 ? 'Hoạt động' : 'Chưa hoạt động' }}
                    </span>
                  </td>
                  <td>
                    <div class="dropdown">
                      <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                        <i class="bx bx-dots-vertical-rounded"></i>
                      </button>
                      <div class="dropdown-menu">
                        <a class="dropdown-item" href="javascript:void(0);" @click="showDetail(classItem.id)">
                          <i class="bx bx-show me-1"></i>Xem chi tiết
                        </a>
                        <a class="dropdown-item" href="javascript:void(0);" @click="openUpdateClassModal(classItem.id)">
                          <i class="bx bx-show me-1"></i>Chỉnh sửa thông tin
                        </a>
                        <a
                          class="dropdown-item text-danger"
                          href="javascript:void(0);"
                          @click="openModalDeleteClass(classItem.id)"
                        >
                          <i class="bx bx-trash me-1"></i>Xóa
                        </a>
                      </div>
                    </div>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>

    <AddNewClass
      :showModalAdd="showModalAdd"
      :errorsAddNewClass="errorsAddNewClass"
      :isUpdateMode="showModalUpdate"
      :dataClassDetail="dataClassDetail" 
      @update:showModal="showModalAdd = $event"
      @closeUpdateModal="showModalUpdate = $event"
      :requestAddClass="requestAddClass"
      @confirm="handleModalConfirm"
    />
    <DeleteClass
      :isShowModalDelete="showModalDelete"
      :classId="classIdToDelete"
      @update:showModalDelete="showModalDelete = $event"
      @confirm="handleModalConfirmDelete"
      @closeModal="closeModalDelete"
    />
    <!-- <AssignTeacher
      :modelValue="showAssignTeacherModal"
      :isUpdate="isUpdate"
      :apiUrl="apiUrl"
      :teacherList="teachers"
      :classId="selectedClassId"
      :semesterId="selectedSemester"
      @assignTeacher="handleModalConfirmAssignTeacher"
      @update:showModal="showAssignTeacherModal = $event"
    />
    <SwapTeacher
      :modelValue="showTeacherSwapModal"
      :apiUrl="apiUrl"
      :teacherList="teachers"
      :classId="selectedClassId"
      :semesterId="selectedSemester"
      @swapTeacher="handleModalConfirmSwapTeacher"
      @update:showModal="showTeacherSwapModal = $event"
    />
    <AssignPupil
      :showAssignPupilModal="showAssignPupilModal"
      :pupils="pupils"
      :classId="selectedClassId"
      :apiUrl="apiUrl"
      :semesterId="selectedSemester"
      @confirm="handleModalConfirmAssignPupils"
      @update:showModal="showAssignPupilModal = $event"
    />
    <RemoveTeacher
      :isShowModalTeacherDelete="isShowModalTeacherDelete"
      :teacherId="selectedTeacherId"
      :classId="classIdToDelete"
      :semesterId="selectedSemester"
      @confirm="handleTeacherRemove"
      @update:showModalTeacherDelete="isShowModalTeacherDelete = $event"
    /> -->
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, watch } from 'vue';
import AddNewClass from '@/components/modals/school-admin/AddNewClass.vue';
import DeleteClass from '@/components/modals/school-admin/DeleteClass.vue';
import AssignTeacher from '@/components/modals/school-admin/AssignTeacher.vue';
import RemoveTeacher from '@/components/modals/school-admin/RemoveTeacher.vue';
import AssignPupil from '@/components/modals/school-admin/AssignPupil.vue';
import SwapTeacher from '@/components/modals/school-admin/SwapTeacher.vue';
import { ViewClassAdminResponseInterface } from '@/types/model/admin-school';
import { useAdminSchoolComposable } from '@/composables/admin-school';
import {
  RequestAssignTeacherInterface,
  RequestDeleteTeacherInterface,
  RequestSwapTeacherInterface,
  RequestUpdateAssignTeacherInterface,
} from '@/types/model/class-enrollment';
import { AddNewClassRequestInterface } from '@/types/model/class';

export default defineComponent({
  name: 'ClassTableComponent',
  components: {
    AddNewClass,
    DeleteClass,
    AssignTeacher,
    RemoveTeacher,
    AssignPupil,
    SwapTeacher,
  },
  props: {
    classDataProp: {
      type: Array as () => ViewClassAdminResponseInterface[],
    },
  },
  emits: ['refresh'],
  setup(props, { emit }) {
    const {
      classIdToDelete,
      selectedClassId,
      selectedSemester,
      selectedTeacherId,
      requestRemoveTeacher,
      showModalAdd,
      isShowModalTeacherDelete,
      showModalDelete,
      classData,
      isUpdate,
      requestAddClass,
      requestGetClass,
      showAssignTeacherModal,
      teachers,
      semesterData,
      schoolId,
      pupils,
      showAssignPupilModal,
      errorsAddNewClass,
      apiUrl,
      selectedClassEnrollment,
      showTeacherSwapModal,
      showModalUpdate,
      dataClassDetail,
      openUpdateClassModal,
      showDetail,
      handleSwapTeacherAssign,
      closeModalPupil,
      openModalSwapTeacher,
      hanldeUpdatessignTeacher,
      closeAddModal,
      openModalPupil,
      closeModalDelete,
      openModalUpdateAssignTeacher,
      openIsUpdateAssignTeacher,
      openModal,
      openModalDeleteClass,
      handleFetchAssignPupils,
      handleFetchRemoveTeacherFromClass,
      handleFetchAssignTeacherToClass,
      handleFetchSemester,
      handleFetchClass,
      openModalAssignTeacher,
      handleFetchAddNewClass,
      handleFetchDeleteClass,
      openModalRemoveTeacher,
      filterBySemester,
      openAssignTeacherModal,
      getActiveSemester,
      handleModalConfirmAssignTeacher,
      handleModalConfirmAssignPupils,
      handleModalConfirmSwapTeacher,
      handleModalConfirmDelete,
      refeshPage,
      handleTeacherRemove,
      handleModalConfirm,
      classForSchoolAdmin,
    } = useAdminSchoolComposable();

    watch(refeshPage, async newValue => {
      if (newValue) {
        refeshPage.value = false;
        await handleFetchClass();
      }
    });
    onMounted(async () => {
      await handleFetchClass();
    });

    return {
      showTeacherSwapModal,
      showAssignPupilModal,
      requestAddClass,
      selectedTeacherId,
      showModalDelete,
      showAssignTeacherModal,
      isShowModalTeacherDelete,
      selectedClassId,
      teachers,
      pupils,
      semesterData,
      isUpdate,
      selectedSemester,
      showModalAdd,
      classData,
      classIdToDelete,
      errorsAddNewClass,
      apiUrl,
      selectedClassEnrollment,
      classForSchoolAdmin,
      showModalUpdate,
      dataClassDetail,
      showDetail,
      handleModalConfirmSwapTeacher,
      closeModalPupil,
      openModalSwapTeacher,
      closeModalDelete,
      openModalPupil,
      handleModalConfirmAssignPupils,
      openModalUpdateAssignTeacher,
      openModalRemoveTeacher,
      handleFetchSemester,
      openModalAssignTeacher,
      openModalDeleteClass,
      handleModalConfirmDelete,
      handleFetchClass,
      handleModalConfirm,
      handleFetchDeleteClass,
      openAssignTeacherModal,
      handleModalConfirmAssignTeacher,
      openModal,
      openIsUpdateAssignTeacher,
      filterBySemester,
      handleTeacherRemove,
      openUpdateClassModal,
    };
  },
});
</script>

<style scoped>
.content-wrapper {
  padding: 20px;
}

.card {
  border: 1px solid #e3e6f0;
  border-radius: 0.5rem;
}

.card-header {
  background-color: #f8f9fa;
  border-bottom: 1px solid #e3e6f0;
}

.table {
  border-collapse: collapse;
  width: 100%;
}

.table th {
  background-color: #92a2b2;
  color: white;
  text-align: left;
  padding: 10px;
}

.table td {
  padding: 10px;
  border-bottom: 1px solid #e3e6f0;
}

.table-hover tbody tr:hover {
  background-color: #f1f1f1;
}

.btn-primary {
  background-color: #007bff;
  border-color: #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
}

.badge {
  font-size: 0.875rem;
}

.dropdown-menu {
  min-width: 200px;
}

.text-danger {
  color: #dc3545 !important;
}

.table-responsive {
  margin-top: 20px;
}
.btn-sm {
  padding: 1px 2px;
  font-size: 0.875rem;
}
</style>
