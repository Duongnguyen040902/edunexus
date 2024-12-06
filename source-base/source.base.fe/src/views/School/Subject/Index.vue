<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="card">
      <div class="card-datatable table-responsive">
        <div id="DataTables_Table_0_wrapper" class="dataTables_wrapper dt-bootstrap5 no-footer">
          <div class="row">
            <div class="col-sm-12 col-md-3">
              <div id="DataTables_Table_0_length" class="dataTables_length">
                <button
                  aria-controls="DataTables_Table_0"
                  class="btn btn-primary mb-6 mb-md-0"
                  tabindex="0"
                  type="button"
                  @click="handleOpenModalSubject"
                >
                  <span
                    ><i class="bx bx-plus bx-xs me-0 me-sm-2"></i
                    ><span class="d-none d-sm-inline-block">Tạo Môn học</span></span
                  >
                </button>
              </div>
            </div>
          </div>
          <table
            id="DataTables_Table_0"
            aria-describedby="DataTables_Table_0_info"
            class="datatables-permissions table dataTable no-footer dtr-column collapsed"
            style="width: 1210px"
          >
            <thead class="border-top">
              <tr>
                <th aria-label="" class="control sorting_disabled" colspan="1" rowspan="1" style="width: 0px"></th>
                <th
                  aria-controls="DataTables_Table_0"
                  aria-label="Name: activate to sort column ascending"
                  class="sorting"
                  colspan="1"
                  rowspan="1"
                  style="width: 256px"
                  tabindex="0"
                >
                  Tên Môn học
                </th>
                <th
                  aria-controls="DataTables_Table_0"
                  aria-label="Name: activate to sort column ascending"
                  class="sorting"
                  colspan="1"
                  rowspan="1"
                  style="width: 256px"
                  tabindex="0"
                >
                  Tên viết tắt
                </th>
                <th class="cell-fit sorting_disabled" style="width: 118px">Hành động</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(subject, index) in responseSubject.data" class="odd">
                <td>
                  <span class="text-nowrap text-heading">{{ subject.id }}</span>
                </td>
                <td>
                  <span class="text-nowrap text-heading">{{ subject.name }}</span>
                </td>
                <td>
                  <span class="text-nowrap text-heading">{{ subject.code }}</span>
                </td>
                <td>
                  <div class="d-flex align-items-center">
                    <button class="btn btn-icon">
                      <i class="bx bx-edit bx-md" @click="handleSwapValueClickEdit(subject)"></i>
                    </button>
                    <button class="btn btn-icon">
                      <i class="bx bx-trash bx-md" @click="handleOpenModalDelete(subject.id)"></i>
                    </button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  <ModalSubjectComponent
    :isShowModal="isShowModalSubject"
    @closeModal="handleCloseModalSubject"
    @confirmAction="handleConfirmWithStateSubject"
  />
  <ModalConfirmDelete
      :isShowModal="isShowModalDelete"
      :id="idDelete"
      @closeModal="handleCloseModalDelete"
      @confirmAction="handleDeleteSubject"
  />
</template>

<script lang="ts">
import { defineComponent, onMounted } from 'vue';
import ModalSubjectComponent from '@/components/modals/school/ModalSubject.vue';
import ModalConfirmDelete from '@/components/common/ModalConfirmDelete.vue';
import { useSubjectComposable } from '@/composables/subject.ts';

export default defineComponent({
  name: 'SubjectIndex',
  components: { ModalSubjectComponent, ModalConfirmDelete },
  props: {},
  emits: [],
  setup(props, { emit }) {
    const subject = useSubjectComposable();
    const {
      idDelete,
      errorSubject,
      isShowModalDelete,
      isShowModalSubject,
      responseSubject,
      handleGetAllSubject,
      handleOpenModalSubject,
      handleCloseModalSubject,
      handleSwapValueClickEdit,
      handleDeleteSubject,
      handleOpenModalDelete,
      handleCloseModalDelete,
      handleCreateSubject,
      handleConfirmWithStateSubject,
    } = subject;

    onMounted(async () => {
      await handleGetAllSubject();
    });
    return {
      idDelete,
      errorSubject,
      isShowModalDelete,
      isShowModalSubject,
      responseSubject,
      handleGetAllSubject,
      handleOpenModalSubject,
      handleCloseModalSubject,
      handleSwapValueClickEdit,
      handleDeleteSubject,
      handleOpenModalDelete,
      handleCloseModalDelete,
      handleCreateSubject,
      handleConfirmWithStateSubject,
    };
  },
});
</script>

<style scoped></style>
