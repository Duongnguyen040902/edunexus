export interface RequestGetClubIndex {
  pageNumber: number;
  pageSize: number;
}

export interface ResponseGetClubIndex {
  pageNumber: number;
  pageSize: number;
  firstPage: number;
  lastPage: number;
  totalPages: number;
  totalRecords: number;
  nextPage: number | null;
  previousPage: number | null;
  data: Array<{
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
    teacher?: {
      id: number;
      firstName: string;
      lastName: string;
      dateOfBirth: string;
      gender: boolean;
      address: string;
      phoneNumber: string;
    };
    pupil: {
      id: number;
      firstName: string;
      lastName: string;
      dateOfBirth: string;
    };
  }>;
}

export interface RequestGetClubEnrollment {
  semesterId: number;
}

export interface ResponseGetClubEnrollment {
  data: Array<{
    id: number;
    clubId: number;
    pupilId: number;
    status: number;
    semesterId: number;
    clubName: string;
    clubDescription: string;
    semesterName: string;
    teacher?: {
        id: number;
        firstName: string;
        lastName: string;
        dateOfBirth: string;
        gender: boolean;
        address: string;
        phoneNumber: string;
      };
  }>;
}

export interface RequestCreateAndUpdateClubEnrollment {
  clubId: number;
  status: number;
}

export interface ReponseGetNextSemester {
  id: number;
  semesterName: string;
  semesterCode: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  schoolYearId: number;
}

export interface ReponseGetSemesters {
  data: Array<{
    id: number;
    semesterName: string;
    semesterCode: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
    schoolYearId: number;
    schoolYearName: string;
  }>;
}
