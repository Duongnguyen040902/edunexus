import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import RepositoryFactory from '@/repositories/repository';
import { AdminSchoolRepository } from '@/repositories/repository-admin-school';
import {
  ErrorResponseAdminSchool,
  RequestAdminSchoolIndex,
  RequestAdminSchoolUpdate,
  RequestCreateSchoolAdmin,
  ResponseAdminSchoolDetail,
  ResponseAdminSchoolIndex,
  ResponseClasses,
  SchoolInterface,
} from '@/types/model/admin-school';
import { defineStore } from 'pinia';
import { reactive } from 'vue';
import { mapErrorKeys, clearErrorKeys } from '@/helpers/state';
import { RequestGetListClassesInterface, ViewClassAdminResponseInterface } from '@/types/model/school-admin';
import { SchoolAdminRepository } from '@/repositories/repository-schoolAdmin';
export const useSchoolAdminStore = defineStore('schoolAdmin', () => {
  const adminSchoolFactory = RepositoryFactory.create('adminSchool') as AdminSchoolRepository;
  const classForSchoolAdmin = reactive<{ value: ResponseClasses | null }>({
    value: {
      pageNumber: 1,
      pageSize: 10,
      firstPage: 1,
      lastPage: 1,
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });
  const requestGetClass= reactive<RequestGetListClassesInterface>({
    pageNumber: 1,
    pageSize: 10,
    status: null,
    keyword: '',
  });
  const requestAdminSchoolIndex = reactive<RequestAdminSchoolIndex>({
    pageNumber: 1,
    pageSize: 10,
    status: '',
    keyword: '',
    subscriptionPlanId: '',
  });

  const dataSchoolAdmin = reactive<{ value: ResponseAdminSchoolIndex }>({
    value: {
      pageNumber: 1,
      pageSize: 10,
      firstPage: 1,
      lastPage: 1,
      totalPages: 0,
      totalRecords: 0,
      nextPage: null,
      previousPage: null,
      data: [],
    },
  });

  const requestAdminSchoolUpdate = reactive<RequestAdminSchoolUpdate>({
    id: 0,
    address: '',
    phoneNumber: '',
    websiteLink: '',
    schoolName: '',
    accountStatus: 0,
    email: '',
    standardCode: '',
    dateOfEstablishment: '',
  });

  const requestCreateSchoolAdmin = reactive<RequestCreateSchoolAdmin>({
    username: '',
    password: '',
    schoolName: '',
    address: '',
    phoneNumber: null,
    email: '',
    subscriptionPlanId: null,
    paymentMethod: '',
  });

  const schoolAdminDetail = reactive<{ value: ResponseAdminSchoolDetail }>({
    value: {
      school: {} as SchoolInterface,
      schoolSubscriptionPlans: [],
    },
  });
  const errorAdminSchool = reactive(<ErrorResponseAdminSchool>{
    Address: [],
    PhoneNumber: [],
    WebsiteLink: [],
    SchoolName: [],
    Email: [],
    StandardCode: [],
    DateOfEstablishment: [],
    AccountStatus: [],
    SubscriptionPlanId: [],
    PaymentMethod: [],
    Username: [],
    Password: [],
  });

  const errorAdminSchoolKeys: (keyof ErrorResponseAdminSchool)[] = [
    'Address',
    'PhoneNumber',
    'WebsiteLink',
    'SchoolName',
    'Email',
    'StandardCode',
    'DateOfEstablishment',
    'AccountStatus',
    'SubscriptionPlanId',
    'PaymentMethod',
    'Username',
    'Password',
  ];
  const getAllAccountSchoolAdmin = async () => {
    await adminSchoolFactory.getAllAccountSchoolAdmin(
      requestAdminSchoolIndex,
      (res: SuccessResponse) => {
        const response = res.data as ResponseAdminSchoolIndex;

        dataSchoolAdmin.value = response;
      },
      (err: ErrorResponse) => {
        console.log('err', err);
      },
    );
  };

  const getSchoolDetail = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await adminSchoolFactory.getSchoolDetail(
      id,
      res => {
        const response = res.data as ResponseAdminSchoolDetail;
        schoolAdminDetail.value.school.id = response.id;
        schoolAdminDetail.value.school.username = response.username;
        schoolAdminDetail.value.school.address = response.address;
        schoolAdminDetail.value.school.phoneNumber = response.phoneNumber;
        schoolAdminDetail.value.school.websiteLink = response.websiteLink;
        schoolAdminDetail.value.school.schoolName = response.schoolName;
        schoolAdminDetail.value.school.email = response.email;
        schoolAdminDetail.value.school.standardCode = response.standardCode;
        schoolAdminDetail.value.school.dateOfEstablishment = response.dateOfEstablishment;
        schoolAdminDetail.value.schoolSubscriptionPlans = response.schoolSubscriptionPlans;
        schoolAdminDetail.value.school.accountStatus = Number(response.accountStatus);
        schoolAdminDetail.value.school.statusName = response.statusName;
        return success(res);
      },
      err => {
        console.log('err', err);
        error(err);
      },
    );
  };

  const updateSchoolAdmin = async (
    id: number,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) => {
    await adminSchoolFactory.updateSchoolAdmin(
      id,
      requestAdminSchoolUpdate,
      res => {
        clearErrorKeys(errorAdminSchoolKeys, errorAdminSchool);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseAdminSchool;
        mapErrorKeys(errorAdminSchoolKeys, errorAdminSchool, errorsResponse);
        return error(err);
      },
    );
  };

  const getAllClass = async () => {
    await adminSchoolFactory.getListClass(
      requestGetClass,
      res => {
        classForSchoolAdmin.value = res.data as ResponseClasses;
        return res.data;
      },
      (err: ErrorResponse) => {
      },
    );
  };

  const createSchoolAdmin = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
    await adminSchoolFactory.createSchoolAdmin(
      requestCreateSchoolAdmin,
      res => {
        clearErrorKeys(errorAdminSchoolKeys, errorAdminSchool);
        return success(res);
      },
      err => {
        const errorsResponse = err.errors as ErrorResponseAdminSchool;
        mapErrorKeys(errorAdminSchoolKeys, errorAdminSchool, errorsResponse);
        return error(err);
      },
    );
  };

  return {
    errorAdminSchool,
    errorAdminSchoolKeys,
    requestAdminSchoolUpdate,
    requestAdminSchoolIndex,
    requestCreateSchoolAdmin,
    dataSchoolAdmin,
    schoolAdminDetail,
    classForSchoolAdmin,
    requestGetClass,
    getAllClass,
    getAllAccountSchoolAdmin,
    getSchoolDetail,
    updateSchoolAdmin,
    createSchoolAdmin,
  };
});
