export enum InvoiceStatus {
  PENDING = 1,
  PAID = 2,
  CANCELED = 3,
  SENT = 4,
  CHANGE = 5,
}

export const InvoiceStatusLabels: { [key in InvoiceStatus]: string } = {
  [InvoiceStatus.PENDING]: 'Chờ',
  [InvoiceStatus.PAID]: 'Đã thanh toán',
  [InvoiceStatus.CANCELED]: 'Hủy',
  [InvoiceStatus.SENT]: 'Đã gửi',
  [InvoiceStatus.CHANGE]: 'Thay đổi',
};

export enum PaymentMethod {
  VN_PAY = 'VN PAY',
  MOMO = 'MOMO',
}

export enum PaymentStatuses {
  SUCCESS = 1,
  ERROR = 2,
}

export const PaymentStatusLabels: { [key in PaymentStatuses]: string } = {
  [PaymentStatuses.SUCCESS]: 'Thành công',
  [PaymentStatuses.ERROR]: 'Lỗi khi thanh toán',
};

export enum AccountStatus {
  ACTIVE = 1,
  INACTIVE = 2,
  DELETED = 3,
}

export const AccountStatusLabels: { [key in AccountStatus]: string } = {
  [AccountStatus.ACTIVE]: 'Hoạt động',
  [AccountStatus.INACTIVE]: 'Không hoạt động',
  [AccountStatus.DELETED]: 'Đã xóa',
};

export const StatusEditSchool = {
  'Hoạt động': 1,
  'Không hoạt động': 2,
  'Đã xóa': 3,
};

export enum SubscriptionPlan {
  TRIAL = 1,
  STANDARD = 2,
  PREMIUM = 3,
}

export enum StatusClubEnrollment {
  REGISTER= 1,
  APPROVED= 2,
  REJECTED = 3,
  CANCEL=4,
  TEACHING = 5
}
