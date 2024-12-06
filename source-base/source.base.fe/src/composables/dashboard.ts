import { startLoading, endLoading } from '@/helpers/mixins';
import { notifyError } from '@/helpers/notify';
import { useDashboardStore } from '@/stores/dashboard.ts';

export const useDashboardComposable = () => {
  const dashboardStore = useDashboardStore();
  const {
    responseDashboardResponseMonthlyRevenues,
    responseDashboardResponseTotalSchool,
    responseDashboardResponseTotalSchoolSubscription,
    responseDashboard,
    requestDashboard,
    getDashboardData,
    getTotalSchoolStatus,
    getTotalSchoolSubscription,
  } = dashboardStore;
  const handleGetAllDataDashboard = async () => {
    startLoading();
    await getDashboardData(
      () => {
        endLoading();
      },
      () => {
        notifyError('Lỗi khi lấy dữ liệu bảng điều khiển');
        endLoading();
      },
    );
  };

  const handleGetTotalSchoolStatus = async () => {
    startLoading();
    await getTotalSchoolStatus(
      () => {
        endLoading();
      },
      () => {
        notifyError('Lỗi khi lấy dữ liệu bảng điều khiển');
        endLoading();
      },
    );
  };

  const handleGetTotalSchoolSubscription = async () => {
    startLoading();
    await getTotalSchoolSubscription(
      () => {
        endLoading();
      },
      () => {
        notifyError('Lỗi khi lấy dữ liệu bảng điều khiển');
        endLoading();
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
    handleGetAllDataDashboard,
    handleGetTotalSchoolStatus,
    handleGetTotalSchoolSubscription,
  };
};
