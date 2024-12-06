<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row">
      <div class="col-xl-4 col-lg-5 order-1 order-md-0">
        <ProfileAdminSchoolComponent :school="schoolAdminDetail.value.school" />
      </div>
      <InvoiceAdminSchoolComponent
        :schoolSubscriptionPlans="listInvoiceResponse.value"
      />
    </div>
  </div>
</template>
<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import { useAdminSchoolComposable } from '@/composables/admin-school.ts';
import { useRouter, useRoute } from 'vue-router';
import ProfileAdminSchoolComponent from '@/components/views/admin/ProfileAdminSchool.vue';
import InvoiceAdminSchoolComponent from '@/components/views/admin/InvoiceAdminSchool.vue';
import {useInvoiceComposable} from '@/composables/invoice.ts';
export default defineComponent({
  name: 'DetailAccount',
  components: {
    ProfileAdminSchoolComponent,
    InvoiceAdminSchoolComponent,
  },
  setup() {
    const adminSchoolComposable = useAdminSchoolComposable();
    const { schoolAdminDetail, handleGetSchoolDetail } = adminSchoolComposable;
    const invoiceComposable = useInvoiceComposable();
    const { handleGetListInvoice, requestInvoiceList, listInvoiceResponse } = invoiceComposable;
    const route = useRoute();

    onMounted(async () => {
      if (route.query.id) {
        await handleGetSchoolDetail(Number(route.query.id));
        requestInvoiceList.school = Number(route.query.id);

        await handleGetListInvoice();
        console.log(listInvoiceResponse.value);
      }
    });

    return {
      schoolAdminDetail,
      handleGetSchoolDetail,
      listInvoiceResponse,
    };
  },
});
</script>