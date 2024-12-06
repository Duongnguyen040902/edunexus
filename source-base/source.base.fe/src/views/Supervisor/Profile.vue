<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div class="row">
            <div class="col-md-12">
                <div class="card mb-4">
                    <h5 class="card-header">Hồ Sơ Giám Sát Xe Buýt</h5>
                    <!-- Account -->
                    <div class="card-body">
                        <div class="d-flex align-items-start align-items-sm-center gap-4">
                            <img :src="imageUrl" alt="user-avatar" class="d-block rounded" id="uploadedAvatar" />
                            <div v-if="isEditMode" class="button-wrapper">
                                <label for="upload" class="btn btn-primary me-2 mb-4" tabindex="0">
                                    <span class="d-none d-sm-block">Tải ảnh lên</span>
                                    <i class="bx bx-upload d-block d-sm-none"></i>
                                    <input type="file" id="upload" class="account-file-input" hidden
                                        accept="image/png, image/jpeg, image/gif,image/jpg"
                                        @change="handleFileChange" />
                                </label>
                                <p class="text-muted mb-0">CHO PHÉP JPG, GIF, PNG or JPEG.</p>
                                <div v-if="errorsUpdateBusSupervisor.Image" class="text-danger">{{
                                    errorsUpdateBusSupervisor.Image[0] }}</div>
                            </div>
                        </div>
                    </div>
                    <hr class="my-0" />
                    <div class="card-body">
                        <form id="formAccountSettings" method="POST" onsubmit="return false">
                            <div class="row">
                                <div class="mb-3 col-md-6">
                                    <label for="firstName" class="form-label">Tên(*)</label>
                                    <input class="form-control" type="text" id="firstName" name="firstName"
                                        v-model="requestDataUpdateBusSupervisor.firstName" autofocus
                                        :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateBusSupervisor.FirstName" class="text-danger">{{
                                        errorsUpdateBusSupervisor.FirstName[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="lastName" class="form-label">Họ(*)</label>
                                    <input class="form-control" type="text" name="lastName" id="lastName"
                                        v-model="requestDataUpdateBusSupervisor.lastName" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateBusSupervisor.LastName" class="text-danger">{{
                                        errorsUpdateBusSupervisor.LastName[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="gender" class="form-label">Giới tính(*)</label>
                                    <select class="form-control" id="gender"
                                        v-model="requestDataUpdateBusSupervisor.gender" :disabled="!isEditMode">
                                        <option :value="true">Nam</option>
                                        <option :value="false">Nữ</option>
                                    </select>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="phoneNumber" class="form-label">Số điện thoại(*)</label>
                                    <input class="form-control" type="text" id="phoneNumber" name="phoneNumber"
                                        v-model="requestDataUpdateBusSupervisor.phoneNumber" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateBusSupervisor.PhoneNumber" class="text-danger">{{
                                        errorsUpdateBusSupervisor.PhoneNumber[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="address" class="form-label">Địa chỉ(*)</label>
                                    <input type="text" class="form-control" id="address" name="address"
                                        v-model="requestDataUpdateBusSupervisor.address" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateBusSupervisor.Address" class="text-danger">{{
                                        errorsUpdateBusSupervisor.Address[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="email" class="form-label">E-mail</label><br>
                                    <strong class="form-label me-2 fs-6">{{ requestDataUpdateBusSupervisor.email
                                        }}</strong>
                                </div>
                            </div>
                            <div class="mt-2">
                                <button class="btn btn-primary me-2" v-if="!isEditMode" @click="enableEditMode">Chỉnh
                                    sửa</button>
                                <button class="btn btn-primary me-2" v-if="isEditMode" @click="handleUpdateBusSupervisor">Lưu</button>
                                <button class="btn btn-outline-secondary" v-if="isEditMode"
                                    @click="disableEditMode">Hủy</button>
                            </div>
                        </form>
                    </div>
                    <!-- /Account -->
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent, onMounted } from "vue";
import { useBusSupervisorComposable } from "@/composables/bus-supervisor";

export default defineComponent({
    name: "ProfileBusSupervisor",
    setup() {
        const {
            handleFetchBusSupervisor,
            requestDataUpdateBusSupervisor,
            isEditMode,
            disableEditMode,
            enableEditMode,
            errorsUpdateBusSupervisor,
            imageUrl,
            handleUpdateBusSupervisor,
            handleFileChange,
        } = useBusSupervisorComposable();

        onMounted(() => {
            handleFetchBusSupervisor();
        });

        return {
            isEditMode,
            enableEditMode,
            disableEditMode,
            errorsUpdateBusSupervisor,
            requestDataUpdateBusSupervisor,
            handleFileChange,
            handleUpdateBusSupervisor: () => handleUpdateBusSupervisor(requestDataUpdateBusSupervisor.value),
            imageUrl,
        };
    },
});
</script>

<style scoped>
.container-p-y {
    padding-top: 20px;
    padding-bottom: 20px;
}

.card img {
    max-width: 30%;
    height: auto;
}
</style>