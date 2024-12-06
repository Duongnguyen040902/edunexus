<template>
<ModalComponent :model-value="isShowCreate" :beforeClose="handleClose">
    <div class="modal-body">
        <div class="text-center mb-6">
            <h4 class="mb-2" v-if="isCreateBusSupervisor">Thêm người phụ trách xe tuyến</h4>
            <h4 class="mb-2" v-else>Chi tiết người phụ trách xe tuyến</h4>
        </div>
        <el-button type="primary" v-if="!isCreateBusSupervisor && !isUpdateBusSupervisor" @click="openEditModal">Chỉnh sửa</el-button>
        <el-form label-position="top" ref="pupilForm" class="form-content">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Họ">
                        <el-input v-model="requestDataCreateBusSupervisor.firstName" :disabled="!isCreateBusSupervisor && !isUpdateBusSupervisor"></el-input>
                        <div v-if="errorsBusSupervisor.FirstName" class="text-danger">{{ errorsBusSupervisor.FirstName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Số điện thoại">
                        <el-input v-model="requestDataCreateBusSupervisor.phoneNumber" :disabled="!isCreateBusSupervisor && !isUpdateBusSupervisor"></el-input>
                        <div v-if="errorsBusSupervisor.PhoneNumber" class="text-danger">{{ errorsBusSupervisor.PhoneNumber[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tên">
                        <el-input v-model="requestDataCreateBusSupervisor.lastName" :disabled="!isCreateBusSupervisor && !isUpdateBusSupervisor"></el-input>
                        <div v-if="errorsBusSupervisor.LastName" class="text-danger">{{ errorsBusSupervisor.LastName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Giới tính">
                        <el-radio-group v-model="requestDataCreateBusSupervisor.gender" :disabled="!isCreateBusSupervisor && !isUpdateBusSupervisor">
                            <el-radio :label="true">Nam</el-radio>
                            <el-radio :label="false">Nữ</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12" v-if="!isCreateBusSupervisor">
                    <el-form-item label="Email">
                        <el-input v-model="requestDataCreateBusSupervisor.email" 
                        :disabled="(!isCreateBusSupervisor && !isUpdateBusSupervisor) || (isUpdateBusSupervisor && requestDataCreateBusSupervisor.email === null || requestDataCreateBusSupervisor.email === '')">
                        </el-input>
                        <div v-if="errorsBusSupervisor.LastName" class="text-danger">{{ errorsBusSupervisor.LastName[0] }}</div>
                    </el-form-item>
                </el-col> 
                <el-col :span="12">
                    <el-form-item label="Địa chỉ">
                        <el-input v-model="requestDataCreateBusSupervisor.address" :disabled="!isCreateBusSupervisor && !isUpdateBusSupervisor"></el-input>
                        <div v-if="errorsBusSupervisor.Address" class="text-danger">{{ errorsBusSupervisor.Address[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item v-if="!isCreateBusSupervisor" label="Trạng thái tài khoản">
                        <el-select v-model="requestDataCreateBusSupervisor.accountStatus" :disabled="!isUpdateBusSupervisor" placeholder="Trạng thái">
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
                <el-button class="btn btn-secondary" @click="handleClose">{{ isCreateBusSupervisor ? 'Hủy' : 'Quay Lại' }}</el-button>
                <el-button v-if="isCreateBusSupervisor" type="primary" @click="handleSave" class="btn btn-primary">
                    Thêm
                </el-button>
                <el-button v-else-if="isUpdateBusSupervisor" type="primary" @click="handleEditBusSupervisor" class="btn btn-primary">
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
        isCreateBusSupervisor: {
            type: Boolean,
            required: true,
        },
        isUpdateBusSupervisor: {
            type: Boolean,
            required: true,
        },
        requestDataCreateBusSupervisor: {
            type: Object,
            required: true,
        },
        errorsBusSupervisor: {
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
    emits: ['handleSave', 'handleClose', 'handleEditBusSupervisor', 'openEditModal'],
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

        const handleEditBusSupervisor = () => {
            emit('handleEditBusSupervisor');
        };

        return {
            handleSave,
            handleClose,
            openEditModal,
            handleEditBusSupervisor,
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
