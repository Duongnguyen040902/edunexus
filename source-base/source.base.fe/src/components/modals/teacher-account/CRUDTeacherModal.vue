<template>
<ModalComponent :model-value="isShowCreate" :beforeClose="handleClose">
    <div class="modal-body">
        <div class="text-center mb-6">
            <h4 class="mb-2" v-if="isCreateTeacher">Thêm giáo viên</h4>
            <h4 class="mb-2" v-else>Chi tiết giáo viên</h4>
        </div>
        <el-button type="primary" v-if="!isCreateTeacher && !isUpdateTeacher" @click="openEditModal">Chỉnh sửa</el-button>
        <el-form label-position="top" ref="teacherForm" class="form-content">
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Họ">
                        <el-input v-model="requestDataCreateTeacher.firstName" :disabled="!isCreateTeacher && !isUpdateTeacher"></el-input>
                        <div v-if="errorsCreateTeacher.FirstName" class="text-danger">{{ errorsCreateTeacher.FirstName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Số điện thoại">
                        <el-input v-model="requestDataCreateTeacher.phoneNumber" :disabled="!isCreateTeacher && !isUpdateTeacher"></el-input>
                        <div v-if="errorsCreateTeacher.PhoneNumber" class="text-danger">
                            {{ errorsCreateTeacher.PhoneNumber[0] }}
                        </div>
                    </el-form-item>
                </el-col>
            </el-row>

            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Tên">
                        <el-input v-model="requestDataCreateTeacher.lastName" :disabled="!isCreateTeacher && !isUpdateTeacher"></el-input>
                        <div v-if="errorsCreateTeacher.LastName" class="text-danger">{{ errorsCreateTeacher.LastName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Chuyên môn">
                        <el-select v-model="requestDataCreateTeacher.subjectIds" multiple placeholder="Chọn chuyên môn" :disabled="!isCreateTeacher && !isUpdateTeacher">
                            <el-option v-for="subject in listSubjects" :key="subject.id" :label="subject.name" :value="subject.id"></el-option>
                        </el-select>
                        <div v-if="errorsCreateTeacher.SubjectIds" class="text-danger">
                            {{ errorsCreateTeacher.SubjectIds[0] }}
                        </div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Giới tính">
                        <el-radio-group v-model="requestDataCreateTeacher.gender" :disabled="!isCreateTeacher && !isUpdateTeacher">
                            <el-radio :label="true">Nam</el-radio>
                            <el-radio :label="false">Nữ</el-radio>
                        </el-radio-group>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item label="Địa chỉ">
                        <el-input v-model="requestDataCreateTeacher.address" :disabled="!isCreateTeacher && !isUpdateTeacher"></el-input>
                        <div v-if="errorsCreateTeacher.Address" class="text-danger">{{ errorsCreateTeacher.Address[0] }}</div>
                    </el-form-item>
                </el-col>
            </el-row>
            <el-row :gutter="10">
                <el-col :span="12">
                    <el-form-item label="Ngày sinh">
                        <el-date-picker v-model="requestDataCreateTeacher.dateOfBirth" type="date" placeholder="Chọn ngày" :disabled="!isCreateTeacher && !isUpdateTeacher"></el-date-picker>
                        <div v-if="errorsCreateTeacher.DateOfBirth" class="text-danger"> {{ errorsCreateTeacher.DateOfBirth[0] }} </div>
                    </el-form-item>
                </el-col>
                <el-col :span="12" v-if="!isCreateTeacher">
                    <el-form-item label="Email">
                        <el-input v-model="requestDataCreateTeacher.email" :disabled="(!isCreateTeacher && !isUpdateTeacher) || (isUpdateTeacher && requestDataCreateTeacher.email === null)">
                        </el-input>
                        <div v-if="errorsCreateTeacher.LastName" class="text-danger">{{ errorsCreateTeacher.LastName[0] }}</div>
                    </el-form-item>
                </el-col>
                <el-col :span="12">
                    <el-form-item v-if="isUpdateTeacher" label="Trạng thái tài khoản">
                        <el-select v-model="requestDataCreateTeacher.accountStatus" placeholder="Trạng thái">
                            <el-option v-for="status in statuses" :key="status.id" :value="status.id" :label="status.name">
                            </el-option>
                        </el-select>
                    </el-form-item>
                </el-col>
            </el-row>
        </el-form>
    </div>
    <template #footer>
        <div class="dialog-footer">
            <el-button @click="handleClose">{{ isCreateTeacher ? 'Hủy' : 'Quay Lại' }}</el-button>
            <el-button v-if="isCreateTeacher" type="primary" @click="handleSave" class="custom-confirm-button">
                Thêm
            </el-button>
            <el-button v-else-if="isUpdateTeacher" type="primary" @click="handleEditTeacher" class="custom-confirm-button">
                Lưu
            </el-button>
        </div>
    </template>
</ModalComponent>
</template>

<script lang="ts">
import {
    defineComponent,
    ref,
    watch
} from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import {
    Upload
} from 'ant-design-vue';

export default defineComponent({
    name: 'CRUDTeacherModal',
    components: {
        ModalComponent,
    },
    props: {
        isShowCreate: {
            type: Boolean,
            required: true,
        },
        isCreateTeacher: {
            type: Boolean,
            required: true,
        },
        isUpdateTeacher: {
            type: Boolean,
            required: true,
        },
        requestDataCreateTeacher: {
            type: Object,
            required: true,
        },
        errorsCreateTeacher: {
            type: Object,
            required: true,
        },
        listSubjects: {
            type: Array,
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
    emits: ['handleSave', 'handleClose', 'handleEditTeacher', 'openEditModal'],
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

        const handleEditTeacher = () => {
            emit('handleEditTeacher');
        };

        return {
            handleSave,
            handleClose,
            openEditModal,
            handleEditTeacher,
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
