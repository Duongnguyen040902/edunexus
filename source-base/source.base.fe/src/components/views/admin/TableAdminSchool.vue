<template>
  <div>
    <table
      id="DataTables_Table_0"
      aria-describedby="DataTables_Table_0_info"
      class="datatables-users table border-top dataTable no-footer dtr-column collapsed"
      style="width: 1391px"
    >
      <thead>
        <tr>
          <th aria-label="" class="control sorting_disabled" colspan="1" rowspan="1" style="width: 0px"></th>
          <th
            aria-label=""
            class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all"
            colspan="1"
            data-col="1"
            rowspan="1"
            style="width: 18px"
          >
            ID
          </th>
          <th
            aria-controls="DataTables_Table_0"
            aria-label="User: activate to sort column ascending"
            aria-sort="descending"
            class="sorting sorting_desc"
            colspan="1"
            rowspan="1"
            style="width: 334px"
            tabindex="0"
          >
            Trường
          </th>
          <th
            aria-controls="DataTables_Table_0"
            aria-label="Role: activate to sort column ascending"
            class="sorting"
            colspan="1"
            rowspan="1"
            style="width: 149px"
            tabindex="0"
          >
            Địa chỉ
          </th>
          <th
            aria-controls="DataTables_Table_0"
            aria-label="Plan: activate to sort column ascending"
            class="sorting"
            colspan="1"
            rowspan="1"
            style="width: 107px"
            tabindex="0"
          >
            Gói
          </th>
          <th
            aria-controls="DataTables_Table_0"
            aria-label="Billing: activate to sort column ascending"
            class="sorting"
            colspan="1"
            rowspan="1"
            style="width: 201px"
            tabindex="0"
          >
            Website
          </th>
          <th
            aria-controls="DataTables_Table_0"
            aria-label="Status: activate to sort column ascending"
            class="sorting"
            colspan="1"
            rowspan="1"
            style="width: 101px"
            tabindex="0"
          >
            Trạng thái
          </th>
          <th aria-label="Actions" class="sorting_disabled" colspan="1" rowspan="1" style="width: 175px">Hành động</th>
        </tr>
      </thead>
      <tbody>
        <tr v-if="dataSchoolAdmin.length === 0">
          <td class="text-center" colspan="8">Không có dữ liệu</td>
        </tr>
        <tr v-for="(accountSchoolAdmin, index) in dataSchoolAdmin" v-else :class="index % 2 === 0 ? 'even' : 'odd'">
          <td class="control" style="" tabindex="0"></td>
          <td>
            <span class="text-truncate d-flex align-items-center text-heading">
              {{ accountSchoolAdmin.id }}
            </span>
          </td>
          <td class="sorting_1">
            <div class="d-flex justify-content-start align-items-center user-name">
              <div class="avatar-wrapper">
                <div class="avatar avatar-sm me-4">
                  <img :src="getUserAvatar(accountSchoolAdmin?.image)" alt="Avatar" class="rounded-circle" />
                </div>
              </div>
              <div class="d-flex flex-column">
                <a class="text-heading text-truncate" href="#">
                  <span class="fw-medium">{{ accountSchoolAdmin.schoolName }}</span>
                </a>
                <small>{{ accountSchoolAdmin.email }}</small>
              </div>
            </div>
          </td>
          <td>
            <span class="text-truncate d-flex align-items-center text-heading">
              {{ accountSchoolAdmin.address ?? 'Chưa có' }}
            </span>
          </td>
          <td>
            <span class="text-heading">
              {{
                accountSchoolAdmin.schoolSubscriptionPlans && accountSchoolAdmin.schoolSubscriptionPlans[0]
                  ? accountSchoolAdmin.schoolSubscriptionPlans[0].subscriptionPlan.name
                  : 'Chưa có gói'
              }}
            </span>
          </td>
          <td>{{ accountSchoolAdmin.websiteLink ?? 'Chưa có' }}</td>
          <td>
            <span :class="getClassByStatus(Number(accountSchoolAdmin.accountStatus))" class="badge bg-label-success">{{
              accountSchoolAdmin.statusName
            }}</span>
          </td>
          <td class="" style="">
            <div class="d-flex align-items-center">
              <router-link
                :to="{ path: '/admin/detail-account', query: { id: accountSchoolAdmin.id } }"
                class="btn btn-icon"
              >
                <i class="bx bx-show bx-md"></i>
              </router-link>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script lang="ts">
import { computed, defineComponent, PropType } from 'vue';
import { ResponseAdminSchoolIndex } from '@/types/model/admin-school.ts';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import { AccountStatus } from '@/constants/enums/statuses.ts';

export default defineComponent({
  name: 'TableAdminSchoolComponent',
  components: {},
  props: {
    dataSchoolAdmin: {
      type: Array as PropType<ResponseAdminSchoolIndex[]>,
      required: true,
    },
  },
  setup(props, { emit }) {
    const adminSchoolComposable = useAdminSchoolComposable();
    const { handleRedirectToDetail } = adminSchoolComposable;
    const getClassByStatus = (status: number) => {
      switch (status) {
        case AccountStatus.ACTIVE:
          return 'bg-label-success';
        case AccountStatus.INACTIVE:
          return 'bg-label-danger';
        case AccountStatus.DELETED:
          return 'bg-label-danger';
        default:
          return 'bg-label-primary';
      }
    };
    const apiUrl = import.meta.env.VITE_APP_API_URL;

    const getUserAvatar = (image: string | null) => {
      return image ? `${apiUrl}${image}` : 'http://localhost:8082/assets/images/avatars/1.png';
    };

    return {
      apiUrl,
      getUserAvatar,
      getClassByStatus,
      handleRedirectToDetail,
    };
  },
});
</script>
