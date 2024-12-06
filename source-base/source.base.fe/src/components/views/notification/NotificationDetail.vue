<template>
    <div class="col-lg-8">
        <small class="text-light fw-semibold">Chi Tiết Thông Báo</small>
        <div class="demo-inline-spacing mt-3" v-if="notification">
            <div class="list-group">
                <div class="list-group-item list-group-item-action flex-column align-items-start">
                    <div class="d-flex justify-content-between w-100 align-items-center">
                        <small class="text-muted">{{ new Date(notification.value?.createdDate).toLocaleDateString()
                            }}</small>
                    </div>
                    <div class="d-flex justify-content-between w-100 align-items-center mt-2">
                        <div class="d-flex align-items-center w-100">
                            <div class="w-100">
                                <label class="form-label">Tiêu đề:</label>
                                <h6 v-if="!isEditing">{{ notification.value?.title }}</h6>
                                <input v-else v-model="notification.value.title" class="form-control" />
                                <div v-if="errorsNotification.Title" class="text-danger">{{ errorsNotification.Title[0]
                                    }}</div>
                            </div>
                            <div class="form-group category-container" style="margin-left: 20px;">
                                <label for="category">Thể loại</label>
                                <div v-if="!isEditing" >
                                    <p v-for="category in notificationCategories" :key="category.id" :class="{ 'text-danger': category.name === 'Khẩn cấp' }">
                                        <span v-if="category.id === notification.value?.categoryId" >{{ category.name}}</span>
                                    </p>
                                </div>
                                <select v-else v-model="notification.value.categoryId" class="form-control"
                                    id="category">
                                    <option v-for="category in notificationCategories" :key="category.id"
                                        :value="category.id">
                                        {{ category.name }}
                                    </option>
                                </select>
                                <div v-if="errorsNotification.CategoryId" class="text-danger">{{
                                    errorsNotification.CategoryId[0] }}</div>
                            </div>
                        </div>
                    </div>
                    <div class="w-100 mt-3">
                        <label class="form-label">Nội dung:</label>
                        <p class="mb-1" v-if="!isEditing">
                            {{ notification.value?.descriptions }}
                        </p>
                        <textarea v-else v-model="notification.value.descriptions" class="form-control"
                            rows="4"></textarea>
                        <div v-if="errorsNotification.Descriptions" class="text-danger">{{
                            errorsNotification.Descriptions[0] }}</div>
                    </div>

                    <!-- Thêm danh sách hình ảnh -->
                    <div class="w-100 mt-3">
                        <label class="form-label">Ảnh:</label>
                        <div v-if="!isEditing" class="list-group mt-3 d-flex flex-row flex-wrap">
                            <div v-for="(image, index) in notification.value?.notificationImages" :key="image.id"
                                class="list-group-item list-group-item-action flex-column align-items-start m-2 p-0"
                                style="width: 250px;">
                                <img :src="`${apiUrl}${image.url}`" alt="Notification Image" class="img-fluid"
                                    style="width: 100%; height: auto;" @click="showImage(index)">
                            </div>
                        </div>

                        <div v-else class="list-group mt-3 d-flex flex-row flex-wrap">
                            <div class="form-group" style="display: flex;">
                                <div class="list-group-item list-group-item-action flex-column align-items-start m-2 p-0"
                                    style="width:100%;">
                                    <!-- Sử dụng el-upload để hiển thị ảnh cũ và thêm/xóa ảnh -->
                                    <el-upload class="upload-demo" action="#" :auto-upload="false"
                                        list-type="picture-card" :on-change="handleFileChange"
                                        :on-remove="handleFileRemove" :file-list="fileList" multiple>
                                        <el-button slot="trigger" size="small" type="primary">Chọn ảnh</el-button>
                                    </el-upload>
                                    <div v-if="errorsNotification.FileImage">
                                        <div v-for="(error, index) in errorsNotification.FileImage" :key="index"
                                            class="text-danger">
                                            {{ error[0] }}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="mt-3 d-flex justify-content-end">
                        <button v-if="!isEditing" @click="startEditing" class="btn btn-primary">Chỉnh sửa</button>
                        <div v-else>
                            <button @click="saveChanges" class="btn btn-primary">Lưu</button>
                            <button @click="cancelEditing" class="btn btn-danger"
                                style="margin-left: 10px;">Hủy</button>
                        </div>
                        <button v-if="!isEditing" @click="showDeleteModal" class="btn btn-danger"
                            style="margin-left: 10px;">Xóa</button>
                    </div>
                </div>
            </div>
        </div>
        <div v-else>
            <p>Chọn một thông báo để xem chi tiết.</p>
        </div>
    </div>
    <ModalConfirmDeleteComponent :isShowModal="isShowDeleteModal" :id="notification?.value?.id ?? ''"
        @closeModal="closeDeleteModal" @confirmAction="confirmDelete" />

    <!-- Modal hiển thị ảnh toàn màn hình -->
    <div v-if="isImageModalVisible" class="image-modal" @click.self="closeImageModal">
        <button class="prev-button" @click="prevImage">
            &#9664;
        </button>
        <img :src="currentImageUrl" class="full-screen-image" />
        <button class="next-button" @click="nextImage">
            &#9654;
        </button>
        <button class="close-button" @click="closeImageModal">
            &#10006;
        </button>
    </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import { ResponseGetNotificationDetailInterface,ResponseGetNotificationCategoryInterface } from '@/types/model/notification';
