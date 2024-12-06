<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="card">
      <div
            class="card-header border-bottom d-flex justify-content-between align-items-center"
          >
            <h5 class="card-title mb-0">Xem câu lạc bộ theo kỳ</h5>
            <div class="d-flex justify-content-end align-items-center col-md-7">
              <div class="col-md-6 user_role d-flex align-items-center">
                <h6 class="mb-0 me-2 nowrap-text">Chọn kỳ: </h6>
                <select id="UserRole" class="form-select text-capitalize" v-model="selectedSemesterId" @change="handleSemesterChange">
                  <option v-for="semester in semesters.data" :key="semester.id" :value="semester.id">
                    {{ semester.semesterName }} - {{ semester.schoolYearName }}
                  </option>
                </select>
              </div>
            </div>
          </div>
      <div class="card-datatable table-responsive">
        <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed"
          id="DataTables_Table_0" aria-describedby="DataTables_Table_0_info" style="width: 100%">
          <thead>
            <tr>
              <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                style="width: 334px" aria-label="Club: activate to sort column ascending">
                Tên câu lạc bộ
              </th>
              <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                style="width: 149px" aria-label="Description: activate to sort column ascending">
                Mô tả
              </th>
              <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                style="width: 149px" aria-label="Teacher: activate to sort column ascending">
                Giáo viên
              </th>
              <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
                style="width: 107px" aria-label="Status: activate to sort column ascending">
                Trạng thái
              </th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="!dataClubEnrollment.value.data || dataClubEnrollment.value.data.length === 0">
              <td colspan="4" class="text-center">Không tham gia câu lạc bộ nào trong kì này</td>
            </tr>
            <tr v-else v-for="(enrollment, index) in dataClubEnrollment.value.data" :key="enrollment.id"
              :class="index % 2 === 0 ? 'even' : 'odd'" @click="openModalDetail(enrollment)">
              <td>{{ enrollment.clubName }}</td>
              <td>{{ truncatedDescription(enrollment.clubDescription) }}</td>
              <td>{{ enrollment.teacher && enrollment.teacher.firstName && enrollment.teacher.lastName ? `${enrollment.teacher.firstName} ${enrollment.teacher.lastName}` : 'Kì này chưa có giáo viên' }}</td>
              <td>
                <span class="badge" :class="getStatus(enrollment.status).class">
                  {{ getStatus(enrollment.status).text }}
                </span>
              </td>
            </tr>
          </tbody>
        </table>
        <ModalClubDetail :isShowModal="isShowModalDetail" :club="selectedClub" @closeModal="handleCloseModalDetail" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useClubComposable } from '@/composables/pupil-club';
import ModalClubDetail from '@/components/modals/pupil/ModalClubDetail.vue';

export default defineComponent({
  name: 'ViewClub',
  components: {
    ModalClubDetail,
  },
  setup() {
    const clubComposable = useClubComposable();
    const {
      semesters,
      dataClubEnrollment,
      handleGetClubEnrollmentByPupilId,
      handleGetSemester,
      isShowModalDetail,
      handleCloseModalDetail,
    } = clubComposable;

    const selectedSemesterId = ref<number | null>(null);
    const selectedClub = ref<any>({});

    const handleSemesterChange = async () => {
      if (selectedSemesterId.value !== null) {
        await handleGetClubEnrollmentByPupilId(selectedSemesterId.value);
      }
    };

    const openModalDetail = (club: any) => {
      selectedClub.value = club;
      isShowModalDetail.value = true;
    };

    const truncatedDescription = (description: string) => {
      if (description.length > 25) {
        return description.substring(0, 25) + '...';
      }
      return description;
    };

    const getStatus = (status: number) => {
      switch (status) {
        case 1:
          return { text: 'Đã đăng kí', class: 'bg-label-success' };
        case 2:
          return { text: 'Đã tham gia', class: 'bg-label-info' };
        case 3:
          return { text: 'Bị từ chối', class: 'bg-label-warning' };
        case 4:
          return { text: 'Đã hủy', class: 'bg-label-danger' };
        default:
          return { text: 'Chưa đăng kí', class: 'bg-label-default' };
      }
    };

    onMounted(async () => {
      await handleGetSemester();
      if (semesters.data.length > 0) {
        selectedSemesterId.value = semesters.data[0].id;
        await handleSemesterChange();
      }
    });

    return {
      semesters,
      isShowModalDetail,
      handleCloseModalDetail,
      dataClubEnrollment,
      selectedSemesterId,
      selectedClub,
      handleSemesterChange,
      openModalDetail,
      truncatedDescription,
      getStatus,
    };
  },
});
</script>

<style scoped>
.form-group {
  margin-bottom: 20px;
}
.nowrap-text {
  white-space: nowrap;
}
.custom-select {
  appearance: none;
  -webkit-appearance: none;
  -moz-appearance: none;
  background-color: #007bff;
  border: 1px solid #007bff;
  border-radius: 4px;
  padding: 10px 20px;
  font-size: 16px;
  color: #fff;
  width: 100%;
  max-width: 200px; /* Kích thước 3-6 */
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1);
  transition: border-color 0.3s, box-shadow 0.3s;
  cursor: pointer;
}

.custom-select:focus {
  border-color: #0056b3;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
  outline: none;
}

.custom-select option {
  padding: 10px;
  font-size: 16px;
  color: #333;
  background-color: #fff;
}
.table {
  width: 100%;
  margin-bottom: 1rem;
  color: #212529;
}

.table th,
.table td {
  padding: 0.75rem;
  vertical-align: top;
  border-top: 1px solid #dee2e6;
}

.table thead th {
  vertical-align: bottom;
  border-bottom: 2px solid #dee2e6;
}

.table tbody + tbody {
  border-top: 2px solid #dee2e6;
}

.table .table {
  background-color: #fff;
}

.badge {
  display: inline-block;
  padding: 0.25em 0.4em;
  font-size: 75%;
  font-weight: 700;
  line-height: 1;
  text-align: center;
  white-space: nowrap;
  vertical-align: baseline;
  border-radius: 0.375rem;
}

.bg-label-success {
  background-color: #28a745;
  color: #fff;
}

.bg-label-info {
  background-color: #17a2b8;
  color: #fff;
}

.bg-label-warning {
  background-color: #ffc107;
  color: #212529;
}

.bg-label-danger {
  background-color: #dc3545;
  color: #fff;
}

.bg-label-default {
  background-color: #6c757d;
  color: #fff;
}
</style>