import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository.ts';
import {
  ErrorResponseSubscription,
  RequestCreateSubscriptionInterface,
  Subscription,
} from '@/types/model/subscription.ts';
import { reactive } from 'vue';
import { SubscriptionRepository } from '@/repositories/repository-subscription.ts';
import { ErrorResponse, SuccessResponse } from '@/constants/api/responses.ts';
import { clearErrorKeys, mapErrorKeys } from '@/helpers/state.ts';

export const useSubscriptionStore = defineStore('subscription', () => {
  const subscriptionRepository = RepositoryFactory.create('subscription') as SubscriptionRepository;

  const responseSubscription = reactive<{ value: Subscription[] }>({
    value: [
      {
        id: 0,
        name: '',
        description: '',
        price: 0,
        durationDays: 0,
        maxActiveAccounts:0,
      },
    ],
  });

  const requestCreateSubscription = reactive<RequestCreateSubscriptionInterface>({
    id: 0,
    name: '',
    description: '',
    price: 0,
    durationDays: 0,
    maxActiveAccounts: 0,
  });

  const errorSubscriptionKeys: (keyof ErrorResponseSubscription)[] = ['Name', 'Description', 'Price', 'DurationDays', 'MaxActiveAccounts'];

  const errorSubscription = reactive<ErrorResponseSubscription>({
    Name: [],
    Description: [],
    Price: [],
    DurationDays: [],
    MaxActiveAccounts: [],
  });
  
  const getAllSubscription = async () => {
    await subscriptionRepository.getAllSubscription(
      success => {
        const response = success.data as Subscription[];
        responseSubscription.value = response;
      },
      error => {
        console.error(error);
      },
    );
  };

  const createSubscription = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await subscriptionRepository.createServicePackage(
      requestCreateSubscription,
      res => {
        clearErrorKeys(errorSubscriptionKeys, errorSubscription);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseSubscription;
        mapErrorKeys(errorSubscriptionKeys, errorSubscription, errorsResponse);
        error(err);
      },
    );
  };

  const updateSubscription = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await subscriptionRepository.updateServicePackage(
      id,
      requestCreateSubscription,
      res => {
        clearErrorKeys(errorSubscriptionKeys, errorSubscription);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseSubscription;
        mapErrorKeys(errorSubscriptionKeys, errorSubscription, errorsResponse);
        error(err);
      },
    );
  };

  return {
    requestCreateSubscription,
    responseSubscription,
    errorSubscription,
    getAllSubscription,
    createSubscription,
    updateSubscription,
  };
});
