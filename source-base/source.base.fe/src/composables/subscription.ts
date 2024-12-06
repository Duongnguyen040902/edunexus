import { useSubscriptionStore } from '@/stores/subscription.ts';
import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError, notifySuccess } from '@/helpers/notify.ts';
import { ref } from 'vue';
import { Subscription } from '@/types/model/subscription.ts';

export const useSubscriptionComposable = () => {
  const subscriptionStore = useSubscriptionStore();
  const {
    errorSubscription,
    requestCreateSubscription,
    responseSubscription,
    getAllSubscription,
    createSubscription,
    updateSubscription,
  } = subscriptionStore;
  const isShowModalSubscription = ref(false);
  const isDetailSubscription = ref(false);
  const handleGetAllSubscription = async () => {
    await getAllSubscription();
  };

  const handleCreateSubscription = async () => {
    startLoading();
    await createSubscription(
      () => {
        notifySuccess('Tạo gói dịch vụ thành công');
        endLoading();
        handleCloseModalSubscription();
        handleGetAllSubscription();
      },
      () => {
        notifyError('Tạo gói dịch vụ thất bại');
        endLoading();
      },
    );
  };

  const resetRequestCreateSubscription = () => {
    requestCreateSubscription.id = 0;
    requestCreateSubscription.price = 0;
    requestCreateSubscription.durationDays = 0;
    requestCreateSubscription.name = '';
    requestCreateSubscription.description = '';
    requestCreateSubscription.maxActiveAccounts = 0;
    isDetailSubscription.value = false;
  };

  const handleUpdateSubscription = async (id: number) => {
    startLoading();
    await updateSubscription(
      id,
      () => {
        notifySuccess('Cập nhật gói dịch vụ thành công');
        endLoading();
        resetRequestCreateSubscription();
        handleCloseModalSubscription();
        handleGetAllSubscription();
      },
      () => {
        notifyError('Cập nhật gói dịch vụ thất bại');
        endLoading();
      },
    );
  };

  const handleFetchDetailSubscription = async (subscription: Subscription) => {
    requestCreateSubscription.id = subscription.id;
    requestCreateSubscription.price = subscription.price;
    requestCreateSubscription.durationDays = subscription.durationDays;
    requestCreateSubscription.name = subscription.name;
    requestCreateSubscription.description = subscription.description;
    requestCreateSubscription.maxActiveAccounts = subscription.maxActiveAccounts;
  };

  const handleOpenModalSubscription = () => {
    isShowModalSubscription.value = true;
  };

  const handleCloseModalSubscription = () => {
    isShowModalSubscription.value = false;
    isDetailSubscription.value = false;
    resetRequestCreateSubscription();
  };

  const handleClickDetailSubscription = async (subscription: Subscription) => {
    isDetailSubscription.value = true;
    isShowModalSubscription.value = true;
    await handleFetchDetailSubscription(subscription);
  };

  const handleConfirmWithAction = async () => {
    if (isDetailSubscription.value) {
      if (requestCreateSubscription.id) {
        await handleUpdateSubscription(requestCreateSubscription.id);
      }
    } else {
      await handleCreateSubscription();
    }
  };

  return {
    isDetailSubscription,
    isShowModalSubscription,
    requestCreateSubscription,
    errorSubscription,
    responseSubscription,
    handleGetAllSubscription,
    handleCreateSubscription,
    handleUpdateSubscription,
    handleOpenModalSubscription,
    handleCloseModalSubscription,
    handleFetchDetailSubscription,
    handleClickDetailSubscription,
    handleConfirmWithAction,
  };
};
