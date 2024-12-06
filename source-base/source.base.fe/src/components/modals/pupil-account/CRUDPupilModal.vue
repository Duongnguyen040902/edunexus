<template>
<ModalComponent :model-value="isShowCreate" :beforeClose="handleClose">
    <div class="modal-body">
        <div class="text-center mb-6">
            <h4 class="mb-2" v-if="isCreatePupil">Thêm học sinh</h4>
            <h4 class="mb-2" v-else>Chi tiết học sinh</h4>
        </div>
        <el-button type="primary" v-if="!isCreatePupil && !isUpdatePupil" @click="openEditModal">Chỉnh sửa</el-button>
        <el-form label-position="top" ref="pupilForm" class="form-content">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Họ">
                        <el-input v-model="requestDataCreatePupil.firstName" :disabled="!isCreatePupil && !isUpdatePupil"></el-input>
                        <div v-if="errorsPupil.FirstName" class="text-danger">{{ errorsPupil.FirstName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Số điện thoại">
                        <el-input v-model="requestDataCreatePupil.donorPhoneNumber" :disabled="!isCreatePupil && !isUpdatePupil"></el-input>
                        <div v-if="errorsPupil.DonorPhoneNumber" class="text-danger">{{ errorsPupil.DonorPhoneNumber[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tên">
                        <el-input v-model="requestDataCreatePupil.lastName" :disabled="!isCreatePupil && !isUpdatePupil"></el-input>
                        <div v-if="errorsPupil.LastName" class="text-danger">{{ errorsPupil.LastName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Tên người giám hộ">
                        <el-input v-model="requestDataCreatePupil.donorName" :disabled="!isCreatePupil && !isUpdatePupil"></el-input>
                        <div v-if="errorsPupil.DonorName" class="text-danger">{{ errorsPupil.DonorName[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Giới tính">
                        <el-radio-group v-model="requestDataCreatePupil.gender" :disabled="!isCreatePupil && !isUpdatePupil">
                            <el-radio :label="true">Nam</el-radio>
                            <el-radio :label="false">Nữ</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Địa chỉ">
                        <el-input v-model="requestDataCreatePupil.address" :disabled="!isCreatePupil && !isUpdatePupil"></el-input>
                        <div v-if="errorsPupil.Address" class="text-danger">{{ errorsPupil.Address[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Ngày sinh">
                        <el-date-picker v-model="requestDataCreatePupil.dateOfBirth" type="date" placeholder="Chọn ngày" :disabled="!isCreatePupil && !isUpdatePupil"></el-date-picker>
                    </el-form-item>
                    <div v-if="errorsPupil.DateOfBirth" class="text-danger">{{ errorsPupil.DateOfBirth[0] }}</div>
                </el-col>
                <el-col :span="12" v-if="!isCreatePupil">
                    <el-form-item label="Email">
                        <el-input v-model="requestDataCreatePupil.email" 
                        :disabled="(!isCreatePupil && !isUpdatePupil) || (isUpdatePupil && requestDataCreatePupil.email === null || requestDataCreatePupil.email === '')">
                        </el-input>
                        <div v-if="errorsPupil.LastName" class="text-danger">{{ errorsPupil.LastName[0] }}</div>
                    </el-form-item>
                </el-col>  
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item v-if="isUpdatePupil" label="Trạng thái tài khoản">
                        <el-select v-model="requestDataCreatePupil.accountStatus" placeholder="Trạng thái">
                            <el-option v-for="status in statuses" :key="status.id" :value="status.id" :label="status.name">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                </el-col>
            </el-row>
        </el-form>
    </div>
    <template #footer>
        <div class="dialog-footer">
            <slot name="footer">
                <el-button class="btn btn-secondary" @click="handleClose">{{ isCreatePupil ? 'Hủy' : 'Quay Lại' }}</el-button>
                <el-button v-if="isCreatePupil" type="primary" @click="handleSave" class="btn btn-primary">
                    Thêm
                </el-button>
                <el-button v-else-if="isUpdatePupil" type="primary" @click="handleEditPupil" class="btn btn-primary">
                    Lưu
                </el-button>
            </slot>
        </div>
    </template>
</ModalComponent>
</template>

    
<script lang="ts">
import {
    defineComponent,
    ref,
    watch,
} from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import {
    Upload
} from 'ant-design-vue';

export default defineComponent({
    name: 'CRUDPupilModal',
    components: {
        ModalComponent,
    },
    props: {
        isShowCreate: {
            type: Boolean,
            required: true,
        },
        isCreatePupil: {
            type: Boolean,
            required: true,
        },
        isUpdatePupil: {
            type: Boolean,
            required: true,
        },
        requestDataCreatePupil: {
            type: Object,
            required: true,
        },
        errorsPupil: {
            type: Object,
            required: true,
        },
        apiUrl: {
            type: String,
            required: true,
        },
        statuses: {
            type: Array,
            default: () => [{
                    id: 1,
                    name: 'Kích hoạt',
                },
                {
                    id: 2,
                    name: 'Không kích hoạt',
                },
                {
                    id: 3,
                    name: 'Vô hiệu hóa',
                },
            ],
        },
    },
    emits: ['handleSave', 'handleClose', 'handleEditPupil', 'openEditModal'],
    setup(props, {
        emit
    }) {

        const handleSave = () => {
            emit('handleSave');
        };

        const handleClose = () => {
            emit('handleClose');
        };



        const openEditModal = () => {
            emit('openEditModal');
        };

        const handleEditPupil = () => {
            emit('handleEditPupil');
        };

        return {
            handleSave,
            handleClose,
            openEditModal,
            handleEditPupil,
        };
    },
});
</script>

    
<style scoped>
.form-content {
    padding: 10px;
}

.dialog-footer {
    text-align: right;
}
</style>
