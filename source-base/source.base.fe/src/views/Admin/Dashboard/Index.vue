<template>
  <div class="container-xxl flex-grow-1 container-p-y">
    <div class="row">
      <div class="col-12">
        <div class="card mb-6">
          <h5 class="card-header">Báo cáo doanh thu</h5>
          <div class="card-body">
            <div class="row">
              <div class="col-md-8 col-sm-12 pe-0 mb-md-0 mb-2">
                <el-date-picker
                  v-model="requestDashboard.year"
                  class="me-3"
                  placeholder="Chọn năm"
                  size="large"
                  type="year"
                />
                <el-select v-model="requestDashboard.filterType" placeholder="Lọc" size="large" style="width: 240px">
                  <el-option v-for="item in selectedOption" :key="item.value" :label="item.label" :value="item.value" />
                </el-select>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="row">
      <div class="col-12 mb-4">
        <div class="card">
          <div ref="chartContainer" style="width: 100%; height: 500px"></div>
        </div>
      </div>
      <div class="col-12">
        <div class="card d-flex flex-row align-items-stretch" style="height: 500px">
          <div class="flex-grow-1 d-flex align-items-center justify-content-center border-end">
            <div ref="pieChartContainer" style="width: 90%; height: 90%"></div>
          </div>

          <div class="flex-grow-1 px-3">
            <div class="card text-center h-100 account-status" style="box-shadow: none">
              <div class="card-header nav-align-top">
                <div class="d-flex flex-column align-items-start gap-1">
                  <h3 class="mb-1">{{ responseDashboardResponseTotalSchool?.value?.total }}</h3>
                  <small>Tổng số người dùng</small>
                </div>
              </div>
              <div class="tab-content pt-0 pb-4">
                <div id="navs-pills-browser" class="tab-pane fade active show" role="tabpanel">
                  <div class="table-responsive text-start text-nowrap">
                    <table class="table table-borderless">
                      <thead>
                        <tr>
                          <th>#</th>
                          <th>Trạng thái</th>
                          <th>Tổng</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="(status, index) in totalSchoolStatus" :key="index">
                          <td>{{ index + 1 }}</td>
                          <td>
                            <div class="d-flex align-items-center">
                              <span class="text-heading">{{ status.label }}</span>
                            </div>
                          </td>
                          <td class="text-heading">{{ status.value }}</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref, watch } from 'vue';
import * as echarts from 'echarts';
import { useDashboardComposable } from '@/composables/dashboard.ts';
export default defineComponent({
  name: 'DashboardIndex',
  components: {
  },
  setup() {
    const dashboard = useDashboardComposable();
    const {
      requestDashboard,
      responseDashboardResponseMonthlyRevenues,
      responseDashboardResponseTotalSchoolSubscription,
      responseDashboardResponseTotalSchool,
      handleGetAllDataDashboard,
      handleGetTotalSchoolStatus,
      handleGetTotalSchoolSubscription,
    } = dashboard;
    const chartContainer = ref<HTMLDivElement | null>(null);
    const pieChartContainer = ref<HTMLDivElement | null>(null);

    const initializeBarChart = () => {
      if (chartContainer.value) {
        const myChart = echarts.init(chartContainer.value);
        const monthlyRevenues = responseDashboardResponseMonthlyRevenues.value.map(item => item.totalRevenue) ?? [];

        const option: echarts.EChartsOption = {
          title: { left: 'center' },
          tooltip: { trigger: 'axis' },
          legend: { data: ['Doanh thu'], top: '10%' },
          xAxis: {
            type: 'category',
            data: [
              'Tháng 1',
              'Tháng 2',
              'Tháng 3',
              'Tháng 4',
              'Tháng 5',
              'Tháng 6',
              'Tháng 7',
              'Tháng 8',
              'Tháng 9',
              'Tháng 10',
              'Tháng 11',
              'Tháng 12',
            ],
          },
          yAxis: { type: 'value' },
          series: [
            {
              name: 'Doanh thu',
              type: 'bar',
              data: monthlyRevenues,
              itemStyle: { color: '#42A5F5' },
            },
          ],
        };

        myChart.setOption(option);
        window.addEventListener('resize', () => myChart.resize());
      }
    };

    const initializePieChart = () => {
      if (pieChartContainer.value) {
        const pieChart = echarts.init(pieChartContainer.value);
        const subscriptionRevenue =
          responseDashboardResponseTotalSchoolSubscription.value.map(item => ({
            value: item.totalSubscription,
            name: item.planName,
          })) ?? [];

        const pieOption: echarts.EChartsOption = {
          title: { text: '', left: 'center' },
          tooltip: { trigger: 'item' },
          legend: { orient: 'vertical', left: 'left' },
          series: [
            {
              name: 'Gói dịch vụ',
              type: 'pie',
              radius: '50%',
              data: subscriptionRevenue,
              emphasis: {
                itemStyle: {
                  shadowBlur: 10,
                  shadowOffsetX: 0,
                  shadowColor: 'rgba(0, 0, 0, 0.5)',
                },
              },
            },
          ],
        };

        pieChart.setOption(pieOption);
        window.addEventListener('resize', () => pieChart.resize());
      }
    };

    onMounted(async () => {
      await handleGetAllDataDashboard();
      await handleGetTotalSchoolStatus();
      await handleGetTotalSchoolSubscription();
      initializeBarChart();
      initializePieChart();
    });

    watch(
      requestDashboard,
      async () => {
        await handleGetAllDataDashboard();
        await handleGetTotalSchoolStatus();
        await handleGetTotalSchoolSubscription();
        initializeBarChart();
        initializePieChart();
      },
      { deep: true },
    );

    const dataStatus = responseDashboardResponseTotalSchool?.value?.dataStatus ?? {};
    if (!dataStatus) return;

    const totalSchoolStatus = Object.entries(dataStatus).map(([label, value]) => ({
      label: label === 'active' ? 'Hoạt động' : label === 'inactive' ? 'Ngừng hoạt động' : label,
      value,
    }));

    const selectedOption = [
      { label: 'Từ đầu tháng đến nay', value: 'month' },
      { label: 'Từ đầu quý đến nay', value: 'quarter' },
      { label: 'Từ đầu năm đến nay', value: 'year' },
    ];

    return {
      chartContainer,
      pieChartContainer,
      totalSchoolStatus,
      responseDashboardResponseTotalSchool,
      requestDashboard,
      selectedOption,
    };
  },
});
</script>
