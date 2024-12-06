<template>
<ModalComponent :model-value="isImportModalVisible" :beforeClose="closeImportModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="text-center mb-6">
                <h4 class="mb-2" >Chọn tệp Excel</h4>
            </div>
            <div class="modal-body">
                <el-upload class="upload-demo" action="#" :auto-upload="false" :on-change="onFileChange" :limit="1" list-type="text">
                    <el-button type="primary" class="custom-upload-button">Chọn tệp</el-button>
                </el-upload>
                <p class="text-muted mb-0">CHO PHÉP XLSX, XLS.</p>
            </div>
        </div>
    </div>
    <template #footer>
        <div class="dialog-footer">
            <el-button class="btn btn-secondary" @click="closeImportModal">Quay lại</el-button>
            <el-button type="primary" @click="handleImportExcelTeacherAccount" class="btn btn-primary">
                Nhập excel
            </el-button>
        </div>
    </template>
</ModalComponent>
</template>

<script lang="ts">
import {
    defineComponent,
} from 'vue';
import ModalComponent from '@/components/common/Modal.vue';

export default defineComponent({
    name: 'ImportExcelModal',
    components: {
        ModalComponent,
    },
    props: {
        isImportModalVisible: {
            type: Boolean,
            required: true,
        },
    },
    emits: ['handleImportExcelTeacherAccount', 'closeImportModal', 'onFileChange'],
    setup(props, {
        emit
    }) {

        const handleImportExcelTeacherAccount = () => {
            emit('handleImportExcelTeacherAccount');
        };

        const closeImportModal = () => {
            emit('closeImportModal');
        };

        const onFileChange = file => {
            emit('onFileChange', file);
        };

        return {
            handleImportExcelTeacherAccount,
            closeImportModal,
            onFileChange,
        };
    },
});
</script>

<style scoped>
.modal-header {
    text-align: center;
    font-size: 14px;
    font-weight: bold;
    padding: 15px;
    border-bottom: 1px solid #f0f0f0;
}

.modal-body {
    padding: 20px;
    text-align: center;
}

.upload-demo {
    display: flex;
    justify-content: center; 
    align-items: center;     
    margin: 10px 0;
    flex-direction: column;  
}

.file-info {
    margin-top: 15px; 
    text-align: center; 
    font-size: 14px;
    color: #333;
}

.custom-upload-button {
    background-color: #696cff;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 10px 20px;
    font-size: 15px;
    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
    transition: all 0.3s ease-in-out;
    cursor: pointer;
}

.custom-upload-button:hover {
    background-color: #5848cc;
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.2);
}

.dialog-footer {
    display: flex;
    justify-content: space-between;
    padding: 15px 20px;
    border-top: 1px solid #f0f0f0;
}

.dialog-footer .el-button {
    padding: 10px 15px;
    font-size: 14px;
    border-radius: 6px;
}

.custom-confirm-button {
    background-color: #007bff;
    color: white;
    border: none;
    font-weight: bold;
}

.custom-confirm-button:hover {
    background-color: #0056b3;
}
</style>
