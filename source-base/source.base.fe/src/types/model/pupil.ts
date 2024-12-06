export interface ListPupilAssignRequestInterface {
  id: number;
  firstName?: string;
  lastName?: string;
  email?: string;
  gender?: boolean;
  genderName?: string;
  donorName?: string;
  donorPhoneNumber?: string;
  address?: string;
  dateOfBirth?: Date;
  schoolId: number;
  schoolName?: string;
  accountStatus?: number;
  accountStatusName?: string;
  username?: string;
  image?: string | null;
}

export interface RequestGetPupilAssignInterface {
  semesterId: number;
}
export interface RequestUpdateDataPupilInterface {
  id: number;
  firstName: string;
  lastName: string;
  gender: boolean;
  donorName: string;
  donorPhoneNumber: string;
  address: string;
  dateOfBirth: string;
  email: string;
  image: Blob;
}

export interface ErrorResponseUpdatePupil{
  FirstName?: string[];
  LastName?: string[];
  Email?: string[];
  DonorName: string[];
  DonorPhoneNumber?: string[];
  Image?: string[];
  DateOfBirth?: string[];
  Address?: string[];
}