<template>
    <div class="container-xxl flex-grow-1 container-p-y">
        <div class="row">
            <div class="col-md-12">
                <div class="card mb-4">
                    <h5 class="card-header">Hồ Sơ Giáo Viên</h5>
                    <!-- Account -->
                    <div class="card-body">
                        <div class="d-flex align-items-start align-items-sm-center gap-4" style="width: 100%;">
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
                                <div v-if="errorsUpdateTeacher.Image" class="text-danger">{{
                                    errorsUpdateTeacher.Image[0] }}</div>
                            </div>
                        </div>
                    </div>
                    <hr class="my-0" />
                    <div class="card-body">
                        <div class="mb-3 col-md-6 d-flex align-items-center">
                            <strong class="form-label me-2 fs-5">Danh sách chuyên môn:</strong>
                            <p id="listSubject" name="listSubject" class="mb-0 d-inline-flex flex-wrap">
                                <span v-for="(subject, index) in requestDataUpdateTeacher.listSubject" :key="index"
                                    class="me-1">
                                    <strong>{{ subject.name }}</strong>
                                    <span v-if="index < requestDataUpdateTeacher.listSubject.length - 1">,</span>
                                </span>
                            </p>
                        </div>
                        <form id="formAccountSettings" method="POST" onsubmit="return false">
                            <div class="row">
                                <div class="mb-3 col-md-6">
                                    <label for="firstName" class="form-label">Tên(*)</label>
                                    <input class="form-control" type="text" id="firstName" name="firstName"
                                        v-model="requestDataUpdateTeacher.firstName" autofocus
                                        :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateTeacher.FirstName" class="text-danger">{{
                                        errorsUpdateTeacher.FirstName[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="lastName" class="form-label">Họ(*)</label>
                                    <input class="form-control" type="text" name="lastName" id="lastName"
                                        v-model="requestDataUpdateTeacher.lastName" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateTeacher.LastName" class="text-danger">{{
                                        errorsUpdateTeacher.LastName[0] }}</div>
                                </div>

                                <div class="mb-3 col-md-6">
                                    <label for="gender" class="form-label">Giới tính(*)</label>
                                    <select class="form-control" id="gender" v-model="requestDataUpdateTeacher.gender"
                                        :disabled="!isEditMode">
                                        <option :value="true">Nam</option>
                                        <option :value="false">Nữ</option>
                                    </select>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="dateOfBirth" class="form-label">Ngày sinh(*)</label>
                                    <input class="form-control" type="date" id="dateOfBirth" name="dateOfBirth"
                                        v-model="requestDataUpdateTeacher.dateOfBirth" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateTeacher.DateOfBirth" class="text-danger">{{
                                        errorsUpdateTeacher.DateOfBirth[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="phoneNumber" class="form-label">Số điện thoại(*)</label>
                                    <div class="input-group input-group-merge">
                                        <input type="text" id="phoneNumber" name="phoneNumber" class="form-control"
                                            v-model="requestDataUpdateTeacher.phoneNumber" :disabled="!isEditMode" />
                                    </div>
                                    <div v-if="errorsUpdateTeacher.PhoneNumber" class="text-danger">{{
                                        errorsUpdateTeacher.PhoneNumber[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="address" class="form-label">Địa chỉ(*)</label>
                                    <input type="text" class="form-control" id="address" name="address"
                                        v-model="requestDataUpdateTeacher.address" :disabled="!isEditMode" />
                                    <div v-if="errorsUpdateTeacher.Address" class="text-danger">{{
                                        errorsUpdateTeacher.Address[0] }}</div>
                                </div>
                                <div class="mb-3 col-md-6">
                                    <label for="email" class="form-label">E-mail</label><br>
                                    <strong class="form-label me-2 fs-6">{{ requestDataUpdateTeacher.email }}</strong>
                                </div>
                                <!-- <div class="mb-3 col-md-6">
  <label for="email" class="form-label">E-mail</label>
  <input class="form-control" type="text" id="email" name="email" v-model="requestDataUpdateTeacher.email" placeholder="john.doe@example.com" :readonly="true" />
  <div v-if="errorsUpdateTeacher.Email" class="text-danger">{{ errorsUpdateTeacher.Email[0] }}</div>
</div> -->

                            </div>
                            <div class="mt-2">
                                <button class="btn btn-primary me-2" v-if="!isEditMode" @click="enableEditMode">Chỉnh
                                    sửa</button>
                                <button class="btn btn-primary me-2" v-if="isEditMode"
                                    @click="handleUpdateTeacher">Lưu</button>
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
import { useTeacherComposable } from "@/composables/teacher";

export default defineComponent({
    name: "ProfileTeacher",
    setup() {
        const {
            handleFetchTeacher,
            requestDataUpdateTeacher,
            isEditMode,
            disableEditMode,
            enableEditMode,
            errorsUpdateTeacher,
            imageUrl,
            handleFileChange,
            handleUpdateTeacher,
        } = useTeacherComposable();

        onMounted(() => {
            handleFetchTeacher();
        });

        return {
            isEditMode,
            handleUpdateTeacher: () => handleUpdateTeacher(requestDataUpdateTeacher.value),
            enableEditMode,
            disableEditMode,
            errorsUpdateTeacher,
            requestDataUpdateTeacher,
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