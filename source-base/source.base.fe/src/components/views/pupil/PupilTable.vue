//TeacherTable.vue
<template>
<div class="card-datatable table-responsive" style="margin-top: 30px;">
    <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed">
        <thead>
            <tr>
                <th class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all" style="width: 18px">
                    <input :checked="selectAll" class="form-check-input" type="checkbox" @change="selectAllRows" />
                </th>
                <th>Học sinh</th>
                <th>Tài khoản</th>
                <th>Ngày sinh</th>
                <th>Giới tính</th>
                <th>Người giám hộ</th>
                <th>Liên hệ</th>
                <th>Trạng thái</th>
                <th class="dtr-hidden">Actions</th>
            </tr>
        </thead>
        <tbody v-if="pupilsData.length > 0">
            <tr v-for="pupil in pupilsData">
                <td class="dt-checkboxes-cell">
                    <input class="dt-checkboxes form-check-input" type="checkbox" :checked="selectedPupils.includes(pupil.id)" @change="togglePupilSelection(pupil.id)" />
                </td>
                <td>
                    <div class="d-flex justify-content-start align-items-center user-name">
                        <div class="avatar-wrapper">
                            <div class="avatar avatar-sm me-4">
                                <img :src="`${apiUrl}${pupil.image}`" alt="Avatar" class="rounded-circle" />
                            </div>
                        </div>
                        <div class="d-flex flex-column">
                            <a href="#" class="text-heading text-truncate"><span class="fw-medium">{{ pupil.firstName + ' ' + pupil.lastName }}</span></a><small>{{ pupil.email }}</small>
                        </div>
                    </div>
                </td>
                <td>
                    <span class="text-truncate d-flex align-items-center text-heading"><i class="bx bx-user text-success me-2"></i>{{ pupil.username }}</span>
                </td>
                <td>{{ formatDate(pupil.dateOfBirth) }}</td>
                <td>{{ pupil.genderName }}</td>
                <td>{{ pupil.donorName }}</td>
                <td>{{ pupil.donorPhoneNumber }}</td>
                <td>
                    <span :class="{ 
                    'badge bg-label-success': pupil.accountStatus === 1, 
                    'badge bg-label-warning': pupil.accountStatus === 2,
                    'badge bg-label-danger': pupil.accountStatus === 3 }" text-capitalized="">
                        {{ pupil.accountStatusName }}
                    </span>
                </td>
                <td>
                    <a class="dropdown-item" href="javascript:void(0);" @click="$emit('showDetail', pupil.id)">
                        <i class="bx bx-edit-alt me-1"></i>
                    </a>
                </td>
            </tr>
        </tbody>
        <tbody v-else>
            <tr>
                <td colspan="10" class="text-center">Không có học sinh nào</td>
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
    name: 'PupilTableComponent',
    props: {
        pupilsData: {
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
    emits: ['delete-pupils'],
    setup(props, {
        emit
    }) {
        const selectedPupils = ref < number[] > ([]);
        const selectAll = ref(false);

        const selectAllRows = () => {
            selectedPupils.value = selectAll.value ? [] : props.pupilsData.map(pupil => pupil.id);
            selectAll.value = !selectAll.value;
            emit('delete-pupils', selectedPupils.value);
        };

        watch(
            () => selectedPupils.value.length,
            () => {
                selectAll.value = selectedPupils.value.length === props.pupilsData.length;
            },
        );

        const togglePupilSelection = (pupilId: number) => {
            if (selectedPupils.value.includes(pupilId)) {
                selectedPupils.value = selectedPupils.value.filter(id => id !== pupilId);
            } else {
                selectedPupils.value.push(pupilId);
            }
            emit('delete-pupils', selectedPupils.value);
        };

        return {
            selectedPupils,
            selectAll,
            selectAllRows,
            togglePupilSelection,
        };
    },
});
</script>
