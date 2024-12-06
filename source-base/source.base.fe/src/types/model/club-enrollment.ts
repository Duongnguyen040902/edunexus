export interface ResponseClubEnrollmentData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: ClubEnrollment[];
}

export interface ClubEnrollment {
  id: number;
  clubId: number;
  pupilId: number | null;
  teacherId: number | null;
  semesterId: number;
  pupilName: string | null;
  pupilUsername: string | null;
  teacherName: string | null;
  teacherUsername: string | null;
  semesterName: string;
  status: number;
}

export interface RequestClubEnrollmentIndex {
  pageNumber: number | null;
  pageSize: number | null;
  keyword: string | null;
  clubId?: number | null;
  semesterId?: number | null;
}

export interface CreateClubEnrollment {
  clubId: number;
  pupilId: number | null;
  semesterId: number;
  teacherId: number | null;
  status: number;
}

export interface UpdateClubEnrollment {
  id: number;
  clubId: number;
  pupilId: number | null;
  semesterId: number;
  teacherId: number | null;
  status: number;
} 
export interface PupilDTOResponse {
  id: number;
  firstName: string;
  lastName: string;
  userName: string;
  gender: boolean;
  donorName: string;
  donorPhoneNumber: string;
  address: string;
  dateOfBirth: string;
  schoolId: number;
  schoolName: string | null;
  image: string;
}

export interface TeacherDTOResponse {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  gender: boolean;
  schoolId: number;
}
