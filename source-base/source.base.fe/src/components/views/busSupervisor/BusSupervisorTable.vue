//TeacherTable.vue
<template>
<div class="card-datatable table-responsive" style="margin-top: 30px;">
    <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed">
        <thead>
            <tr>
                <th class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all" style="width: 18px">
                    <input :checked="selectAll" class="form-check-input" type="checkbox" @change="selectAllRows" />
                </th>
                <th>Người phụ trách</th>
                <th>Tài khoản</th>
                <th>Giới tính</th>
                <th>Liên hệ</th>
                <th>Trạng thái</th>
                <th class="dtr-hidden">Hành động</th>
            </tr>
        </thead>
        <tbody v-if="listBusSupervisorResponse.length > 0">
            <tr v-for="busSupervisor in listBusSupervisorResponse">
                <td class="dt-checkboxes-cell">
                    <input class="dt-checkboxes form-check-input" type="checkbox" 
                    :checked="selectedBusSupervisors.includes(busSupervisor.id)" 
                    @change="togglePupilSelection(busSupervisor.id)" />
                </td>
                <td>
                    <div class="d-flex justify-content-start align-items-center user-name">
                        <div class="avatar-wrapper">
                            <div class="avatar avatar-sm me-4">
                                <img :src="`${apiUrl}${busSupervisor.image}`" alt="Avatar" class="rounded-circle" />
                            </div>
                        </div>
                        <div class="d-flex flex-column">
                            <a href="#" class="text-heading text-truncate"><span class="fw-medium">{{ busSupervisor.firstName + ' ' + busSupervisor.lastName }}</span></a><small>{{ busSupervisor.email }}</small>
                        </div>
                    </div>
                </td>
                <td>
                    <span class="text-truncate d-flex align-items-center text-heading"><i class="bx bx-user text-success me-2"></i>{{ busSupervisor.username }}</span>
                </td>
                <td>{{ busSupervisor.genderName }}</td>
                <td>{{ busSupervisor.phoneNumber }}</td>
                <td>
                    <span :class="{ 
                    'badge bg-label-success': busSupervisor.accountStatus === 1, 
                    'badge bg-label-warning': busSupervisor.accountStatus === 2,
                    'badge bg-label-danger': busSupervisor.accountStatus === 3 }" text-capitalized="">
                        {{ busSupervisor.accountStatusName }}
                    </span>
                </td>
                <td>
                    <a class="dropdown-item" href="javascript:void(0);" @click="$emit('showDetail', busSupervisor.id)">
                        <i class="bx bx-edit-alt me-1"></i>
                    </a>
                </td>
            </tr>
        </tbody>
        <tbody v-else>
            <tr>
                <td colspan="10" class="text-center">Không có người dùng nào</td>
            </tr>
        </tbody>
    </table>
</div>
</template>

<script lang="ts">
import {
    defineComponent,
    PropType,
    ref,
    watch,
} from 'vue';
export default defineComponent({
    name: 'BusSupervisorTableComponent',
    props: {
        listBusSupervisorResponse: {
            type: Array,
            default: () => []
        },
        apiUrl: {
            type: String,
            required: true,
        },
        formatDate: {
            type: Function as PropType < (date: string) => string > ,
            required: true,
        },
    },
    emits: ['delete-busSupervisor'],
    setup(props, {
        emit
    }) {
        const selectedBusSupervisors = ref < number[] > ([]);
        const selectAll = ref(false);

        const selectAllRows = () => {
            selectedBusSupervisors.value = selectAll.value ? [] : props.listBusSupervisorResponse.map(busSupervisor => busSupervisor.id);
            selectAll.value = !selectAll.value;
            emit('delete-busSupervisor', selectedBusSupervisors.value);
        };

        watch(
            () => selectedBusSupervisors.value.length,
            () => {
                selectAll.value = selectedBusSupervisors.value.length === props.listBusSupervisorResponse.length;
            },
        );

        const togglePupilSelection = (busSupervisorsId: number) => {
            if (selectedBusSupervisors.value.includes(busSupervisorsId)) {
                selectedBusSupervisors.value = selectedBusSupervisors.value.filter(id => id !== busSupervisorsId);
            } else {
                selectedBusSupervisors.value.push(busSupervisorsId);
            }
            emit('delete-busSupervisor', selectedBusSupervisors.value);
        };

        return {
            selectedBusSupervisors,
            selectAll,
            selectAllRows,
            togglePupilSelection,
        };
    },
});
</script>
