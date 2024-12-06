<template>
  <div>
    <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed" id="DataTables_Table_0" aria-describedby="DataTables_Table_0_info" style="width: 1391px">
      <thead>
        <tr>
          <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px" aria-label=""></th>
          
          <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1" style="width: 334px" aria-label="User: activate to sort column ascending" aria-sort="descending">
            Tên học kỳ
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1" style="width: 149px" aria-label="Role: activate to sort column ascending">
            Mã học kỳ
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1" style="width: 107px" aria-label="Plan: activate to sort column ascending">
            Ngày bắt đầu
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1" style="width: 107px" aria-label="Plan: activate to sort column ascending">
            Ngày kết thúc
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1" style="width: 201px" aria-label="Billing: activate to sort column ascending">
            Trạng thái
          </th>
          <th class="sorting_disabled" rowspan="1" colspan="1" style="width: 175px" aria-label="Actions">Actions</th>
        </tr>
      </thead>
      <tbody >
        <tr v-if="dataSemester.length===0">
          <td colspan="7" class="text-center">Không có kỳ học</td>
        </tr>
        <tr  v-for="(semester, index) in dataSemester" :class="index % 2 === 0 ? 'even' : 'odd'" :key="semester.id">
          <td class="control" tabindex="0" style=""></td>
          
          <td class="sorting_1">
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ semester.semesterName }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ semester.semesterCode }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ formatDate(semester.startDate) }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ formatDate(semester.endDate) }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <span :class="['badge', semester.isActive ? 'bg-label-success' : 'bg-label-danger']"
              text-capitalized="">{{
                semester.isActive ? "Đang hoạt động" : "Không hoạt động"
              }}</span>
          </td>
          <td class="" style="">
            <div class="d-flex align-items-center">
              
              <a v-if="new Date(semester.endDate) > new Date()"><i title="chỉnh sửa"
                  @click="$emit('openModalEdit', semester)" class='bx bx-edit bx-md'></i></a>
              <a v-if="new Date(semester.endDate) > new Date()"><i title="Bật hoạt động"
                  @click="$emit('isActive', semester)" class='bx bx-toggle-right bx-md'></i></a>
              <a class="btn btn-icon delete-record"><i class="bx bx-trash bx-md" @click="$emit('openModalDelete', semester)"></i></a>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue';
import { ResponseSemesterDetail } from '@/types/model/semester';
export default defineComponent({
  name: 'TableSemesterComponent',
  props: {
    dataSemester: {
      type: Array as PropType<ResponseSemesterDetail[]>,
      required: true,
    },
  },
  setup() {
    
    function formatDate(date: string) {
      const d = new Date(date);
      const month = `${d.getMonth() + 1}`.padStart(2, '0');
      const day = `${d.getDate()}`.padStart(2, '0');
      const year = d.getFullYear();
      return `${day}-${month}-${year}`;
    }

    return {
      formatDate,
    };
  },
});
</script>