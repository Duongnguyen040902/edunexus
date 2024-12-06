export interface SchoolInfoResponseInterface {
  id: number;
  name: string;
  address?: string;
  province: string;
  district: string;
  ward: string;
  email?: string;
  phoneNumber?: string;
  principalName?: string;
  principalPhone?: string;
  websiteLink?: string;
  image?: string;
  imageFile?: Blob;
  standardCode: string;
  dateOfEstablishment?: Date;
  fax?: string;
}

export interface ErrorResponseUpdateSchoolInfo {
  Name?: string[];
  Address?: string[];
  PhoneNumber?: string[];
  Email?: string[];
  PrincipalName?: string[];
  PrincipalPhone?: string[];
  WebsiteLink?: string[];
  ImageFile?: string[];
  StandardCode?: string[];
  DateOfEstablishment?: string[];
  FAX?: string[];
}

export interface SchoolDashboardDTO {
  countActiveClass: number;
  countActiveClub: number;
  countActiveBusRoute: number;
  countActiveBus: number;
  countActivePupil: number;
  countActiveTeacher: number;
  countActiveSupervisor: number;
  totalActiveAccount: number;
  semesterName?: string;
  schoolYearName?: string;
}
