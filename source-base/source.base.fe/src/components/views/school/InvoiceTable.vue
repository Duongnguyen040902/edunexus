<template>
<table class="table datatable-invoice dataTable no-footer dtr-column collapsed" id="DataTables_Table_1" aria-describedby="DataTables_Table_1_info" style="width: 911px">
    <thead>
        <tr>
            <th class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all" style="width: 18px">
                <input :checked="selectAll" class="form-check-input" type="checkbox" @change="selectAllRows" />
            </th>
            <th class="sorting sorting_desc" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 95px" aria-label="#: activate to sort column ascending" aria-sort="descending">
                #
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 112px" aria-label="Status: activate to sort column ascending">
                Trạng thái
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 98px" aria-label="Total: activate to sort column ascending">
                Tổng giá
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 170px" aria-label="Issued Date: activate to sort column ascending">
                Ngày hết hạn
            </th>
            <th class="sorting" tabindex="0" aria-controls="DataTables_Table_1" rowspan="1" colspan="1" style="width: 170px" aria-label="Issued Date: activate to sort column ascending">
                Hành động
            </th>
        </tr>
    </thead>
    <tbody>
        <tr v-for="(invoice, index) in invoicesData" :key="invoice.id" class="odd">
            <td class="dt-checkboxes-cell">
                <input :checked="selectedInvoices.includes(invoice.id)" class="dt-checkboxes form-check-input" type="checkbox" @change="toggleInvoiceSelection(invoice.id)" />
            </td>
            <td class="sorting_1">
                <a href="#"><span>#{{ invoice.id }}</span></a>
            </td>
            <td>
                <div class="tooltip-container">
                    <span v-if="invoice.status === 2" class="badge badge-center d-flex align-items-center justify-content-center rounded-pill bg-label-success w-px-30 h-px-30">
                        <i class="bx bx-check bx-xs"></i>
                    </span>
                    <div class="tooltip-content">
                        <p>{{ invoice.statusName }}</p>
                        <p>Balance: {{ formatCurrency(invoice.totalAmount) }}</p>
                        <p>Due Date: {{ formatDate(invoice.dueDate) }}</p>
                    </div>
                </div>
                <div class="tooltip-container">
                    <span v-if="invoice.status === 3" class="badge badge-center d-flex align-items-center justify-content-center rounded-pill bg-label-danger w-px-30 h-px-30">
                        <i class="bx bx-error bx-xs"></i>
                    </span>
                    <div class="tooltip-content">
                        <p>{{ invoice.statusName }}</p>
                        <p>Balance: {{ formatCurrency(invoice.totalAmount) }}</p>
                        <p>Due Date: {{ formatDate(invoice.dueDate) }}</p>
                    </div>
                </div>
                <div class="tooltip-container">
                    <span v-if="invoice.status === 1" class="badge badge-center d-flex align-items-center justify-content-center rounded-pill bg-label-warning w-px-30 h-px-30">
                        <i class="bx bx-pie-chart-alt bx-xs"></i>
                    </span>
                    <div class="tooltip-content">
                        <p>{{ invoice.statusName }}</p>
                        <p>Balance: {{ formatCurrency(invoice.totalAmount) }}</p>
                        <p>Due Date: {{ formatDate(invoice.dueDate) }}</p>
                    </div>
                </div>
            </td>
            <td>{{ formatCurrency(invoice.totalAmount)}}</td>
            <td>{{ formatDate(invoice.dueDate)}}</td>
            <td>
            <td>
                <div class="action-icons">
                    <a href="javascript:;" class="dropdown-item" @click="handleDetailClick(invoice.id)">
                        <i class="bx bx-edit-alt me-1"></i>
                    </a>
                    <a v-if="invoice.status === 1" href="javascript:;" class="dropdown-item" @click="handlePayment(invoice.id)">
                        <i class="bx bx-cart-alt me-1"></i>
                    </a>
                </div>
            </td>

            </td>
        </tr>
    </tbody>
</table>
<ModalConfirmDeleteComponent :is-show-modal="isShowDeleteModal" :message="'Bạn có chắc chắn muốn xóa hóa đơn ?'" :id="invoiceId" :before-close="handleCloseDeleteModal" @confirm-action="handleConfirmDeleteModal" @close-modal="handleCloseDeleteModal" />
</template>

<script lang="ts">
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';
import {
    InvoiceData
} from '@/types/model/invoice';
import {
    defineComponent,
    PropType,
    ref,
    watch,
} from 'vue';
export default defineComponent({
    name: 'InvoiceTableComponent',
    props: {
        invoicesData: {
            type: Array as() => Array < InvoiceData > ,
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
        invoiceId: {
            type: Number,
            required: true,
        },
        isShowDeleteModal: {
            type: Boolean,
            required: true,
        }
    },
    components: {
        ModalConfirmDeleteComponent
    },
    emits: ['handleDetailClick', 'handleConfirmDeleteModal', 'handlePayment', 'handleCloseDeleteModal', 'delete-invoices'],
    setup(props, {
        emit
    }) {
        const selectedInvoices = ref < number[] > ([]);
        const selectAll = ref(false);

        const selectAllRows = () => {
            if (selectAll.value) {
                selectedInvoices.value = [];
            } else {
                selectedInvoices.value = props.invoicesData.map(invoice => invoice.id);
            }
            selectAll.value = !selectAll.value;

            emit('delete-invoices', selectedInvoices.value);
        };

        watch(
            () => selectedInvoices.value,
            (newSelected) => {
                selectAll.value = newSelected.length === props.invoicesData.length;
            }, {
                immediate: true
            },
        );
        const toggleInvoiceSelection = (invoiceId: number) => {
            if (selectedInvoices.value.includes(invoiceId)) {
                selectedInvoices.value = selectedInvoices.value.filter(id => id !== invoiceId);
            } else {
                selectedInvoices.value.push(invoiceId);
            }
            emit('delete-invoices', selectedInvoices.value);
        };
        const handleDetailClick = (invoiceId: number) => {
            emit('handleDetailClick', invoiceId);
        };

        const handleConfirmDeleteModal = (invoiceId: number) => {
            emit('handleConfirmDeleteModal', invoiceId);
        };

        const handlePayment = (invoiceId: number) => {
            emit('handlePayment', invoiceId);
        };

        const handleCloseDeleteModal = () => {
            emit('handleCloseDeleteModal');
        };

        return {
            handleDetailClick,
            handleConfirmDeleteModal,
            handlePayment,
            handleCloseDeleteModal,
            selectAllRows,
            toggleInvoiceSelection,
            selectAll,
            selectedInvoices,
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
.action-icons {
    display: flex;
    align-items: center;
    gap: 10px; 
}

.action-icons a {
    display: inline-flex;
    align-items: center;
    justify-content: center;
}
</style>
