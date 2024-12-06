//stores/subscription.ts
import { ErrorResponse } from "@/constants/api/responses";
import RepositoryFactory from "@/repositories/repository";
import { SubscriptionRepository } from "@/repositories/repository-subscription";
import { ResponseSubscriptionInterface } from "@/types/model/school-subscription";
import { defineStore } from "pinia";
import { reactive } from "vue";


export const useSubscriptionStore = defineStore('subscription', () => {
    const SubscriptionFactory = RepositoryFactory.create('subscription') as SubscriptionRepository;

    const responseSubscription = reactive<{ value: ResponseSubscriptionInterface}>({
        value: {
        id: 0,
        name: '',
        description: '',
        price: 0,
        durationDays: 0,
        features: [
            {
                id: 0,
                featureName: '',
                description: '',
            }
        ]
    }
    });

    const getSubscriptionByDurationDay = async () => {
        await SubscriptionFactory.getSubscription(
            res => {
                responseSubscription.value = res.data as ResponseSubscriptionInterface;        
            },
            err => {
                console.log(err);
            },
        );
    };
    


    return {
        getSubscriptionByDurationDay,    
        responseSubscription,
    }

});
