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
                            <div class="w-100 flex: 0 0 200px;">
                                <label class="form-label">Tiêu đề:</label>
                                <h6>{{ notification.value?.title }}</h6>
                            </div>
                            <div class="form-group category-container" style="margin-left: 20px;">
                                <label for="category">Thể loại</label>
                                <div>
                                    <p :class="{ 'text-danger': notification.value?.categoryId === 2 }">
                                        <span v-for="category in notificationCategories" :key="category.id">
                                            <span v-if="category.id === notification.value?.categoryId">{{ category.name
                                                }}</span>
                                        </span>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="w-100 mt-3">
                        <label class="form-label">Nội dung:</label>
                        <p class="mb-1">
                            {{ notification.value?.descriptions }}
                        </p>
                    </div>

                    <div class="w-100 mt-3">
                        <label class="form-label">Ảnh:</label>
                        <div class="list-group mt-3 d-flex flex-row flex-wrap">
                            <div v-for="(image, index) in notification.value?.notificationImages" :key="image.id"
                                class="list-group-item list-group-item-action flex-column align-items-start m-2 p-0"
                                style="width: 250px;">
                                <img :src="`${apiUrl}${image.url}`" alt="Notification Image" class="img-fluid"
                                    style="width: 100%; height: auto;" @click="showImage(index)">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div v-else>
            <p>Chọn một thông báo để xem chi tiết.</p>
        </div>
    </div>

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
import { defineComponent, PropType, ref } from 'vue';
import { ResponseGetNotificationDetailInterface } from '@/types/model/notification';
import { useNotificationComposable } from '@/composables/notification';

export default defineComponent({
    name: 'ViewNotificationDetailComponent',
    props: {
        notification: {
            type: Object as PropType<ResponseGetNotificationDetailInterface | null>,
            required: false,
        },
        notificationCategories: {
            type: Array as () => { id: number; name: string }[],
            required: true,
        },
    },
    setup(props) {
        const { apiUrl } = useNotificationComposable();

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

        return {
            apiUrl,
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