<template>
  <div class="dataTables_paginate paging_simple_numbers">
    <ul class="pagination">
      <li :class="{ disabled: currentPage === 1 }" class="paginate_button page-item previous">
        <router-link
          :aria-disabled="currentPage === 1"
          :to="{ query: { page: currentPage - 1 } }"
          aria-controls="DataTables_Table_0"
          class="page-link"
          role="link"
          tabindex="-1"
          @click.native.prevent="prevPage"
        >
          <i class="bx bx-chevron-left bx-18px"></i>
        </router-link>
      </li>

      <li
        v-for="page in totalPages"
        :key="page"
        :class="{ active: currentPage === page }"
        class="paginate_button page-item"
      >
        <router-link
          :to="{ query: { page } }"
          aria-controls="DataTables_Table_0"
          class="page-link"
          role="link"
          @click.native.prevent="goToPage(page)"
        >
          {{ page }}
        </router-link>
      </li>

      <li :class="{ disabled: currentPage === totalPages }" class="paginate_button page-item next">
        <router-link
          :aria-disabled="currentPage === totalPages"
          :to="{ query: { page: currentPage + 1 } }"
          aria-controls="DataTables_Table_0"
          class="page-link"
          role="link"
          @click.native.prevent="nextPage"
        >
          <i class="bx bx-chevron-right bx-18px"></i>
        </router-link>
      </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { defineComponent, computed } from 'vue';

export default defineComponent({
  name: 'PaginationComponent',
  props: {
    totalRecords: {
      type: Number,
      required: true,
    },
    pageSize: {
      type: Number,
      required: true,
    },
    currentPage: {
      type: Number,
      default: 1,
    },
  },
  setup(props, { emit }) {
    const totalPages = computed(() => {
      const pages = Math.ceil(props.totalRecords / props.pageSize);
      return pages > 0 ? pages : 1;
    });

    const goToPage = (page: number) => {
      if (page < 1 || page > totalPages.value) return;
      emit('update:currentPage', page);
      emit('page-changed', page);
    };

    const prevPage = () => {
      if (props.currentPage > 1) {
        const newPage = props.currentPage - 1;
        emit('update:currentPage', newPage);
        emit('page-changed', newPage);
      }
    };

    const nextPage = () => {
      if (props.currentPage < totalPages.value) {
        const newPage = props.currentPage + 1;
        emit('update:currentPage', newPage);
        emit('page-changed', newPage);
      }
    };

    return {
      totalPages,
      goToPage,
      prevPage,
      nextPage,
    };
  },
});
</script>
