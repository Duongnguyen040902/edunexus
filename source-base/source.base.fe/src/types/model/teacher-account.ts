export interface RequestGetListTeachersInterface {
    pageNumber: number;
    pageSize: number;
    subjectId?: number;
    accountStatus?: number;
    searchKey?: string
  }
    
  export interface ResponseGetListTeachersInterface {
    pageNumber: number;
    pageSize: number;
    firstPage: string;
    lastPage: string;
    totalPages: number;
    totalRecords: number;
    nextPage: string;
    previousPage: string;
    data: [
      {
        id: number;
        firstName: string;
        lastName: string;
        username: string;
        genderName: string;
        dateOfBirth: string;
        phoneNumber: string;
        email: string;
        address: string;
        accountStatusName: string;
        subjects: string[];
        image?: string | null;
      }  
    ];
  };


  export interface RequestGetTeacherDetailInterface {
    teacherId: number;
  }
  
  export interface ResponseGetTeacherDetailInterface {
    id: number;
    firstName: string;
    lastName: string;
    username: string;
    gender: boolean;
    genderName: string;
    dateOfBirth: string;
    phoneNumber: string;
    email: string;
    address: string;
    accountStatusName: string;
    accountStatus: number;
    subjectIds: number[];
    subjects: string[];
    image: string;
  }

  export interface RequestCreateTeacherInterface {
    id: number;
    teacherId: number;
    firstName: string;
    lastName: string;
    gender: boolean;
    genderName?: string
    dateOfBirth: string;
    phoneNumber: string;
    email: string;
    address: string;
    subjectIds: number[];
    accountStatus: number;
    subjects?: string[];
    image?: string;
  }


  export interface ErrorResponseCreateTeacher {
    FirstName?: string[];
    LastName?: string[];
    Address?: string[];
    SubjectIds?: string[];
    DateOfBirth?: string[];
    PhoneNumber?: string[];
    Email?: string[];
  }

  export interface RequestDeleteTeacher {
    ids: number[];
  }
  

  export interface RequestImportExcelInterface {
    file: Blob;
  }