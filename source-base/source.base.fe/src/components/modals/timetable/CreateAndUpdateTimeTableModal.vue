<template>
  <ModalComponent
    :modelValue="showModal"
    :title="isUpdateMode ? 'Cập nhật thời khóa biểu' : 'Thêm thời khóa biểu'"
    @update:modelValue="$emit('update:showModal', $event)"
  >
    <el-form>
      <el-form-item label="Chọn môn học">
        <el-select 
          v-model="localSelectedSubjectId" 
          placeholder="Chọn môn học"
          @update:modelValue="$emit('update:selectedSubjectId', $event)"
        >
          <el-option
            v-for="subject in subjects"
            :key="subject.id"
            :label="subject.name"
            :value="subject.id"
          >
          </el-option>
        </el-select>
      </el-form-item>
    </el-form>
    <template #footer>
      <div class="dialog-footer">
        <el-button @click="$emit('update:showModal', false)">HỦY</el-button>
        <el-button
          type="primary"
          @click="handleModalConfirm"
          class="custom-confirm-button"
        >
          {{ isUpdateMode ? "Cập nhật" : "Thêm" }}
        </el-button>
      </div>
    </template>
  </ModalComponent>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from "vue";
import ModalComponent from "@/components/common/Modal.vue";

export default defineComponent({
  name: "CreateAndUpdateTimeTableModalComponent",
  components: {
    ModalComponent,
  },
  props: {
    showModal: {
      type: Boolean,
      required: true,
    },
    isUpdateMode: {
      type: Boolean,
      required: true,
    },
    selectedSubjectId: {
      type: [Number, null],
      required: true,
    },
    subjects: {
      type: Array as PropType<{ id: number; name: string }[]>,
      required: true,
    },
  },
  emits: ['update:showModal', 'update:selectedSubjectId', 'confirm'],
  setup(props, { emit }) {
    const localSelectedSubjectId = ref(props.selectedSubjectId);

    watch(() => props.selectedSubjectId, (newVal) => {
      localSelectedSubjectId.value = newVal;
    });

    const handleModalConfirm = () => {
      emit('confirm');
    };

    return {
      localSelectedSubjectId,
      handleModalConfirm,
    };
  },
});
</script>

<style scoped>
.dialog-footer {
  text-align: right;
}

.custom-confirm-button {
  background-color: #696cff !important;
  border-color: #696cff !important;
}
</style>