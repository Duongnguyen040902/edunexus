<template>
<table class="table datatable-invoice dataTable no-footer dtr-column collapsed" id="DataTables_Table_1" aria-describedby="DataTables_Table_1_info" style="width: 911px">
    <thead>
        <tr>
            <th class="control sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 0px" aria-label=": activate to sort column ascending"></th>
            <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 95px" aria-label="#: activate to sort column ascending" aria-sort="descending">
                #
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 112px" aria-label="Status: activate to sort column ascending">
                Trạng thái
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 98px" aria-label="Total: activate to sort column ascending">
                Tên gói
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 170px" aria-label="Issued Date: activate to sort column ascending">
                Ngày bắt đầu
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 170px" aria-label="Issued Date: activate to sort column ascending">
                Ngày kết thúc
            </th>
        </tr>
    </thead>
    <tbody>
        <tr v-for="schoolSubscription in listSchoolSubscriptionResponse">
            <td class="control" tabindex="0" style=""></td>
            <td class="sorting_1">
                <a href="#"><span>#{{ schoolSubscription.id }}</span></a>
            </td>
            <td>
                <div class="tooltip-container">
                    <span v-if="schoolSubscription.status === 1" class="badge badge-center d-flex align-items-center justify-content-center rounded-pill bg-label-success w-px-30 h-px-30">
                        <i class="bx bx-check bx-xs"></i>
                    </span>
                    <div class="tooltip-content">
                        <p>{{ schoolSubscription.statusName }}</p>
                    </div>
                </div>
                <div class="tooltip-container">
                    <span v-if="schoolSubscription.status === 2" class="badge badge-center d-flex align-items-center justify-content-center rounded-pill bg-label-danger w-px-30 h-px-30">
                        <i class="bx bx-error bx-xs"></i>
                    </span>
                    <div class="tooltip-content">
                        <p>{{ schoolSubscription.statusName }}</p>
                    </div>
                </div>
            </td>
            <td>{{ schoolSubscription.subscriptionPlanName }}</td>
            <td>{{ formatDate(schoolSubscription.startDate)}}</td>
            <td>{{ formatDate(schoolSubscription.endDate)}}</td>
            <td>
                <div>
                    <a v-for="invoice in schoolSubscription.invoices" :key="invoice.id" class="dropdown-item" href="javascript:void(0);" @click="handleDetailClick(invoice.id)">
                        <i class="bx bx-edit-alt me-1"></i>
                    </a>
                    <a v-if="!schoolSubscription.invoices || schoolSubscription.invoices.length === 0" class="dropdown-item text-muted" href="javascript:void(0);" @click="showNoInvoiceNotification()">
                        <i class="bx bx-edit-alt me-1"></i>
                    </a>
                </div>
            </td>
        </tr>
    </tbody>
</table>
</template>

    
<script lang="ts">
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';
import {
    notifyError
} from '@/helpers/notify';
import {
    defineComponent,
    PropType,
} from 'vue';
export default defineComponent({
    name: 'SchoolSubscriptionTableComponent',
    props: {
        listSchoolSubscriptionResponse: {
            type: Array,
            default: () => []
        },
        formatDate: {
            type: Function as PropType < (date: string) => string > ,
            required: true,
        },
        formatCurrency: {
            type: Function as PropType < (amount: number) => string > ,
            required: true,
        },
    },
    components: {
        ModalConfirmDeleteComponent
    },
    emits: ['handleDetailClick'],
    setup(props, {
        emit
    }) {

        const handleDetailClick = (invoiceId: number) => {
            emit('handleDetailClick', invoiceId);
        };
        const showNoInvoiceNotification = () => {
            notifyError('Gói Basic là miễn phí nên không có hóa đơn chi trả.');
        };
        return {
            handleDetailClick,
            showNoInvoiceNotification
        };
    },
});
</script>

    
<style scoped>
.tooltip-container {
    position: relative;
    display: inline-block;
}

.tooltip-content {
    visibility: hidden;
    width: 140px;
    background-color: #2d3a45;
    color: #fff;
    text-align: center;
    border-radius: 5px;
    padding: 5px;
    position: absolute;
    z-index: 1;
    bottom: 125%;
    left: 50%;
    transform: translateX(-50%);
    opacity: 0;
    transition: opacity 0.3s;
    font-size: 12px;
}

.tooltip-container:hover .tooltip-content {
    visibility: visible;
    opacity: 1;
}

.tooltip-content::after {
    content: '';
    position: absolute;
    top: 100%;
    left: 50%;
    margin-left: -5px;
    border-width: 5px;
    border-style: solid;
    border-color: #2d3a45 transparent transparent transparent;
}
</style>
