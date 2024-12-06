<template>
<ModalComponent :model-value="isShowModal" :beforeClose="handleClose" :width="`1100px`">
    <div class="modal-body">
        <!-- Pricing Plans -->
        <div class="rounded-top">
            <h4 class="text-center mb-2">Pricing Plans</h4>
            <p class="text-center mb-0">All plans include 40+ advanced tools and features to boost your product. Choose the best plan to fit your needs.</p>
            <div class="d-flex align-items-center justify-content-center flex-wrap gap-2 pt-12 pb-4">
                <div class="toggle">

                </div>
            </div>
            <div class="row gy-6">
                <!-- Basic -->
                <div class="col-xl mb-md-0" v-for="plan in responseSubscription" v-key="plan.id">
                    <div class="card border rounded shadow-none">
                        <div class="card-body pt-12">
                            <div class="mt-3 mb-5 text-center">
                                <img v-if="plan.name === 'Basic Plan'" src="@/assets/images/subscriptions/pricing-plan-basic.png" alt="Basic Image" width="100">
                                <img v-if="plan.name === 'Standard Plan'" src="@/assets/images/subscriptions/pricing-plan-standard.png" alt="Basic Image" width="100">
                                <img v-if="plan.name === 'Premium Plan'" src="@/assets/images/subscriptions/pricing-plan-enterprise.png" alt="Basic Image" width="100">
                            </div>
                            <h4 class="card-title text-center text-capitalize mb-1">{{ plan.name }}</h4>
                            <p class="text-center mb-5">{{ plan.description }}</p>
                            <div class="text-center h-px-50">
                                <div class="d-flex justify-content-center" v-if="plan.price === 0">
                                    <sup class="h6 text-body pricing-currency mt-2 mb-0 me-1"></sup>
                                    <h3 class="mb-0 text-primary">Miễn phí</h3>
                                    <sub class="h6 text-body pricing-duration mt-auto mb-1"></sub>
                                </div>
                                <div class="d-flex justify-content-center" v-else>
                                    <sup class="h6 text-body pricing-currency mt-2 mb-0 me-1">VND</sup>
                                    <h2 class="mb-0 text-primary">{{ formatCurrency(plan.price) }}</h2>
                                    <sub class="h6 text-body pricing-duration mt-auto mb-1">/{{ isAnnual ? 'năm' : 'tháng' }}</sub>
                                </div>
                            </div>

                            <ul class="list-group my-5 pt-9">
                                <li v-for="feature in plan.features" v-key="feature.id" class="mb-4 d-flex align-items-center">
                                    <span class="badge p-50 w-px-20 h-px-20 rounded-pill bg-label-primary me-2">
                                        <i class="bx bx-check bx-xs"></i>
                                    </span><span>{{ feature.featureName }}</span>
                                </li>
                            </ul>
                            <div v-if="plan.name === 'Basic Plan'">
                                <button v-if="plan.id === subscriptionCurrent.SubscriptionPlanId" class="btn current-plan-button d-grid w-100">
                                    Gói hiện tại
                                </button>
                                <button v-else class="btn btn-label-success d-grid w-100" data-bs-dismiss="modal">Miễn phí để sử dụng</button>
                            </div>
                            <div v-else>
                                <button v-if="plan.id === subscriptionCurrent.SubscriptionPlanId" class="btn current-plan-button d-grid w-100">
                                    Gói hiện tại
                                </button>
                                <button v-else type="button" class="btn btn-primary d-grid w-100" @click="handleUpgrade(plan.id)">Nâng cấp</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!--/ Pricing Plans -->
    <template #footer>
        <div class="dialog-footer">
            <slot name="footer">
                <el-button @click="handleClose">Quay lại</el-button>
            </slot>
        </div>
    </template>
</ModalComponent>
</template>

    
<script lang="ts">
import {
    defineComponent,
    PropType,
    ref,
    watch
} from 'vue';
import ModalComponent from '@/components/common/Modal.vue';
import {
    ResponseCurrentSchoolSubscriptionInterface,
    ResponseSubscriptionInterface
} from '@/types/model/school-subscription';

