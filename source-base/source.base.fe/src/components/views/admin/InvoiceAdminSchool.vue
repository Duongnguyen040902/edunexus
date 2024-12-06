<template>
  <div class="col-xl-8 col-lg-7 order-0 order-md-1">
    <div class="card mb-4">
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_1_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="row mx-6">
            <div
              class="col-sm-6 col-12 d-flex align-items-center justify-content-center justify-content-sm-start mt-6 mt-sm-0"
            >
              <div class="head-label"><h5 class="card-title mb-0">Lịch sử hóa đơn</h5></div>
            </div>
            <div class="col-sm-6 col-12 d-flex justify-content-center justify-content-md-end align-items-baseline">
              <div class="dt-action-buttons d-flex justify-content-center flex-md-row align-items-baseline gap-4">
                <div id="DataTables_Table_1_length" class="dataTables_length"></div>
              </div>
            </div>
          </div>
          <TableInvoiceAdminSchoolComponent :schoolSubscriptionPlans="schoolSubscriptionPlans" />
          <div class="row mx-6">
            <div class="col-sm-12 col-xxl-6 text-center text-xxl-start pb-md-2 pb-xxl-0"></div>
            <div class="col-sm-12 col-xxl-6 d-md-flex justify-content-xxl-end justify-content-center"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import TableInvoiceAdminSchoolComponent from '@/components/views/admin/TableInvoiceAdminSchool.vue';
import { SchoolSubscriptionPlan } from '@/types/model/admin-school.ts';

export default defineComponent({
  name: 'InvoiceAdminSchoolComponent',
  components: {
    TableInvoiceAdminSchoolComponent,
  },
  props: {
    schoolSubscriptionPlans: {
      type: Array as () => SchoolSubscriptionPlan[],
      required: true,
    },
  },
  setup(props, { emit }) {

    onMounted(async () => {
    });

    return {
    };
  },
});
</script>
