<template>
    <div>
        <el-pagination :current-page="currentPage" :page-size="pageSize" :total="total" layout="prev, pager, next"
            @current-change="handlePageChange" size="small" background class="pagination" />
    </div>
</template>

<script lang="ts">
import { defineComponent, toRefs, SetupContext } from 'vue';

export default defineComponent({
    name: 'PaginateComponent',
    props: {
        total: {
            type: Number,
            required: true,
        },
        pageSize: {
            type: Number,
            default: 10,
        },
        currentPage: {
            type: Number,
            default: 1,
        },
    },
    emits: ['update:currentPage'],
    setup(props, { emit }: SetupContext) {
        const { total, pageSize, currentPage } = toRefs(props);

        const handlePageChange = (page: number) => {
            emit('update:currentPage', page);
        };

        return {
            total,
            pageSize,
            currentPage,
            handlePageChange,
        };
    },
});
</script>

<style scoped>
.pagination {
    margin-top: 1rem;
}

@media (max-width: 768px) {
    .pagination {
        font-size: 12px;
    }
}

@media (max-width: 480px) {
    .pagination {
        font-size: 10px;
    }
}
</style>