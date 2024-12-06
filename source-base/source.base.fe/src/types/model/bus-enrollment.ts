export interface ResponseBusEnrollmentData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: BusEnrollment[];
}

export interface BusEnrollment {
  id: number;
  busId: number;
  pupilId: number;
  busSupervisorId: number | null;
  semesterId: number;
  pupilName: string;
  pupilCode: string;
  semesterName: string;
  academicYear: string;
  busName: string;
  busSupervisorName: string | null;
  busStopId: number;
  busStopName: string | null;
}

export interface RequestBusEnrollmentIndex {
  pageNumber: number | null;
  pageSize: number | null;
  busId?: number | null;
  semesterId?: number | null;
}

export interface CreateBusEnrollment {
  busId: number;
  pupilId: number | null;
  semesterId: number;
  busSupervisorId: number | null;
  busStopId: number | null;
}

export interface PupilDTOResponse {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  gender: boolean;
  genderName: string;
  donorName: string;
  donorPhoneNumber: string;
  address: string;
  dateOfBirth: string;
  schoolId: number;
  schoolName: string | null;
  accountStatus: number;
  accountStatusName: string;
  username: string;
  image: string; 
}

export interface BusSupervisorDTOResponse {
  id: number;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  address: string;
  gender: boolean;
  schoolId: number;
}
