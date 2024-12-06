export interface Payment {
  id: number;
  amount: number;
  invoiceId: number;
  paymentDate: string;
  paymentMethod: string;
  status: number;
}

export interface InvoiceData {
  id: number;
  issueDate: string;
  dueDate: string;
  status: number;
  schoolId: number;
  subscriptionPlanId: number;
  statusName: string | null;
  schoolName: string | null;
  subscriptionPlanName?: string;
  totalAmount: number | null;
  payments: Payment[];
}

export interface Invoice {
  pageNumber: number;
  pageSize: number;
  firstPage: number;
  lastPage: number;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: InvoiceData[];
}

export interface RequestIndexInvoice {
  pageNumber: number;
  pageSize: number;
  status?: number | null;
  keyword?: string;
}

export interface RequestCreateInvoiceManager {
  subscriptionPlanId: number;
  schoolId: number;
  issueDate: string;
  dueDate: string;
  status?: number;
  paymentMethod?: string;
}

export interface RequestDeleteInvoiceManager {
  ids: number[];
}

export interface ErrorResponseInvoice {
  IssueDate?: string[];
  DueDate?: string[];
  SubscriptionPlanId?: string[];
  SchoolId?: string[];
  Status?: string[];
  PaymentMethod?: string[];
}

export interface RequestCreatePayment {
  invoiceId: number;
  amount?: number;
  paymentDate: string;
  paymentMethod: string;
  status: number;
}

export interface ErrorResponsePayment {
  Amount?: string[];
  InvoiceId?: string[];
  PaymentDate?: string[];
  PaymentMethod?: string[];
  Status?: string[];
}
export interface RequestListInvoiceInterface {
  pageNumber: number;
  pageSize: number;
  invoiceStatus?: number | null;
  startDate?: Date | null;
  endDate?: Date | null;
  school?: number | null;
}

export interface ResponseListInvoiceInterface {
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
          price: number;
          invoiceStatusName: string;
          status: number;
          issueDate: string;
          dueDate: string;
      }
  ];
};

export interface RequestInvoiceDetailInterface {
  invoiceId: number;
};

export interface ResponseInvoiceDetailInterface {
  id: number;
  subscriptionPlanName: string;
  totalAmount: number;
  durationDays: number;
  statusName: string;
  status: number;
  issueDate: string;
  dueDate: string;
  startDate: string;
  endDate: string;
  payments: [
      {
          id: number;
          amount: number;
          paymentDate: string;
          paymentMethod: string;
          status: number;
      }
  ];
}