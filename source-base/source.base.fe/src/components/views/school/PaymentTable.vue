<template>
<div class="table-responsive border border-bottom-0 border-top-0 rounded">
    <table class="table m-0">
        <thead>
            <tr>
                <th>#</th>
                <th>Đơn giá</th>
                <th>Ngày thanh toán</th>
                <th>Dịch vụ</th>
                <th>Trạng thái</th>
            </tr>
        </thead>
        <tbody>
            <tr v-for="payment in responseInvoiceDetail.payments">
                <td class="text-nowrap text-heading">{{ payment.id }}</td>
                <td class="text-nowrap">{{ formatCurrency(payment.amount) }}</td>
                <td>{{ formatDate(payment.paymentDate) }}</td>
                <td>{{ payment.paymentMethod }}</td>
                <td>{{ getStatusLabel(payment.status) }}</td>
            </tr>
        </tbody>
    </table>
</div>
</template>

    
<script lang="ts">
import {
    defineComponent,
    PropType,
} from 'vue';
import {
    PaymentStatuses,
    PaymentStatusLabels
} from '@/constants/enums/statuses';
export default defineComponent({
    name: 'paymentTableComponent',
    props: {
        responseInvoiceDetail: {
            type: Object,
            default: () => []
        },
        formatDate: {
            type: Function as PropType < (date: string) => string > ,
            required: true,
        },
        formatCurrency: {
            type: Function as PropType < (amount: number) => string > ,
            required: true,
        }
    },
    setup() {
        const getStatusLabel = (status: PaymentStatuses): string => {
            return PaymentStatusLabels[status] || 'Không xác định';
        };

        return {
            getStatusLabel,
        };
    },
});
</script>
