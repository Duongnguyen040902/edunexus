<template>
    <ModalComponent :modelValue="isShowModal" :title="title" :width="'50%'" :before-close="handleCloseModal"
        @update:modelValue="$emit('update:isShowModal', $event)">
        <div class="form-group">
            <label for="title">Tiêu đề(*)</label>
            <input v-model="requestDataCreateNotification.title" type="text" class="form-control" id="title"
                placeholder="Nhập tiêu đề" />
            <div v-if="errorsNotification.Title" class="text-danger">{{ errorsNotification.Title[0] }}</div>
        </div>
        <div class="form-group">
            <label for="description">Nội dung(*)</label>
            <textarea v-model="requestDataCreateNotification.descriptions" class="form-control" id="description"
                placeholder="Nhập nội dung"></textarea>
            <div v-if="errorsNotification.Descriptions" class="text-danger">{{ errorsNotification.Descriptions[0] }}
            </div>
        </div>
        <div class="form-group">
            <label for="category">Thể loại</label>
            <select v-model="requestDataCreateNotification.categoryId" class="form-control" id="category">
                <option v-for="category in notificationCategories" :key="category.id" :value="category.id">
                    {{ category.name }}
                </option>
            </select>
            <div v-if="errorsNotification.CategoryId" class="text-danger">{{ errorsNotification.CategoryId[0] }}</div>
        </div>

        <div class="form-group">
            <label for="category">Ảnh</label>
            <el-upload class="upload-demo" action="#" :auto-upload="false" list-type="picture-card"
                :on-change="handleFileChange" :on-remove="handleFileRemove" ref="localUploadRef" multiple>
                <!-- <template #file="{ file }">
                    <div class="el-upload-list__item">
                        <img class="el-upload-list__item-thumbnail" :src="file.url" alt="" />
                        <div class="text-danger mt-2">
                            {{ errorsNotification.FileImage[0] }}
                        </div>
                    </div>
                </template> -->
            </el-upload>
            <!-- <div class="text-danger mt-2">
                {{ errorsNotification.FileImage[0] }}
            </div> -->
            <div v-if="errorsNotification.FileImage">
                <div v-for="(error, index) in errorsNotification.FileImage" :key="index" class="text-danger">
                    {{ error[0] }}
                </div>
            </div>

        </div>

        <template #footer>
            <div class="dialog-footer">
                <el-button @click="handleCloseModal">Hủy</el-button>
                <el-button type="primary" @click="handleSubmit" class="custom-confirm-button">Thêm</el-button>
            </div>
        </template>
    </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, ref, watch, onMounted, PropType } from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import { useNotificationComposable } from '@/composables/notification';
import { Upload } from 'ant-design-vue';
import { ResponseGetNotificationCategoryInterface } from '@/types/model/notification';
export default defineComponent({
    name: 'AddNotificationModal',
    components: {
        ModalComponent,
    },
    props: {
        isShowModal: {
            type: Boolean,
            required: true,
        },
        notificationCategories: {
            type: Array as PropType<ResponseGetNotificationCategoryInterface[]>,
            required: true,
        },
        requestDataCreateNotification: {
            type: Object,
            required: true,
        },
        errorsNotification: {
            type: Object,
            required: true,
        },
        uploadRef: {
            type: Upload,
            required: true,
        }
    },
    emits: ['submit', 'handleFileChange', 'cancel', 'update:isShowModal','handleFileRemove'],
    setup(props, { emit }) {
        const title = "Thêm thông báo";
        const localUploadRef = ref(props.uploadRef);
        watch(() => props.uploadRef, (newVal) => {
            localUploadRef.value = newVal;
        });
        const handleCloseModal = () => {
            emit('cancel', localUploadRef.value);
        };

        const handleSubmit = () => {
            emit('submit', localUploadRef.value);
        };
        
        const handleFileChange = (file: File) => {
            emit('handleFileChange', file);
        };

        const handleFileRemove = (file: File) => {
            emit('handleFileRemove', file);
        }

        return {
            handleCloseModal,
            handleSubmit,
            handleFileChange,
            title,
            handleFileRemove,
            localUploadRef,
        };
    },
});
</script>

<style scoped>
.form-group {
    margin-bottom: 1rem;
}
</style>