import { ROUTER_PATHS } from '@/constants/api/router-paths';

export interface SubMenu {
  label: string;
  link: string;
  icon?: string;
}

export interface Menu {
  id: string;
  label: string;
  icon: string;
  badge?: number;
  subMenus?: SubMenu[];
  link?: string;
}

export const AdminMenu: Menu[] = [
  {
    id: 'dashboard',
    label: 'Báo cáo',
    icon: 'menu-icon bx bxs-user-account',
    link: ROUTER_PATHS.ADMIN.DASHBOARD,
  },
  {
    id: 'school-admin',
    label: 'Quản lý trường học',
    icon: 'menu-icon bx bxs-school',
    subMenus: [{ label: 'Danh sách trường học', link: ROUTER_PATHS.ADMIN.SCHOOL }],
  },
  {
    id: 'invoice',
    label: 'Quản lý hóa đơn',
    icon: 'menu-icon bx bx-food-menu',
    subMenus: [{ label: 'Danh sách Hóa đơn', link: ROUTER_PATHS.ADMIN.INVOICE }],
  },
  {
    id: 'subscription',
    label: 'Quản lý gói',
    icon: 'menu-icon bx bx-book-bookmark',
    subMenus: [{ label: 'Danh sách gói', link: ROUTER_PATHS.ADMIN.PACKAGE }],
  },
];

export const SchoolMenu: Menu[] = [
  {
    id: 'dashboard',
    label: 'Tổng quan',
    icon: 'bx bx-tachometer menu-icon',
    link: '/school/dashboard' ,
    subMenus: [],
  },
  {
    id: 'profile',
    label: 'Thông tin nhà trường',
    icon: 'bx bx-user-circle bx bx-dock-top menu-icon',
    subMenus: [
      { label: 'Quản lý gói', link: ROUTER_PATHS.SCHOOL_ADMIN.SUBSCRIPTION_MANAGER },
      { label: 'Lịch sử hóa đơn', link: ROUTER_PATHS.SCHOOL_ADMIN.INVOICE_MANAGER },
    ],
  },
  {
    id: 'account',
    label: 'Quản lý tài khoản',
    icon: 'bx bx bx-user bx-dock-top menu-icon',
    subMenus: [
      { label: 'Giáo viên', link: ROUTER_PATHS.SCHOOL_ADMIN.TEACHER_ACCOUNT },
      { label: 'Học sinh', link: ROUTER_PATHS.SCHOOL_ADMIN.PUPIL_ACCOUNT },
      { label: 'Quản lý xe tuyến', link: ROUTER_PATHS.SCHOOL_ADMIN.BUS_SUPERVISOR },
    ],
  },
  {
    id: 'busRoute',
    label: 'Quản lý xe tuyến',
    icon: 'menu-icon tf-icons bx bxs-bus',
    link: ROUTER_PATHS.BUS_ROUTE.INDEX,
    subMenus: [],
  },
  {
    id: 'class',
    label: 'Quản lý lớp học',
    icon: 'menu-icon tf-icons bx bx-chalkboard',
    link: ROUTER_PATHS.SCHOOL_ADMIN.CLASS_MANAGER,
    subMenus: [],
  },
  {
    id: 'club',
    label: 'Quản lý câu lạc bộ',
    icon: 'menu-icon tf-icons bx bx-group',
    link:ROUTER_PATHS.SCHOOL_ADMIN.CLUB_MANAGEMENT,
    subMenus: [],
  },
  {
    id: 'schoolYear',
    label: 'Quản lý năm, kỳ học',
    icon: 'menu-icon tf-icons bx bx-calendar',
    link: ROUTER_PATHS.SCHOOL_ADMIN.SCHOOLYEAR_MANAGER,
    subMenus: [],
  },
  {
    id: 'subject',
    label: 'Quản lý môn',
    icon: 'menu-icon tf-icons bx bxs-book-bookmark',
    link: ROUTER_PATHS.SCHOOL_ADMIN.SUBJECT_MANAGE,
    subMenus: [],
  },
  {
    id: 'time-slot',
    label: 'Quản lý tiết học',
    icon: 'menu-icon tf-icons bx bx-time',
    link: ROUTER_PATHS.SCHOOL_ADMIN.TIME_SLOT,
    subMenus: [],
  },
  {
    id: 'changePassword',
    label: 'Đổi mật khẩu',
    icon: 'menu-icon  bx bx-lock',
    link: ROUTER_PATHS.SCHOOL_ADMIN.CHANGE_PASSWORD,
    subMenus: [],
  }
];

