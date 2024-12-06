<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div class="row">
            <div class="col-md-12">
                <div class="card mb-4">
                    <h5 class="card-header">Hồ Sơ Học Sinh</h5>
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
                                <div v-if="errorsUpdatePupil.Image" class="text-danger">{{
                                    errorsUpdatePupil.Image[0] }}</div>
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
                                        v-model="requestDataUpdatePupil.firstName" autofocus :disabled="!isEditMode" />
                                    <div v-if="errorsUpdatePupil.FirstName" class="text-danger">{{
                                        errorsUpdatePupil.FirstName[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="lastName" class="form-label">Họ(*)</label>
                                    <input class="form-control" type="text" name="lastName" id="lastName"
                                        v-model="requestDataUpdatePupil.lastName" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdatePupil.LastName" class="text-danger">{{
                                        errorsUpdatePupil.LastName[0] }}</div>
                                </div>

                                <div class="mb-3 col-md-6">
                                    <label for="gender" class="form-label">Giới tính(*)</label>
                                    <select class="form-control" id="gender" v-model="requestDataUpdatePupil.gender"
                                        :disabled="!isEditMode">
                                        <option :value="true">Nam</option>
                                        <option :value="false">Nữ</option>
                                    </select>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="dateOfBirth" class="form-label">Ngày sinh(*)</label>
                                    <input class="form-control" type="date" id="dateOfBirth" name="dateOfBirth"
                                        v-model="requestDataUpdatePupil.dateOfBirth" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdatePupil.DateOfBirth" class="text-danger">{{
                                        errorsUpdatePupil.DateOfBirth[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="donorName" class="form-label">Tên người bảo hộ(*)</label>
                                    <input class="form-control" type="text" id="donorName" name="donorName"
                                        v-model="requestDataUpdatePupil.donorName" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdatePupil.DonorName" class="text-danger">{{
                                        errorsUpdatePupil.DonorName[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="donorPhoneNumber" class="form-label">Số điện thoại người bảo hộ(*)</label>
                                    <div class="input-group input-group-merge">
                                        <input type="text" id="donorPhoneNumber" name="donorPhoneNumber"
                                            class="form-control" v-model="requestDataUpdatePupil.donorPhoneNumber"
                                            :disabled="!isEditMode" />
                                    </div>
                                    <div v-if="errorsUpdatePupil.DonorPhoneNumber" class="text-danger">{{
                                        errorsUpdatePupil.DonorPhoneNumber[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="address" class="form-label">Địa chỉ(*)</label>
                                    <input type="text" class="form-control" id="address" name="address"
                                        v-model="requestDataUpdatePupil.address" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdatePupil.Address" class="text-danger">{{
                                        errorsUpdatePupil.Address[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="email" class="form-label">E-mail</label><br>
                                    <strong class="form-label me-2 fs-6">{{ requestDataUpdatePupil.email }}</strong>
                                </div>
                            </div>
                            <div class="mt-2">
                                <button class="btn btn-primary me-2" v-if="!isEditMode" @click="enableEditMode">Chỉnh
                                    sửa</button>
                                <button class="btn btn-primary me-2" v-if="isEditMode"
                                    @click="handleUpdatePupil">Lưu</button>
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
import { usePupilComposable } from "@/composables/pupil";

export default defineComponent({
    name: "ProfilePupil",
    setup() {
        const {
            handleFetchPupil,
            requestDataUpdatePupil,
            isEditMode,
            disableEditMode,
            enableEditMode,
            errorsUpdatePupil,
            imageUrl,
            handleFileChange,
            handleUpdatePupil,
        } = usePupilComposable();

        onMounted(() => {
            handleFetchPupil();

        });

        return {
            handleUpdatePupil: () => handleUpdatePupil(requestDataUpdatePupil.value),
            isEditMode,
            enableEditMode,
            disableEditMode,
            errorsUpdatePupil,
            requestDataUpdatePupil,
            handleFileChange,
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
    /* Thay đổi từ 100px sang kích thước lớn hơn */
    height: auto;
}
</style>