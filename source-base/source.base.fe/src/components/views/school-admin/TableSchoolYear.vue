<template>
  <div>
    <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed" id="DataTables_Table_0"
      aria-describedby="DataTables_Table_0_info" style="width: 1391px">
      <thead>
        <tr>
          <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px" aria-label=""></th>

          <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 334px" aria-label="User: activate to sort column ascending" aria-sort="descending">
            Tên kì học
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 149px" aria-label="Role: activate to sort column ascending">
            Ngày bắt đầu
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 107px" aria-label="Plan: activate to sort column ascending">
            Ngày kết thúc
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 201px" aria-label="Billing: activate to sort column ascending">
            Trạng thái
          </th>
          <th class="sorting_disabled" rowspan="1" colspan="1" style="width: 175px" aria-label="Actions">
            Hành động
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="dataSchoolYear.length === 0">
          <td colspan="7" class="text-center">Không có năm học</td>
        </tr>
        <tr v-for="(schoolYear, index) in dataSchoolYear" :class="index % 2 === 0 ? 'even' : 'odd'"
          :key="schoolYear.id">
          <td class="control" tabindex="0" style=""></td>

          <td class="sorting_1">
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ schoolYear.name }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ formatDate(schoolYear.startDate) }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate">
                  <span class="fw-medium">{{ formatDate(schoolYear.endDate) }}</span>
                </a>
              </div>
            </div>
          </td>
          <td>
            <span :class="['badge', schoolYear.isActive ? 'bg-label-success' : 'bg-label-danger']"
              text-capitalized="">{{
                schoolYear.isActive ? "Đang hoạt động" : "Không hoạt động"
              }}</span>
          </td>
          <td class="" style="">
            <div class="d-flex align-items-center">
               <!-- <a  ><i title="Quản lý kỳ học" @click="gotosemester(`${schoolYear.id}`)" class='bx bx-detail bx-md'></i></a> -->
               <a :href="`http://localhost:8082/school/semester?schoolYearId=${schoolYear.id}`" style="color:dimgrey" ><i title="Quản lý kỳ học" class='bx bx-detail bx-md'></i></a>
              <a v-if="new Date(schoolYear.endDate) > new Date()"><i title="chỉnh sửa"
                  @click="$emit('openModalEdit', schoolYear)" class='bx bx-edit bx-md'></i></a>
              <a v-if="new Date(schoolYear.endDate) > new Date()"><i title="Bật hoạt động"
                  @click="$emit('isActive', schoolYear)" class='bx bx-toggle-right bx-md'></i></a>
              <a href="#" class="btn btn-icon delete-record"><i class="bx bx-trash bx-md"
                @click="$emit('openModalDelete', schoolYear)"></i></a>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script lang="ts">
import { defineComponent, PropType, ref } from "vue";
import { ResponseSchoolYearIndex } from "@/types/model/school-year.ts";
import { useSchoolYearComposable } from "@/composables/school-year.ts";
import ModalEditSchoolYearComponent from "@/components/modals/school-year/EditSchoolYearModal.vue";
export default defineComponent({
  name: "TableSchoolYearComponent",
  components: { ModalEditSchoolYearComponent },
  props: {
    dataSchoolYear: {
      type: Array as PropType<ResponseSchoolYearIndex[]>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const schoolYearComposable = useSchoolYearComposable();
    const { gotosemester } = schoolYearComposable;
    function formatDate(date: string) {
      const d = new Date(date);
      const month = `${d.getMonth() + 1}`.padStart(2, "0");
      const day = `${d.getDate()}`.padStart(2, "0");
      const year = d.getFullYear();
      return `${day}-${month}-${year}`;
    }
    return { formatDate, gotosemester };
  },
});
</script>
