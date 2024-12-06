import internal from "stream";

export interface RequestAdminSchoolIndex {
  pageNumber: number;
  pageSize: number;
  status?: number | null;
  keyword?: string;
  subscriptionPlanId?: number | null | '';
}

export interface ResponseAdminSchoolIndex {
  pageNumber: number;
  pageSize: number;
  firstPage: number;
  lastPage: number;
  totalPages: number;
  totalRecords: number;
  nextPage: number | null;
  previousPage: number | null;
  data: Array<{
    id: number;
    address: string | null;
    phoneNumber: string | null;
    websiteLink: string | null;
    schoolName: string;
    email: string;
    standardCode: string | null;
    accountStatus: number;
    statusName: string;
    dateOfEstablishment: string | null;
    schoolSubscriptionPlans: Array<{
      id: number;
      status: number;
      startDate: string;
      endDate: string;
      subscriptionPlan: {
        id: number;
        name: string;
      };
      invoices: Array<{
        id: number;
        issueDate: string;
        dueDate: string;
        status: number;
        payments: Array<{
          id: number;
          amount: number;
          paymentDate: string;
          paymentMethod: string;
          status: number;
        }>;
      }>;
    }>;
  }>;
}

export interface Payment {
  id: number;
  amount: number;
  paymentDate: string;
  paymentMethod: string;
  status: number;
}

export interface Invoice {
  id: number;
  issueDate: string;
  dueDate: string;
  status: number;
  statusName: string;
  schoolName?: string;
  subscriptionPlanName?: string;
  payments: Payment[];
}

export interface SubscriptionPlan {
  id: number;
  name: string;
}

export interface SchoolSubscriptionPlan {
  id: number;
  status: number;
  startDate: string;
  endDate: string;
  subscriptionPlan: SubscriptionPlan;
  invoices: Invoice;
}

export interface SchoolInterface {
  id: number;
  username: string;
  address: string | null;
  phoneNumber: string | null;
  websiteLink: string | null;
  schoolName: string;
  email: string;
  standardCode: string | null;
  dateOfEstablishment: string | null;
  accountStatus?: number;
  statusName?: string;
  image?: string;
}

export interface ResponseAdminSchoolDetail {
  school: SchoolInterface;
  schoolSubscriptionPlans: SchoolSubscriptionPlan[];
}

export interface RequestAdminSchoolUpdate {
  id: number;
  address: string | null;
  phoneNumber: string | null;
  websiteLink: string | null;
  schoolName: string;
  accountStatus: number;
  email: string | null;
  standardCode: string | null;
  dateOfEstablishment: string | null;
}

export interface RequestCreateSchoolAdmin {
  username: string;
  password: string;
  schoolName: string;
  address: string;
  phoneNumber: string | null;
  email: string;
  subscriptionPlanId: number | null | '';
  paymentMethod: string | null;
}

export interface ErrorResponseAdminSchool {
  Username?: string[];
  Password?: string[];
  Address?: string[];
  PhoneNumber?: string[];
  WebsiteLink?: string[];
  SchoolName?: string[];
  Email?: string[];
  StandardCode?: string[];
  DateOfEstablishment?: string[];
  AccountStatus?: string[];
  SubscriptionPlanId?: string[];
  PaymentMethod?: string[];
}
export interface ViewClassAdminResponseInterface {
  id?: number;
  className?: string;
  schoolId?: string;
  status?: number;
  block?: string;
}

export interface RequestGetListClassesInterface {
  pageNumber: number | null;
  pageSize: number | null;
  keyword?: string | null;
  status?: number | null;
}

export interface ResponseClasses {
  pageNumber: number;
  pageSize: number;
  firstPage: number;
  lastPage: number;
  totalPages: number;
  totalRecords: number;
  nextPage: number | null;
  previousPage: number | null;
  data: ViewClassAdminResponseInterface[];
}