<template>
<div class="card-datatable table-responsive" style="margin-top: 30px;">
    <template v-if="teachersData.length > 0">
        <table class="datatables-users table border-top dataTable no-footer dtr-column collapsed">
            <thead>
                <tr>
                    <th class="sorting_disabled dt-checkboxes-cell dt-checkboxes-select-all" style="width: 18px">
                        <input :checked="selectAll" class="form-check-input" type="checkbox" @change="selectAllRows" />
                    </th>
                    <th>Giáo viên</th>
                    <th>Tài khoản</th>
                    <th>Chuyên môn</th>
                    <th>Ngày sinh</th>
                    <th>Giới tính</th>
                    <th>Trạng thái</th>
                    <th class="dtr-hidden">Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="teacher in teachersData" :key="teacher.id">
                    <td class="dt-checkboxes-cell">
                        <input 
                        class="dt-checkboxes form-check-input" 
                        type="checkbox" 
                        :checked="selectedTeachers.includes(teacher.id)" 
                        @change="toggleTeacherSelection(teacher.id)" 
                        />
                    </td>
                    <td>
                        <div class="d-flex justify-content-start align-items-center user-name">
                            <div class="avatar-wrapper">
                                <div class="avatar avatar-sm me-4">
                                    <img :src="`${apiUrl}${teacher.image}`" alt="Avatar" class="rounded-circle" />
                                </div>
                            </div>
                            <div class="d-flex flex-column">
                                <a href="#" class="text-heading text-truncate"><span class="fw-medium">{{ teacher.firstName + ' ' + teacher.lastName }}</span></a>
                                <small>{{ teacher.email }}</small>
                            </div>
                        </div>
                    </td>
                    <td>
                        <span class="text-truncate d-flex align-items-center text-heading">
                            <i class="bx bx-user text-success me-2"></i>{{ teacher.username }}
                        </span>
                    </td>
                    <td>
                        <el-tooltip class="item" effect="dark" :content="teacher.subjects.join(', ')" placement="top-start">
                            <span class="text-heading">
                                {{ teacher.subjects.slice(0, 2).join(', ') }}
                                <template v-if="teacher.subjects.length > 2">, ...</template>
                            </span>
                        </el-tooltip>
                    </td>
                    <td>{{ formatDate(teacher.dateOfBirth) }}</td>
                    <td>{{ teacher.genderName }}</td>
                    <td>
                        <span :class="{ 
                        'badge bg-label-success': teacher.accountStatus === 1, 
                        'badge bg-label-warning': teacher.accountStatus === 2,
                        'badge bg-label-danger': teacher.accountStatus === 3
                        }">
                            {{ teacher.accountStatusName }}
                        </span>
                    </td>
                    <td>
                        <a class="dropdown-item" href="javascript:void(0);" @click="$emit('showDetail', teacher.id)">
                            <i class="bx bx-edit-alt me-1"></i>
                        </a>
                    </td>
                </tr>
            </tbody>
        </table>
    </template>
    <template v-else>
        <div class="text-center py-5">
            <span class="text-muted">Chưa có giáo viên được phân công</span>
        </div>
    </template>
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
    name: 'TeacherTableComponent',
    props: {
        teachersData: {
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
    emits: ['delete-teachers'],
    setup(props, {
        emit
    }) {
        const selectedTeachers = ref < number[] > ([]);
        const selectAll = ref(false);

        const selectAllRows = () => {
            selectedTeachers.value = selectAll.value ? [] : props.teachersData.map(teacher => teacher.id);
            selectAll.value = !selectAll.value;
            emit('delete-teachers', selectedTeachers.value);
        };

        watch(
            () => selectedTeachers.value.length,
            () => {
                selectAll.value = selectedTeachers.value.length === props.teachersData.length;
            },
        );

        const toggleTeacherSelection = (teacherId: number) => {
            if (selectedTeachers.value.includes(teacherId)) {
                selectedTeachers.value = selectedTeachers.value.filter(id => id !== teacherId);
            } else {
                selectedTeachers.value.push(teacherId);
            }
            emit('delete-teachers', selectedTeachers.value);
        };

        return {
            selectedTeachers,
            selectAll,
            selectAllRows,
            toggleTeacherSelection,
        };
    },
});
</script>
