export interface DashboardDataResponse {
  monthlyRevenues: DashboardResponseMonthlyRevenues[];
  userStatusCounts: DashboardResponseTotalSchool[];
  totalSubscriptions: DashboardResponseTotalSchoolSubscription[];
}

export interface RequestGetDashboardDataInterface {
  year: number;
  filterType?: string;
}

export interface DashboardResponseMonthlyRevenues {
  month: number;
  totalRevenue: number;
}

export interface DashboardResponseTotalSchool {
  dataStatus: [];
  total: number;
}

export interface DashboardResponseTotalSchoolSubscription {
  totalSubscription: number;
  planName: string;
}
