<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="card">
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="row">
            <div class="col-sm-12 col-md-3">
              <div id="DataTables_Table_0_length" class="dataTables_length">
                <button
                  aria-controls="DataTables_Table_0"
                  class="btn btn-primary mb-6 mb-md-0"
                  tabindex="0"
                  type="button"
                  @click="handleOpenModalSubscription"
                >
                  <span
                    ><i class="bx bx-plus bx-xs me-0 me-sm-2"></i
                    ><span class="d-none d-sm-inline-block">Tạo Gói</span></span
                  >
                </button>
              </div>
            </div>
          </div>
          <table
            id="DataTables_Table_0"
            aria-describedby="DataTables_Table_0_info"
            class="datatables-permissions table dataTable no-footer dtr-column collapsed"
            style="width: 1210px"
          >
            <thead class="border-top">
              <tr>
                <th aria-label="" class="control sorting_disabled" colspan="1" rowspan="1" style="width: 0px"></th>
                <th
                  aria-controls="DataTables_Table_0"
                  aria-label="Name: activate to sort column ascending"
                  class="sorting"
                  colspan="1"
                  rowspan="1"
                  style="width: 256px"
                  tabindex="0"
                >
                  Tên gói
                </th>
                <th aria-label="Assigned To" class="sorting_disabled" colspan="1" rowspan="1" style="width: 397px">
                  Mô tả
                </th>
                <th aria-label="Created Date" class="sorting_disabled" colspan="1" rowspan="1" style="width: 250px">
                  Giá
                </th>
                <th aria-label="Created Date" class="sorting_disabled" colspan="1" rowspan="1" style="width: 250px">
                  Thời hạn (ngày)
                </th>
                <th aria-label="Created Date" class="sorting_disabled" colspan="1" rowspan="1" style="width: 250px">
                  Số lượng tài khoản
                </th>
                <th class="cell-fit sorting_disabled" style="width: 118px">Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(subscription, index) in responseSubscription.value" class="odd">
                <td>
                  <span class="text-nowrap text-heading">{{ subscription.id }}</span>
                </td>
                <td>
                  <span class="text-nowrap text-heading">{{ subscription.name }}</span>
                </td>
                <td>
                  <span class="text-nowrap text-heading">{{ subscription.description }}</span>
                </td>
                <td>
                  <span class="text-nowrap">{{ subscription.price }}</span>
                </td>
                <td>
                  <span class="text-nowrap">{{ subscription.durationDays }}</span>
                </td>
                <td>
                  <span class="text-nowrap">{{ subscription.maxActiveAccounts }}</span>
                </td>
                <td>
                  <div class="d-flex align-items-center">
                    <button class="btn btn-icon"> <i class="bx bx-edit bx-md" @click="handleClickDetailSubscription(subscription)"></i></button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <ModalSubscriptionComponent
    :isShowModal="isShowModalSubscription"
    @closeModal="handleCloseModalSubscription"
    @confirmAction="handleConfirmWithAction"
  />
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import { useSubscriptionComposable } from '@/composables/subscription.ts';
import ModalSubscriptionComponent from '@/components/modals/subscription/ModalSubscription.vue';

export default defineComponent({
  name: 'ServicePackageIndex',
  components: {
    ModalSubscriptionComponent,
  },
  props: {},
  emits: [],
  setup(props, { emit }) {
    const subscription = useSubscriptionComposable();
    const {
      isShowModalSubscription,
      errorSubscription,
      requestCreateSubscription,
      responseSubscription,
      handleGetAllSubscription,
      handleOpenModalSubscription,
      handleClickDetailSubscription,
      handleCloseModalSubscription,
      handleConfirmWithAction,
    } = subscription;

    onMounted(async () => {
      await handleGetAllSubscription();
    });

    return {
      isShowModalSubscription,
      errorSubscription,
      requestCreateSubscription,
      responseSubscription,
      handleGetAllSubscription,
      handleOpenModalSubscription,
      handleClickDetailSubscription,
      handleCloseModalSubscription,
      handleConfirmWithAction,
    };
  },
});
</script>

<style scoped></style>
