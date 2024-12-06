import Title from "ant-design-vue/es/typography/Title";

export interface RequestGetListNotificationsInterface {
  classId: number;
}
export interface RequestGetNotificationDetailInterface {
  id: number;
}
export interface ResponseGetListNotificationsInterface {
  id: number;
  classId: number;
  title: string;
  descriptions: string;
  categoryId: number;
  schoolYearId: number;
  createdDate: Date;
  updateDate: Date;
}

export interface ResponseGetNotificationCategoryInterface {
  id: number;
  name: string;
}

export interface ResponseGetNotificationDetailInterface {
  id: number;
  classId: number;
  title: string;
  descriptions: string;
  categoryId: number;
  fileImage?: Blob[];
  notificationImages: [
    {
      id: number;
      url: string;
      notificationId: number;
    },
  ];
  createdDate: Date;
  updateDate: Date;
}

export interface RequestCreateNotificationInterface {
  classId: number;
  title: string;
  descriptions: string;
  categoryId: number;
  fileImage?: Blob[];
}

// export interface RequestUpdateNotificationInterface {
//   id: number;
//   classId: number;
//   title: string;
//   descriptions: string;
//   categoryId: number;
//   notificationImages?: { url: string }[] | null;
// }

export interface RequestDeleteNotificationInterface {
  id: number;
}

export interface ErrorResponseNotificationInterface{
  Title?: string[];
  CategoryId?: string[];
  Descriptions? : string[];
  FileImage?: Blob[];
}