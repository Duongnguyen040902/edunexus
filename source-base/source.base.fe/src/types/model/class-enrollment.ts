export interface ResponseTeacherSwapInterface {
  classEnrollmentId?: number; 
  semesterId?: number;    
  className?: string;    
  teacherName?: string;       
  image?: string | null;      
  userName?: string;         
}

export interface RequestAssignTeacherInterface {
  classId?: number;
  teacherId?: number;
  semesterId?: number;
}

export interface RequestUpdateAssignTeacherInterface {
  classEnrollmentId: number;
  classId?: number;
  teacherId?: number;
  semesterId?: number;
}
export interface RequestDeleteTeacherInterface {
  classId: number;
  semesterId: number;
  teacherId: number;
}

export interface RequestGetTeacherIdInterface {
  classId: number;
  semesterId: number;
}

export interface RequestGetTeacherSwapInterface {
  semesterId: number;
  ceTeacherId1: number;
}

export interface RequestSwapTeacherInterface {
  ceTeacherId1: number;
  ceTeacherId2: number;
}

export interface RequestAssignPupilInterface {
  classId?: number;
  pupilId?: number;
  semesterId?: number;
}

export interface RequestGetClassEnrollment{
  pageNumber: number | null;
  pageSize: number | null;
  keyword: string | null;
  semesterId?: number | null;
  classId?: number | null;
}
export interface MemberInClassDTO {
  id: number;
  classId: number;
  className: string | null;
  block: number | null;
  teacherName?: string | null;
  teacherId?: number | null;
  teacherCode?: string | null;
  teacherImage?: string | null;
  pupilName?: string | null;
  pupilId?: number | null;
  pupilCode?: string | null;
  pupilImage?: string | null;
  semesterId: number;
}
export interface ResponseClassEnrollmentData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: MemberInClassDTO[];
}