import ModalConfirmDeleteComponent from '@/components/common/ModalConfirmDelete.vue';
import { useNotificationComposable } from '@/composables/notification';

export default defineComponent({
    name: 'NotificationDetailComponent',
    components: {
        ModalConfirmDeleteComponent,
    },
    props: {
        notification: {
            type: Object as PropType<ResponseGetNotificationDetailInterface | null>,
            required: false,
        },
        errorsNotification: {
            type: Object,
            required: true,
        },
        notificationCategories: {
            type: Array as PropType<ResponseGetNotificationCategoryInterface[]>,
        },
        isEditing: {
            type: Boolean,
            required: true,
        },
    },
    emits: ['resetSelectedNotification', 'updateNotification', 'saveChanges', 'resetList', 'startEdit', 'cancelEdit'],
    setup(props, { emit }) {
        const { isShowDeleteModal, showDeleteModal, closeDeleteModal, handleDeleteNotification, apiUrl } = useNotificationComposable();

        const fileList = ref<(File | { url: string, isOld?: boolean })[]>([]); // Cho phép cả file mới và URL ảnh cũ (chuyển đổi từ Blob)
        const oldFiles = ref<{ url: string, isOld?: boolean }[]>([]); // Danh sách ảnh cũ

        const isImageModalVisible = ref(false);
        const currentImageUrl = ref('');
        const currentIndex = ref(0);

        const showImage = (index: number) => {
            currentIndex.value = index;
            currentImageUrl.value = `${apiUrl}${props.notification?.value.notificationImages[index].url}`;
            isImageModalVisible.value = true;
        };

        const closeImageModal = () => {
            isImageModalVisible.value = false;
        };

        const nextImage = () => {
            if (props.notification?.value.notificationImages) {
                currentIndex.value = (currentIndex.value + 1) % props.notification.value.notificationImages.length;
                currentImageUrl.value = `${apiUrl}${props.notification.value.notificationImages[currentIndex.value].url}`;
            }
        };

        const prevImage = () => {
            if (props.notification?.value.notificationImages) {
                currentIndex.value = (currentIndex.value - 1 + props.notification.value.notificationImages.length) % props.notification.value.notificationImages.length;
                currentImageUrl.value = `${apiUrl}${props.notification.value.notificationImages[currentIndex.value].url}`;
            }
        };

        const cancelEditing = () => {
            emit('cancelEdit');
        };
        const startEditing = () => {
            emit('startEdit');
            if (props.notification?.value) {
                // Đảm bảo mỗi ảnh trong `notificationImages` có `url` để hiển thị
                oldFiles.value = props.notification.value.notificationImages.map(image => ({
                    name: image.url.split('/').pop(),
                    url: `${apiUrl}${image.url}`, // URL cũ cho hình ảnh
                    isOld: true // Đánh dấu ảnh cũ
                }));
                fileList.value = [...oldFiles.value];
            }
        };

        // Xử lý ảnh mới được thêm vào
        const handleFileChange = (file: { raw: File }) => {
            fileList.value.push(file.raw);
        };

        // Xóa ảnh
        const handleFileRemove = (file: any) => {
            const index = fileList.value.findIndex(f => f.name === file.name);
            if (index !== -1) {
                fileList.value.splice(index, 1);
            }
            // Nếu là ảnh cũ, xóa khỏi danh sách ảnh cũ
            const oldFileIndex = oldFiles.value.findIndex(f => f.url === file.url);
            if (oldFileIndex !== -1) {
                oldFiles.value.splice(oldFileIndex, 1);
            }
        };

        const fetchImageAsBlob = async (url: string) => {
            const response = await fetch(url);
            if (!response.ok) throw new Error('Network response was not ok');
            return await response.blob();
        };

        const saveChanges = async () => {
            const formData = new FormData();
            formData.append('id', props.notification?.value.id.toString());
            formData.append('classId', props.notification?.value.classId.toString());
            formData.append('title', props.notification?.value.title);
            formData.append('descriptions', props.notification?.value.descriptions);
            formData.append('categoryId', props.notification?.value.categoryId.toString());

            // Thêm ảnh cũ vào formData
            const existingImagesPromises = oldFiles.value.map(async (file) => {
                try {
                    const blob = await fetchImageAsBlob(file.url);
                    const mimeType = blob.type || "image/png";
                    const oldFile = new File([blob], file.url.split('/').pop(), { type: mimeType });
                    formData.append('fileImage', oldFile); // Thêm file binary vào formData
                } catch (error) {
                    console.error(`Không thể tải hình ảnh từ ${file.url}:`, error);
                }
            });

            // Chờ tất cả các promise hoàn thành
            await Promise.all(existingImagesPromises);

            // Thêm ảnh mới vào formData
            // const newImagePromises = fileList.value.map((file) => {
            //     formData.append('fileImage', file);
            // });
            const newImagePromises = fileList.value.map((file) => {
                if (file instanceof File) {
                    formData.append('fileImage', file);
                } 
            });

            // Chờ tất cả các ảnh mới được thêm vào formData (trong trường hợp có xử lý gì ở đây)
            await Promise.all(newImagePromises);

            try {
                emit('saveChanges', { formData });
                console.log("Cập nhật thành công:", fileList.value);
            } catch (error) {
                console.error("Có lỗi khi cập nhật:", error);
            }
        };

        watch(() => props.notification, (newValue) => {
            if (newValue) {
                fileList.value = [];
                oldFiles.value = [];
            }
        });

        const confirmDelete = async (id: number) => {
            await handleDeleteNotification(id);
            emit('resetSelectedNotification');
        };

        return {
            handleFileChange,
            handleFileRemove,
            isShowDeleteModal,
            apiUrl,
            handleDeleteNotification,
            startEditing,
            saveChanges,
            cancelEditing,
            showDeleteModal,
            closeDeleteModal,
            confirmDelete,
            fileList,
            oldFiles,
            isImageModalVisible,
            currentImageUrl,
            showImage,
            closeImageModal,
            nextImage,
            prevImage,
        };
    },
});
</script>

<style scoped>
.image-modal {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: rgba(0, 0, 0, 0.8);
    display: flex;
    justify-content: center;
    align-items: center;
    z-index: 1000;
}

.full-screen-image {
    max-width: 90%;
    max-height: 90%;
}

.prev-button,
.next-button,
.close-button {
    position: absolute;
    top: 50%;
    transform: translateY(-50%);
    background-color: rgba(255, 255, 255, 0.7);
    border: none;
    padding: 20px;
    cursor: pointer;
    z-index: 1001;
    font-size: 18px;
    border-radius: 50%;
    box-shadow: 0 2px 10px rgba(0, 0, 0, 0.5);
    transition: background-color 0.3s;
}

.prev-button:hover,
.next-button:hover,
.close-button:hover {
    background-color: rgba(255, 255, 255, 1);
}

.prev-button {
    left: 20px;
}

.next-button {
    right: 20px;
}

.close-button {
    top: 20px;
    right: 20px;
    transform: none;
    font-size: 24px;
    padding: 10px 15px;
}

.category-container {
    flex: 0 0 100px;
    /* Điều chỉnh kích thước của ô thể loại */
}
</style>