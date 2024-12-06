<template>
  <table
        id="DataTables_Table_0"
        class="invoice-list-table table border-top dataTable no-footer dtr-column"
        style="width: 1391px"
    >
      <thead>
      <tr>
        <th class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all" style="width: 18px">
          <input :checked="selectAll" class="form-check-input" type="checkbox" @change="selectAllRows"/>
        </th>
        <th class="sorting sorting_desc" style="width: 88px">#</th>
        <th class="sorting" style="width: 346px">Trường</th>
        <th class="sorting" style="width: 91px">Giá</th>
        <th class="text-truncate sorting" style="width: 159px">Ngày ban hành</th>
        <th class="text-truncate sorting" style="width: 159px">Ngày đến hạn</th>
        <th class="sorting_disabled" style="width: 121px">Trạng thái</th>
        <th class="cell-fit sorting_disabled" style="width: 118px">Hành động</th>
      </tr>
      </thead>
      <tbody>
      <tr v-if="invoices.length === 0">
        <td colspan="8" class="text-center">Không có dữ liệu</td>
      </tr>
      <tr v-else v-for="(invoice, index) in invoices" :key="invoice.id" class="odd">
        <td class="dt-checkboxes-cell">
          <input
              :checked="selectedInvoices.includes(invoice.id)"
              class="dt-checkboxes form-check-input"
              type="checkbox"
              @change="toggleInvoiceSelection(invoice.id)"
          />
        </td>
        <td class="sorting_1">
          <a href="#">#{{ invoice.id }}</a>
        </td>
        <td>
          <div class="d-flex justify-content-start align-items-center">
            <div class="avatar-wrapper">
              <div class="avatar avatar-sm me-3">
                <span class="avatar-initial rounded-circle bg-label-primary">JK</span>
              </div>
            </div>
            <div class="d-flex flex-column">
              <a class="text-heading text-truncate" href="#"
              ><span class="fw-medium">{{ invoice.schoolName }}</span></a
              ><small class="text-truncate">{{ invoice.subscriptionPlanName }}</small>
            </div>
          </div>
        </td>
        <td>
            <span class="d-none">{{ invoice.totalAmount }}</span
            >{{ invoice.totalAmount }}
        </td>
        <td>
            <span class="d-none">{{ formatDate(invoice.issueDate) }}</span
            >{{ formatDate(invoice.issueDate) }}
        </td>
        <td>
            <span class="d-none">{{ formatDate(invoice.dueDate) }}</span
            >{{ formatDate(invoice.dueDate) }}
        </td>
        <td>
            <span :class="getClassByStatus(invoice.status)" class="badge bg-label-success">
              {{ invoice.statusName }}
            </span>
        </td>
        <td>
          <div class="d-flex align-items-center">
            <router-link
                :to="{ path: ROUTER_PATHS.ADMIN.INVOICE_DETAIL.replace(':id', invoice.id.toString()) }"
                aria-label="Preview Invoice"
                class="btn btn-icon"
                data-bs-original-title="Preview Invoice"
                data-bs-placement="top"
                data-bs-toggle="tooltip"
            >
              <i class="bx bx-show bx-md"></i>
            </router-link>
          </div>
        </td>
      </tr>
      </tbody>
    </table>
</template>

<script lang="ts">
import {defineComponent, ref, watch} from 'vue';
import {InvoiceData} from '@/types/model/invoice.ts';
import {InvoiceStatus} from '@/constants/enums/statuses';
import router from '@/router';
import {ROUTER_PATHS} from '@/constants/api/router-paths.ts';
import { formatDate } from '@/helpers/formatDate';
import { useInvoiceManagerComposable } from '@/composables/invoice-manager';
export default defineComponent({
  name: 'InvoiceTableIndexComponent',
  components: {},
  props: {
    invoices: {
      type: Array as () => Array<InvoiceData>,
      required: true,
    },
  },
  emits: ['delete-invoices'],
  setup(props, {emit}) {
    const invoiceManagerComposable = useInvoiceManagerComposable();
    const {
      selectedInvoices,
      selectAll,
    } = invoiceManagerComposable;

    const selectAllRows = () => {
      selectedInvoices.value = selectAll.value ? [] : props.invoices.map(invoice => invoice.id);
      selectAll.value = !selectAll.value;
      emit('delete-invoices', selectedInvoices.value);
    };

    watch(
        () => selectedInvoices.value.length,
        () => {
          selectAll.value = selectedInvoices.value.length === props.invoices.length;
        },
    );

    const getClassByStatus = (status: number) => {
      switch (status) {
      case InvoiceStatus.PAID:
        return 'bg-label-success';
      case InvoiceStatus.PENDING:
        return 'bg-label-warning';
      case InvoiceStatus.CANCELED:
        return 'bg-label-danger';
      case InvoiceStatus.SENT:
        return 'bg-label-danger';
      default:
        return 'bg-label-primary';
      }
    };

    const toggleInvoiceSelection = (invoiceId: number) => {
      if (selectedInvoices.value.includes(invoiceId)) {
        selectedInvoices.value = selectedInvoices.value.filter(id => id !== invoiceId);
      } else {
        selectedInvoices.value.push(invoiceId);
      }
      emit('delete-invoices', selectedInvoices.value);
    };

    const handleRedirectDetailInvoice = (invoiceId: number) => {
      router.push({path: ROUTER_PATHS.ADMIN.INVOICE_DETAIL.replace(':id', invoiceId.toString())});
    };

    return {
      formatDate,
      ROUTER_PATHS,
      selectedInvoices,
      selectAll,
      selectAllRows,
      getClassByStatus,
      toggleInvoiceSelection,
      handleRedirectDetailInvoice,
    };
  },
});
</script>
