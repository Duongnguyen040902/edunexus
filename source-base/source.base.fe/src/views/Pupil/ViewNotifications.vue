<template>
    <div class="container-fluid flex-grow-1 container-p-y">
        <div class="card mb-4">
          <h5 class="card-header">Thông báo</h5>
            <hr class="m-0" />
            <div class="card-body">
                <div v-if="notificationsData.length===0">
                    <p>Không có dữ liệu</p>
                </div>
                <div v-else class="row">
                    <ListNotificationComponent @notificationSelected="handleNotificationSelected"
                        :listNotifications="notificationsData"/>
                    <ViewNotificationDetailComponent :notification="selectedNotification"
                        :notificationCategories="notificationCategories" />
                </div>
            </div>
            
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import ListNotificationComponent from '@/components/views/notification/ListNotification.vue';
import ViewNotificationDetailComponent from '@/components/views/notification/ViewNotificationDetail.vue';
import { ResponseGetNotificationDetailInterface } from '@/types/model/notification';
import { useNotificationComposable } from '@/composables/notification';
import { useClassDetailComposable } from '@/composables/class';

export default defineComponent({
    name: 'ViewNotifications',
    components: {
        ListNotificationComponent,
        ViewNotificationDetailComponent,
    },
    setup() {
        const {goToClassDetail} = useClassDetailComposable(); //TODO đợi sửa thành Pupil
        const selectedNotification = ref<ResponseGetNotificationDetailInterface | null>(null);
        const { handleFetchNotifications,
            notificationDetail, notificationsData, handleFetchNotificationCategories,
            handleFetchNotificationDetail, notificationCategories
            , classId} = useNotificationComposable();

            const handlegoToClassDetail = () => {
                goToClassDetail(classId.value);
            };
        const handleNotificationSelected = (id: number) => {
            handleFetchNotificationDetail(id);
            selectedNotification.value = notificationDetail;
        };

        onMounted(() => {
            handleFetchNotifications();
            handleFetchNotificationCategories();
        });

        return {
            handlegoToClassDetail,
            selectedNotification,
            notificationsData,
            notificationCategories,
            handleNotificationSelected,
        };
    },
});
</script>