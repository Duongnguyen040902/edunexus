<template>
    <ModalComponent :modelValue="showAssignPupilModal" @update:modelValue="$emit('update:showModal', $event)"
        :width="`60%`">
        <div class="modal-header">
            <h3>Danh sách học sinh</h3>
        </div>
        <el-input v-model="searchTerm" placeholder="Tìm kiếm theo tên hoặc mã học sinh" clearable
            class="search-input" />
        <el-form class="table-container">
            <el-table :data="filteredPupils" class="pupil-table"
                empty-text="Không có học sinh chưa tham gia trong kì này!">
                <el-table-column label="Hình ảnh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
                    <template #default="{ row }">
                        <el-image :src="`${apiUrl}${row.image}`" alt="Hình ảnh học sinh"
                            style="width: 40px; height: 40px; object-fit: cover; border-radius: 50%" />
                    </template>
                </el-table-column>

                <el-table-column label="Mã học sinh" prop="username"
                    :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
                    <template #default="{ row }">
                        <span :style="row.clubId ? { color: 'green', fontWeight: 'bold' } : {}">
                            {{ row.userName }}
                            <span v-if="row.clubId" style="color: red; font-size: 16px">*</span>
                        </span>
                    </template>
                </el-table-column>
                <el-table-column label="Tên học sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
                    <template #default="{ row }">
                        <span :style="row.clubId ? { color: 'green', fontWeight: 'bold' } : {}">
                            {{ row.firstName + ' ' + row.lastName }}
                        </span>
                    </template>
                </el-table-column>
                <el-table-column label="Giới tính" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
                    <template #default="{ row }">
                        <span :style="row.clubId ? { color: 'green', fontWeight: 'bold' } : {}">
                            {{ row.gender ? 'Nam' : 'Nữ' }}
                        </span>
                    </template>
                </el-table-column>
                <el-table-column label="Ngày sinh" :header-cell-style="{ fontWeight: 'bold', color: '#409eff' }">
                    <template #default="{ row }">
                        <span :style="row.clubId ? { color: 'green', fontWeight: 'bold' } : {}">
                            {{ formatDate(row.dateOfBirth) }}
                        </span>
                    </template>
                </el-table-column>
                <el-table-column label="Chọn"
                    :header-cell-style="{ fontWeight: 'bold', color: '#409eff', textAlign: 'center' }">
                    <template #default="{ row }">
                        <el-checkbox v-model="selectedPupilIds" :value="row.id" class="hidden-label"></el-checkbox>
                    </template>
                </el-table-column>
            </el-table>
        </el-form>
        <template #footer>
            <div class="dialog-footer">
                <el-button @click="closeModal()">HỦY</el-button>
                <el-button type="primary" @click="handleModalConfirm" class="custom-confirm-button" :disabled="selectedPupilIds.length === 0">Lưu</el-button>
            </div>
        </template>
    </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, computed } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { PupilDTOResponse } from '@/types/model/club-enrollment';

export default defineComponent({
    name: 'AssignPupilToClubModal',
    components: {
        ModalComponent,
    },
    props: {
        showAssignPupilModal: {
            type: Boolean,
        },
        pupils: {
            type: Array as PropType<PupilDTOResponse[]>,
            default: () => [],
        },
        clubId: {
            type: Number,
        },
        semesterId: {
            type: Number,
        },
        apiUrl: {
            type: String,
        },
    },
    emits: ['update:showModal', 'confirm'],
    setup(props, { emit }) {
        const selectedPupilIds = ref<number[]>([]);
        const searchTerm = ref('');
        const filteredPupils = computed(() => {
            const pupils = Array.isArray(props.pupils.value) ? props.pupils.value : [];
            return pupils.filter(
                pupil =>
                    (pupil.firstName + ' ' + pupil.lastName).toLowerCase().includes(searchTerm.value.toLowerCase()) ||
                    pupil.id.toString().includes(searchTerm.value),
            );
        });


        const formatDate = (dateString: string) => {
            const date = new Date(dateString);
            return date.toLocaleDateString('vi-VN');
        };

        const handleModalConfirm = () => {
            const pupilsToAssign = selectedPupilIds.value.map(pupilId => ({
                pupilId,
                clubId: props.clubId,
                semesterId: props.semesterId,
                status: 2
            }));
            emit('confirm', pupilsToAssign);
            emit('update:showModal', false);
            resetForm();
        };

        const closeModal = () => {
            emit('update:showModal', false);
            resetForm();
        };

        const resetForm = () => {
            selectedPupilIds.value = [];
            searchTerm.value = '';
        };

        return {
            formatDate,
            selectedPupilIds,
            handleModalConfirm,
            filteredPupils,
            searchTerm,
            closeModal,
        };
    },
});
</script>