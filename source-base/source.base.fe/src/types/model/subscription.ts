export interface Subscription {
  id: number;
  name: string;
  description: string;
  price: number;
  durationDays: number;
  maxActiveAccounts: number;
}

export interface RequestCreateSubscriptionInterface {
  id?: number ;
  name: string;
  description: string;
  price: number;
  durationDays: number;
  maxActiveAccounts: number;
}

export interface ErrorResponseSubscription {
  Name?: string[];
  Description?: string[];
  Price?: string[];
  DurationDays?: string[];
  MaxActiveAccounts?: string[];
}