export interface RequestGetClassDetailInterface {
  classId: number;
  semesterId: number;
}

export interface ResponseGetClassDetailInterface {
  classId: number;
  className: string;
  semesterId: number;
  semesterName: string;
  schoolYearId: number;
  schoolYearName: string;
  schoolId: number;
  schoolName: string;
  status: number;
  homeroomTeacher: {
    id: number;
    firstName: string;
    lastName: string;
    dateOfBirth: string; // ISO 8601 format
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
    dateOfBirth: string | null; // ISO 8601 format or null
    schoolId: number;
    schoolName: string;
    image:string
  }[];
}


export interface ResponseGetAssignedClassInterface {
  Id: number;
  ClassName: string;
  SchoolId: number;
  SchoolName: string;
  Status: number;
}
export interface  AddNewClassRequestInterface {
  name : string,
  block: number,
}

export interface  updateClassRequestInterface {
  id: number,
  name : string,
  block: number,
}

export interface ErrorResponseAddNewClass {
  Name?: string[];

}
export interface  DeleteClassRequestInterface {
  id : number,
}

export interface  PupilClassesInterface {
  id: number,
  classId : number,
  className : string,
  semesterId : number,
  semesterName : string,
  schoolYearId : number,
  schoolYearName : string,
  isCurrent : boolean,
  startDate : Date,
  endDate : Date,
}