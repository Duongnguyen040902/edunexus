export interface RequestGetSubjectInterface {
  schoolId: number;
}

export interface ResponseGetSubjectInterface {
  id: number;
  name: string;
  code: string;
  schoolId: number;
}

export interface RequestCreateSubjectInterface {
  id: number;
  name: string;
  code: string;
  schoolId: number;
}

export interface ErrorResponseCreateSubjectInterface {
  Name?: string[];
  Code?: string[];
  SchoolId?: string[];
}
