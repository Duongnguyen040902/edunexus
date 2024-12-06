export interface ResponseGetClassApplication {
  id: number;
  pupilId: number;
  firstName: string;
  lastName: string;
  donorName?: string;
  semesterId: number;
  title: string;
  description: string;
  applicationCategoryId: number;
  categoryName?: string;
  response?: string;
  status: number;
  statusName?: string;
  createDate: Date;
}

export interface RequestGetCategory {
  id: number;
  name: string;
}
export interface RequestResponseClassApplication {
  id : number;
  response : string;
  status : number;
}
export interface ErrorResponseClassApplication {
  Id?  : string[];
  Response? : string[];
  Status? : string[];
}

export interface ErrorCreateAndUpdateClassApplication {
  PupilId?: string[];
  SemesterId?: string[];
  Title?: string[];
  Description?: string[];
  ApplicationCategoryId?: string[];
}

export enum ApplicationStatus {
  PENDING = 1,
  APPROVED = 2,
  REJECTED = 3,
}

export enum ApplicationCategory {
  Default = 1,
}