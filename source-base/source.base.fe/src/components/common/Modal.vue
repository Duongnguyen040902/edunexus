<template>
  <el-dialog v-model="internalVisible" :title="title" :width="computedWidth" :before-close="handleBeforeClose"
    custom-class="custom-dialog">
    <slot></slot>
    <template #footer>
      <div class="dialog-footer">
        <slot name="footer">
          <el-button @click="internalVisible = false">Cancel</el-button>
          <el-button type="primary" @click="internalVisible = false" class="custom-confirm-button">
            Confirm
          </el-button>
        </slot>
      </div>
    </template>
  </el-dialog>
</template>

<script lang="ts">
import { defineComponent, ref, watch, computed } from 'vue';

export default defineComponent({
  name: 'ModalComponent',
  props: {
    modelValue: {
      type: Boolean,
      required: true,
    },
    title: {
      type: String,
      default: '',
    },
    width: {
      type: String,
      default: '50%',
    },
    beforeClose: {
      type: Function,
      default: (done: () => void) => done(),
    },
  },
  emits: ['update:modelValue'],
  setup(props, { emit }) {
    const internalVisible = ref(props.modelValue);

    watch(
      () => props.modelValue,
      newVal => {
        internalVisible.value = newVal;
      },
    );

    watch(internalVisible, newVal => {
      emit('update:modelValue', newVal);
    });

    const handleBeforeClose = (done: () => void) => {
      props.beforeClose(done);
    };

    const computedWidth = computed(() => {
      if (window.innerWidth < 768) {
        return '90%';
      }
      return props.width;
    });

    return {
      internalVisible,
      handleBeforeClose,
      computedWidth,
    };
  },
});
</script>

<style scoped>
.custom-dialog {
  /* Add any custom styles here */
  font-family: 'Public Sans', -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Oxygen', 'Ubuntu', 'Cantarell',
    'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
}

.dialog-footer {
  text-align: right;
}

.custom-confirm-button {
  background-color: #696cff !important;
  border-color: #696cff !important;
}

@media (max-width: 768px) {
  .custom-dialog {
    width: 90% !important;
  }
}
</style>