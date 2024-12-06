export interface ResponseGetClubDetailInterface {
  id: number;
  name: string;
  description: string;
  status: number;
  schoolId: number;
  schoolName: string;
  schoolYearId: number;
  schoolYearName: string;
  semesterId: number;
  semesterName: string;
  teacher: {
    id: number;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    gender: boolean;
    address: string;
    phoneNumber: string;
  };
  pupils: {
    id: number;
    firstName: string;
    lastName: string;
    gender: boolean;
    donorName: string;
    donorPhoneNumber: string;
    address: string | null;
    dateOfBirth: string | null;
    schoolId: number;
    schoolName: string;
    image: string;
  }[];
}
export interface ResponseGetClubInterface {
  id: number;
  name: string;
  description: string;
  status: number;
  schoolId: number;
}

export interface ResponseClubInterface {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: ResponseGetClubInterface[];
}

export interface RequestCreateClubInterface {
  name: string;
  description: string;
  status: number;
}

export interface ErrorResponseCreateClub {
  Name?: string[];
  Description?: string[];
  Status?: string[];
}

export interface RequestGetClubInterface {
  pageNumber?: number;
  pageSize?: number;
  status?: number | null;
  keyword?: string | null;
}
