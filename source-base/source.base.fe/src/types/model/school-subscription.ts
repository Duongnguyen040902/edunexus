export interface RequestCurrentSchoolSubscriptionInterface {
    schoolId: number;
};

export interface ResponseCurrentSchoolSubscriptionInterface {
    id: number;
    subscriptionPlanName: string;
    SubscriptionPlanId: number
    description: string;
    price: number;
    durationDays: number;
    startDate: string;
    endDate: string;
    features: [
        {
            id: number;
            featureName: string;
            description: string;
        }
    ];
};


export interface RequestSubscriptionInterface {
    durationDays: number;
};
export interface ResponseSubscriptionInterface {
    id: number;
    name: string;
    description: string;
    price: number;
    durationDays: number;
    features: [
        {
            id: number;
            featureName: string;
            description: string;
        }
    ];
};

export interface RequestCreateInvoiceInterface {
    subscriptionPlanId: number;
};

export interface ResponseInvoiceInterface {
    id: number;
    schoolId: number;
    subscriptionPlanName: string;
    totalAmount: number;
    status: number;
    durationDays: number;
    startDate: string;
    endDate: string;
};

export interface RequestGeneratePayMentUrlInterface {
    invoiceId: number;
};


export interface RequestSubscriptionDetailInterface {
    subscriptionId: number;
};
export interface ResponseSubscriptionDetailInterface {
    id: number;
    name: string;
    description: string;
    price: number;
    durationDays: number;
    startDate: string;
    endDate: string;
};
export interface RequestAllSchoolSubscriptionInterface {
    pageNumber: number;
    pageSize: number;
    status?: number | null;
    year?: string | null;
};
export interface ResponseAllSchoolSubscriptionInterface {
    pageNumber: number;
    pageSize: number;
    firstPage: string;
    lastPage: string;
    totalPages: number;
    totalRecords: number;
    nextPage: string;
    previousPage: string;
    data: [
        {
            id: number;
            subscriptionPlanId: number;
            subscriptionPlanName: string;
            description: string;
            price: number;
            durationDays: number;
            startDate: string;
            endDate: string;
            status: number;
            statusName: string;
        }
    ];
};