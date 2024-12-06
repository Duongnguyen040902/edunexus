export interface RequestGetSemesterInterface {
  schoolId: number;
}

export interface ResponseGetSemesterInterface {
  id: number;
  semesterName: string;
  semesterCode: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  schoolYearId: number;
  schoolYearName: string;
}
export interface  ListSemesterResponseInterface {
  id?: number,
  semesterName?: string,
  semesterCode?: string,
  startDate?: Date,
  endDate?: Date,
  isActive?: boolean,
  schoolYearId?: number,
  schoolYearName?: string,
  isHaveTimetable?: boolean
}

export interface  RequestListSemesterInterface {
  schoolId: number,
}


export interface RequestSemesterIndex {
  schoolYearId: number;
}

export interface RequestCreateAndUpdateSemester {
  id?: number;
  semesterName: string;
  semesterCode: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  schoolYearId: number;
}

// export interface ResponseSemesterIndex {
//   id: number;
//     semesterName: string;
//     semesterCode: string;
//     startDate: string;
//     endDate: string;
//     isActive: boolean;
//     schoolYearId: number;
// }

export interface ResponseSemesterDetail {
  id: number;
  semesterName: string;
  semesterCode: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
  schoolYearId: number;
}

export interface ErrorResponseSemester {
  SemesterName?: string[];
  SemesterCode?: string[];
  StartDate?: string[];
  EndDate?: string[];
}

