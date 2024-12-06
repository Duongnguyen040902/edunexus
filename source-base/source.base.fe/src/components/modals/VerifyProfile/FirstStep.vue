<template>
  <div>
    <div class="formbold-input-flex">
      <div>
        <label class="formbold-form-label" for="firstname">Họ</label>
        <input
          id="firstname"
          v-model="localInformation.firstName"
          class="formbold-form-input"
          name="firstname"
          placeholder="Tên của bạn"
          type="text"
          @input="updateInformation"
        />
      </div>
      <div>
        <label class="formbold-form-label" for="lastname">Tên</label>
        <input
          id="lastname"
          v-model="localInformation.lastName"
          class="formbold-form-input"
          name="lastname"
          placeholder="Dolee"
          type="text"
          @input="updateInformation"
        />
      </div>
    </div>

    <div class="formbold-input-flex">
      <div>
        <label class="formbold-form-label" for="dob">Ngày sinh</label>
        <input
          id="dob"
          v-model="localInformation.dateOfBirth"
          class="formbold-form-input"
          name="dob"
          type="date"
          @input="updateInformation"
        />
      </div>
      <div>
        <label class="formbold-form-label" for="email">Email</label>
        <input
          id="email"
          v-model="localInformation.email"
          class="formbold-form-input"
          name="email"
          placeholder="example@mail.com"
          type="email"
          @input="updateInformation"
        />
      </div>
    </div>

    <div>
      <label class="formbold-form-label" for="address">Địa chỉ</label>
      <input
        id="address"
        v-model="localInformation.address"
        class="formbold-form-input"
        name="address"
        placeholder="Flat 4, 24 Castle Street, Perth, PH1 3JY"
        type="text"
        @input="updateInformation"
      />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, watch } from 'vue';
import { InformationUser } from '@/types/model/auth.ts';

export default defineComponent({
  name: 'FirstStep',
  props: {
    information: {
      type: Object as PropType<InformationUser>,
      default: () => ({}),
    },
  },
  emits: ['update:information'],
  setup(props, { emit }) {
    const localInformation = ref({ ...props.information });
    const updateInformation = () => {
      emit('update:information', localInformation.value);
    };

    watch(
      () => props.information,
      newInfo => {
        localInformation.value = { ...newInfo };
      },
      { deep: true },
    );

    return {
      localInformation,
      updateInformation,
    };
  },
});
</script>

<style scoped></style>
