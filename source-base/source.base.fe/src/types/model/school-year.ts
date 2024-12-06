export interface RequestSchoolYearIndex {
    pageNumber: number;
    pageSize: number;
  }
  export interface RequestSchoolYear {
    schoolYearId:number;
  }

  export interface ResponseSchoolYearIndex {
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
        startDate: string;
        endDate: string;
        isActive: boolean;
        schoolId: number;
    }>;
}

export interface ResponseSchoolYearDetail {
    id: number;
    name: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
    schoolId: number;
}

export interface RequestCreateAndUpdateSchoolYear {
    id: number;
    name: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
}

export interface ErrorResponseSchoolYear{
    Name?: string[];
    StartDate?: string[];
    EndDate?: string[];
}