export const PupilMenu: Menu[] = [

  {
    id: 'personalInfo',
    label: 'Thông tin cá nhân',
    icon: 'bx bx-user-circle bx bx-dock-top menu-icon',
    link: ROUTER_PATHS.PUPIL.PROFILE,
    subMenus: [],
  },
  {
    id: 'myClass',
    label: 'Lớp của tôi',
    icon: 'bx bx-chalkboard menu-icon',
    link: ROUTER_PATHS.PUPIL.MY_CLASS,
    subMenus: [],
  },
  {
    id: 'historyRegisterClub',
    label: 'Lịch sử đăng kí CLB',
    icon: 'bx bx-history menu-icon',
    link: ROUTER_PATHS.PUPIL.HISTORY_REGISTER_CLUB,
    subMenus: [],
  },
  {
    id: 'myBus',
    label: 'Lịch sử xe tuyến',
    icon: 'bx bx-bus menu-icon',
    link: ROUTER_PATHS.PUPIL.MY_BUS,
    subMenus: [],
  },
  {
    id: 'registerClub',
    label: 'Đăng kí câu lạc bộ',
    icon: 'bx bx-calendar-plus menu-icon',
    link: ROUTER_PATHS.PUPIL.REGISTER_CLUB,
    subMenus: [],
  },
  {
    id: 'generalManagement',
    label: 'Quản lý chung',
    icon: 'menu-icon tf-icons bx bx-dock-top',
    subMenus: [
      { label: 'Đơn', icon: 'bx bx-send menu-icon', link: ROUTER_PATHS.PUPIL.APPLICATION },
      { label: 'Điểm danh', icon: 'bx bx-message-alt-check menu-icon', link: ROUTER_PATHS.PUPIL.ATTENDANCE },
      { label: 'Điểm số', icon: 'bx bxs-graduation menu-icon', link: ROUTER_PATHS.PUPIL.SCORE },
      { label: 'Phản hồi', icon: 'bx bx-message-alt-edit menu-icon', link: ROUTER_PATHS.PUPIL.FEEDBACK },
    ],
  },
  {
    id: 'changePassword',
    label: 'Đổi mật khẩu',
    icon: 'menu-icon bx bx-lock',
    link: ROUTER_PATHS.PUPIL.CHANGE_PASSWORD,
    subMenus: [],
  }
];

export const TeacherMenu: Menu[] = [
  {
    id: 'personalInfo',
    label: 'Thông tin cá nhân',
    icon: 'bx bx-user-circle bx bx-dock-top menu-icon',
    link: ROUTER_PATHS.TEACHER.PROFILE,
    subMenus: [],
  },
  {
    id: 'classManagement',
    label: 'Quản lý lớp',
    icon: 'menu-icon tf-icons bx bx-dock-top',
    subMenus: [],
  },
  {
    id: 'clubManagement',
    label: 'Quản lý câu lạc bộ',
    icon: 'menu-icon tf-icons bx bx-dock-top',
    subMenus: [],
  },
  {
    id: 'changePassword',
    label: 'Đổi mật khẩu',
    icon: 'menu-icon  bx bx-lock',
    link: ROUTER_PATHS.TEACHER.CHANGE_PASSWORD,
    subMenus: [],
  }
];

export const SupervisorMenu: Menu[] = [

  {
    id: 'personalInfo',
    label: 'Thông tin cá nhân',
    icon: 'bx bx-user-circle bx bx-dock-top menu-icon',
    link: ROUTER_PATHS.SUPERVISOR.PROFILE,
    subMenus: [],
  },
  {
    id: 'bus',
    label: 'Quản lý xe tuyến',
    icon: 'menu-icon tf-icons bx bx-dock-top',
    subMenus: [],
  },
  {
    id: 'changePassword',
    label: 'Đổi mật khẩu',
    icon: 'menu-icon bx bx-lock',
    link: ROUTER_PATHS.SUPERVISOR.CHANGE_PASSWORD,
    subMenus: [],
  }
];
