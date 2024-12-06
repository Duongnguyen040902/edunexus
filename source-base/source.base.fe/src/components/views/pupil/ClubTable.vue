<template>
  <div>
    <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed" id="DataTables_Table_0"
      aria-describedby="DataTables_Table_0_info" style="width: 1391px">
      <thead>
        <tr>
          <th class="control sorting_disabled" rowspan="1" colspan="1" style="width: 0px" aria-label=""></th>
          <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 334px" aria-label="Club: activate to sort column ascending" aria-sort="descending">
            Tên câu lạc bộ
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 149px" aria-label="Description: activate to sort column ascending">
            Mô tả
          </th>
          <th class="sorting" tabindex="0" aria-controls="DataTables_Table_0" rowspan="1" colspan="1"
            style="width: 107px" aria-label="Status: activate to sort column ascending">
            Trạng thái
          </th>
          <th class="sorting_disabled" rowspan="1" colspan="1" style="width: 175px" aria-label="Actions">
            Actions
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(club, index) in data" :class="index % 2 === 0 ? 'even' : 'odd'"
          :key="club.id">
          <td class="control" tabindex="0" style=""></td>
          <td class="sorting_1">
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="d-flex flex-column">
                <span class="fw-medium">{{ club.name }}</span>
                <small>{{ club.schoolName }}</small>
              </div>
            </div>
          </td>
          <td>
            <span class="text-heading">{{ club.description }}</span>
          </td>
          <td>
            <span class="badge" :class="club.status === 1 ? 'bg-label-success' : 'bg-label-danger'" text-capitalized="">
              {{ club.status === 1 ? 'Đang hoạt động' : 'Không hoạt động' }}
            </span>
          </td>
          <td class="" style="">
            <div class="d-flex align-items-center">
              <a href="#" class="btn btn-icon delete-record"><i class="bx bx-trash bx-md"
                  @click="$emit('openModalDelete', club)"></i></a>
              <a href="#" class="btn btn-icon dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                <i class="bx bx-dots-vertical-rounded bx-md"></i>
              </a>
              <div class="dropdown-menu dropdown-menu-end m-0">
                <a href="#" class="dropdown-item" @click="$emit('openModalEdit', club)">Sửa</a>
                <a href="#" class="dropdown-item" @click="$emit('toggleStatus', club)">{{ club.status === 1 ? 'Tắt hoạt động' : 'Bật hoạt động' }}</a>
              </div>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from "vue";
import { ResponseGetClubIndex } from "@/types/model/pupil-club";

export default defineComponent({
  name: "ClubTableComponent",
  props: {
    data: {
      type: Array as PropType<ResponseGetClubIndex[]>,
      required: true,
    },
  },
  setup(props, { emit }) {
    return {};
  },
});
</script>