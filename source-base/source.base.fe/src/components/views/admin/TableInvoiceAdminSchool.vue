<template>
  <table class="table datatable-invoice dataTable no-footer dtr-column">
    <thead>
      <tr>
        <th>#</th>
        <th>Trạng thái</th>
        <th>Tổng tiền</th>
        <th>Ngày thanh toán</th>
        <th>Gói sử dụng</th>
      </tr>
    </thead>
    <tbody v-if="schoolSubscriptionPlans.data.length > 0">
      <tr v-for="(invoice, index) in schoolSubscriptionPlans.data" :key="index">
        <td>
          <a href="#">{{ invoice.id || 'N/A' }}</a>
        </td>
        <td>{{ invoice.status === 0 ? 'Chưa thanh toán' : 'Đã thanh toán' }}</td>
        <td>{{ invoice.totalAmount || 'N/A' }}</td>
        <td>{{ invoice.payments && invoice.payments.length > 0 ? formatDate(invoice.payments.slice(-1)[0].paymentDate, 'dd/mm/yyyy') : 'N/A' }}</td>
        <td>{{ invoice.subscriptionPlanName || 'N/A' }}</td>
      </tr>
    </tbody>
    <tbody v-else>
      <tr>
        <td class="text-center" colspan="5">Không có hóa đơn nào</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import { ref, computed, onMounted } from 'vue';
import { formatDate } from '@/helpers/formatDate';
export default {
  props: {
    schoolSubscriptionPlans: {
      type: Array,
      required: true,
    },
  },
  setup(props) {
    const invoices = ref([]);
    onMounted(() => {
      invoices.value = props.schoolSubscriptionPlans.data;
    });

    return {
      invoices,
      formatDate,
    };
  },
};
</script>
