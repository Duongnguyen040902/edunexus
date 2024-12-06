<template>
    <div class="col-lg-4 mb-4 mb-xl-0">
        <small class="text-light fw-semibold">Danh Sách Thông Báo</small>
        <div class="demo-inline-spacing mt-3">
            <div class="list-container">
                <ul class="list-group">
                    <li v-for="(notification) in listNotifications" :key="notification.id" :class="[
                        'list-group-item',
                        selectedNotification && selectedNotification.id === notification.id ? 'active' : ''
                    ]" @click="selectNotification(notification)" style="cursor: pointer;">
                        <div class="d-flex justify-content-between w-100">
                            <div class="d-flex flex-column" style="width: 70%;">
                                <span>{{ notification.title }}</span>
                                <small class="text-muted">{{ truncatedDescription(notification.descriptions) }}</small>
                            </div>
                            <div class="d-flex flex-column text-end">
                                <small>{{ new Date(notification.createdDate).toLocaleDateString() }}</small>
                                <small v-for="category in notificationCategories" :key="category.id"
                                    :class="category.name === 'Khẩn cấp' ? 'bg-label-warning' : 'bg-label-success'"
                                    class="badge" style="width: 80px; font-size: x-small;">
                                    <span v-if="category.id === notification.categoryId">{{ category.name }}</span>
                                </small>
                            </div>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, ref, PropType } from 'vue';
import { useNotificationComposable } from '@/composables/notification';
import { ResponseGetListNotificationsInterface, ResponseGetNotificationDetailInterface, ResponseGetNotificationCategoryInterface } from '@/types/model/notification';
import AddNotificationModal from '@/components/modals/notification/addNotification.vue';

export default defineComponent({
    name: 'ListNotificationComponent',
    components: {
        AddNotificationModal,
    },
    props: {
        listNotifications: {
            type: Array as PropType<ResponseGetListNotificationsInterface[]>,
        },
        notificationCategories: {
            type: Array as PropType<ResponseGetNotificationCategoryInterface[]>,
        },
    },
    emits: ['notificationSelected', 'notificationDeleted'],
    setup(_, { emit }) {
        const {
            handleFetchNotifications,
            handleCloseModal,
            requestDataCreateNotification,
            handleFileChange,
            notificationDetail,
        } = useNotificationComposable();

        const selectedNotification = ref<ResponseGetNotificationDetailInterface | null>(null);

        const selectNotification = (notification: ResponseGetNotificationDetailInterface) => {
            selectedNotification.value = notification; // Lưu thông báo đã chọn
            emit('notificationSelected', notification.id);
        };

        const handleNotificationDeleted = () => {
            handleFetchNotifications();
        };

        const truncatedDescription = (description: string) => {
            if (description.length > 25) {
                return description.substring(0, 25) + '...';
            }
            return description;
        };

        return {
            truncatedDescription,
            handleCloseModal,
            selectedNotification,
            selectNotification,
            handleNotificationDeleted,
            requestDataCreateNotification,
            handleFileChange,
            notificationDetail,
        };
    },
});
</script>

<style scoped>
.list-container {
    max-height: 600px;
    /* Adjust the height as needed */
    overflow-y: auto;
}

.list-group-item-danger {
    border-color: aliceblue;
}

@media (max-width: 800px) {
    .list-container {
        max-height: calc(3 * 80px);
        /* Adjust the height to show approximately 3 notifications */
    }
}
</style>