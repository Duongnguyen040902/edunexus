<template>
    <div class="container-fluid flex-grow-1 container-p-y">
        <ClassSidebarComponent />
        <div class="card mb-4 mt-3">
            <h5 class="card-header d-flex justify-content-between align-items-center">
                Thông báo
                <button @click="handleShowModal " class="btn btn-primary">Thêm thông báo</button>
            </h5>
            <hr class="m-0" />
            <div class="card-body">
                <div v-if="notificationsData.length===0">
                    <p>Không có dữ liệu</p>
                </div>
                <div v-else class="row">
                    <ListNotificationComponent @notificationSelected="handleNotificationSelected"
                        :listNotifications="notificationsData" @updateList="resetList" :notificationCategories="notificationCategories"/>
                    <NotificationDetailComponent :notification="selectedNotification"
                        @resetSelectedNotification="resetSelectedNotification" @saveChanges="handleSaveEditNotification"
                        @resetList="resetList" :errorsNotification="errorsNotification"
                        :notificationCategories="notificationCategories" :isEditing="isEditing" @startEdit="startEdit"
                        @cancelEdit="cancelEdit"/>
                </div>
                
            </div>
            <AddNotificationModal :isShowModal="isShowModal"
                :requestDataCreateNotification="requestDataCreateNotification" @submit="handleAddNotification($event)"
                :notificationCategories="notificationCategories" @update:isShowModal="isShowModal = $event"
                @handleFileChange="handleFileChange" @cancel="handleCloseModal($event)"
                :errorsNotification="errorsNotification" @handleFileRemove="handleFileRemove" :uploadRef="uploadRef"/>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue';
import ListNotificationComponent from '@/components/views/notification/ListNotification.vue';
import NotificationDetailComponent from '@/components/views/notification/NotificationDetail.vue';
import { ResponseGetNotificationDetailInterface } from '@/types/model/notification';
import { useNotificationComposable } from '@/composables/notification';
import AddNotificationModal from '@/components/modals/notification/addNotification.vue';
import PaginateComponent from '@/components/common/Paginate.vue';
import { useClassDetailComposable } from '@/composables/class';
import ClassSidebarComponent from '@/components/views/class/ClassSiderbar.vue';

export default defineComponent({
    name: 'NotificationManagement',
    components: {
        ListNotificationComponent,
        NotificationDetailComponent,
        AddNotificationModal,
        PaginateComponent,
        ClassSidebarComponent,
    },
    setup() {
        const {goToClassDetail} = useClassDetailComposable();
        const selectedNotification = ref<ResponseGetNotificationDetailInterface | null>(null);
        const { isShowModal, handleFetchNotifications, handleCreateNotification, handleUpdateNotification,
            notificationDetail, notificationsData, handleFetchNotificationCategories,
            handleFetchNotificationDetail, notificationCategories, handleFileChange, handleCloseModal,
            requestDataCreateNotification, errorsNotification,updateRequestNotifications,isEditing, handleShowModal,handleFileRemove,
            uploadRef, handleAddNotification, resetList, classId} = useNotificationComposable();

            const handlegoToClassDetail = () => {
                goToClassDetail(classId.value);
            };
            const startEdit = () => {
                isEditing.value = true;
            };
            const idLocal = ref<number | null>(null);
            const cancelEdit = () => {
                handleFetchNotificationDetail(idLocal.value)
                selectedNotification.value = notificationDetail;
                isEditing.value = false;
            };
            
        const handleNotificationSelected = (id: number) => {
            handleFetchNotificationDetail(id);
            idLocal.value = id;
            isEditing.value = false;
            selectedNotification.value = notificationDetail;
        };

        const resetSelectedNotification = () => {
            selectedNotification.value = null;
            handleFetchNotifications();
        };

        const handleSaveEditNotification = async ({ formData }: { formData: FormData }) => {
            await handleUpdateNotification(formData);
        };

        onMounted(() => {
            handleFetchNotifications();
            handleFetchNotificationCategories();
        });

        const showAddNotificationModal = () => {
            isShowModal.value = true;
        };

        return {
            isShowModal,
            errorsNotification,
            updateRequestNotifications,
            handlegoToClassDetail,
            handleShowModal,
            requestDataCreateNotification,
            showAddNotificationModal,
            handleCreateNotification,
            uploadRef,
            handleAddNotification,
            handleFileChange,
            handleCloseModal,
            notificationCategories,
            handleFileRemove,
            selectedNotification,
            notificationsData,
            resetList,
            isEditing,
            handleNotificationSelected,
            resetSelectedNotification,
            handleSaveEditNotification,
            startEdit,
            cancelEdit,
        };
    },
});
</script>