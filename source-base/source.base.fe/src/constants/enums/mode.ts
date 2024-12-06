export const ModeLogin = [
  { value: 0, label: 'Quản trị viên hệ thống' },
  { value: 1, label: 'Quản trị viên trường' },
  { value: 2, label: 'Phụ huynh' },
  { value: 3, label: 'Giáo viên' },
  { value: 4, label: 'Bus' },
];

export enum ShortRoleName {
  Admin = 'SPA',
  SchoolAdmin = 'SA',
  Parent = 'DN',
  Teacher = 'TC',
  Bus = 'BSV',
}

export const StatusTimeSlot = [
  { value: true, label: 'Đang hoạt động' },
  { value: false, label: 'Không hoạt động' },
];
