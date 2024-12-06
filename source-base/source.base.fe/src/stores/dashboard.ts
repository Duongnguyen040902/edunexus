import { defineStore } from 'pinia';
import RepositoryFactory from '@/repositories/repository';
import { DashboardRepository } from '@/repositories/repository-dashboard.ts';
import { reactive } from 'vue';
import {
  DashboardDataResponse,
  DashboardResponseMonthlyRevenues,
  DashboardResponseTotalSchool,
  DashboardResponseTotalSchoolSubscription,
  RequestGetDashboardDataInterface,
} from '@/types/model/dashboard.ts';

export const useDashboardStore = defineStore('dashboard', () => {
  const dashboardFactory = RepositoryFactory.create('dashboard') as DashboardRepository;
  const requestDashboard = reactive<RequestGetDashboardDataInterface>({
    year: new Date().getFullYear(),
    filterType: '',
  });
  const responseDashboard = reactive<{ value: DashboardDataResponse }>({
    value: {
      monthlyRevenues: [],
      userStatusCounts: [],
      totalSubscriptions: [],
    },
  });

  const responseDashboardResponseMonthlyRevenues = reactive<{ value: DashboardResponseMonthlyRevenues }>({
    value: {
      month: 0,
      totalRevenue: 0,
    },
  });

  const responseDashboardResponseTotalSchool = reactive<{ value: DashboardResponseTotalSchool }>({
    value: {
      dataStatus: [],
      total: 0,
    },
  });

  const responseDashboardResponseTotalSchoolSubscription = reactive<{
    value: DashboardResponseTotalSchoolSubscription;
  }>({
    value: {
      totalSubscription: 0,
      planName: '',
    },
  });

  const getDashboardData = async (success: (res: any) => void, error: (err: any) => void) => {
    await dashboardFactory.GetRevenue(
      requestDashboard,
      res => {
        const response = res.data as DashboardResponseMonthlyRevenues;
        responseDashboardResponseMonthlyRevenues.value = response;

        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const getTotalSchoolStatus = async (success: (res: any) => void, error: (err: any) => void) => {
    await dashboardFactory.GetTotalSchoolAsync(
      requestDashboard,
      res => {
        const response = res.data as DashboardResponseTotalSchool;
        responseDashboardResponseTotalSchool.value = response;

        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  const getTotalSchoolSubscription = async (success: (res: any) => void, error: (err: any) => void) => {
    await dashboardFactory.GetTotalSchoolSubscriptionAsync(
      requestDashboard,
      res => {
        const response = res.data as DashboardResponseTotalSchoolSubscription;
        responseDashboardResponseTotalSchoolSubscription.value = response;

        return success(res);
      },
      err => {
        error(err);
      },
    );
  };

  return {
    responseDashboardResponseMonthlyRevenues,
    responseDashboardResponseTotalSchool,
    responseDashboardResponseTotalSchoolSubscription,
    responseDashboard,
    requestDashboard,
    getDashboardData,
    getTotalSchoolStatus,
    getTotalSchoolSubscription,
  };
});