export default defineComponent({
    name: 'UpdateSubscriptionModal',
    components: {
        ModalComponent
    },
    props: {
        isShowModal: {
            type: Boolean,
            required: true
        },
        responseSubscription: {
            type: Object as PropType < ResponseSubscriptionInterface > ,
            required: true
        },
        subscriptionCurrent: {
            type: Object as PropType < ResponseCurrentSchoolSubscriptionInterface > ,
            required: true
        },
        formatCurrency: {
            type: Function as PropType < (amount: number) => string > ,
            required: true,
        }
    },
    emits: ['handleClose', 'handleUpgrade'],
    setup(props, {
        emit
    }) {
        const handleClose = () => {
            emit('handleClose');
        };

        const handleUpgrade = (subscriptionId: number) => {
            emit('handleUpgrade', subscriptionId);
        };

        return {
            handleClose,
            handleUpgrade,
        };
    },
});
</script>

    
<style scoped>
.modal-body {
    text-align: center;
}

.pricing-plans {
    display: flex;
    justify-content: space-around;
    margin-top: 20px;
    gap: 20px;
}

.plan {
    border: 1px solid #ddd;
    border-radius: 10px;
    padding: 20px;
    width: 250px;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    transition: transform 0.3s, box-shadow 0.3s;
}

.plan-header {
    text-align: center;
}

.plan-header img {
    width: 80px;
    height: auto;
    margin-bottom: 10px;
}

.plan-header h3 {
    font-size: 18px;
    color: #333;
    margin: 5px 0;
}

.plan-header .description {
    font-size: 14px;
    color: #777;
    margin: 5px 0;
}

.plan-header .price {
    font-size: 22px;
    color: #555;
    margin: 10px 0;
}

.plan-features {
    flex-grow: 1;
    margin-top: 10px;
}

.plan-features ul {
    list-style-type: none;
    padding: 0;
    margin: 0;
    color: #666;
    text-align: left;
}

.plan-features ul li {
    margin: 5px 0;
}

.plan-footer {
    text-align: center;
    margin-top: 15px;
}

.plan-footer button {
    background-color: #007bff;
    color: #fff;
    border: none;
    padding: 10px 20px;
    border-radius: 5px;
    cursor: pointer;
    transition: background-color 0.3s;
}

.current-plan-button {
    background-color: #e6f4ea;
    color: #53d669;
    border: none;
    border-radius: 12px;
    padding: 12px;
    font-size: 16px;
    text-align: center;
}

.plan-footer button.current-plan {
    background-color: #e0ffe0;
    color: #333;
    cursor: default;
}

.plan:hover {
    transform: scale(1.05);
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
}

.plan img {
    width: 80px;
    height: auto;
    margin-bottom: 10px;
}

.toggle {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-bottom: 20px;
}

.toggle label {
    margin: 0 10px;
    font-weight: bold;
}

.toggle-switch {
    position: relative;
    width: 50px;
    height: 24px;
    background-color: #ddd;
    border-radius: 15px;
    margin: 0 10px;
    cursor: pointer;
}

.toggle-switch::before {
    content: '';
    position: absolute;
    width: 20px;
    height: 20px;
    background-color: #fff;
    border-radius: 50%;
    top: 2px;
    left: 2px;
    transition: 0.3s;
}

.toggle-switch:checked::before {
    left: 26px;
    background-color: #007bff;
}

.discount-badge {
    background-color: #ffbb33;
    color: #fff;
    padding: 2px 8px;
    border-radius: 5px;
    font-size: 12px;
}

@media (max-width: 600px) {
    .pricing-plans {
        flex-direction: column;
        align-items: center;
    }

    .plan {
        width: 100%;
        max-width: 300px;
        margin-bottom: 20px;
    }
}
</style>
