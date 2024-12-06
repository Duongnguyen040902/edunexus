export interface ListTeacherAssignResponseInterface {
  id: number;
  firstName: string;
  lastName: string;
  username: string;
  gender: boolean;
  genderName: string;
  dateOfBirth: Date;
  phoneNumber: string;
  email: string;
  address: string;
  accountStatusName: string;
  accountStatus: number;
  subjectIds: number[];
  subjects: string[];
  image: string | null;
}

export interface RequestGetTeacherAssignInterface {
  semesterId?: number;
}
export interface RequestUpdateDataTeacherInterface {
    id: number;
    firstName: string;
    lastName: string;
    gender: boolean;
    address: string;
    dateOfBirth: string;
    email: string;
    phoneNumber: string;
    image?: Blob;
    listSubject: {
      name: string;
  };
  }

  export interface ErrorResponseUpdateTeacher{
    FirstName?: string[];
    LastName?: string[];
    Email?: string[];
    PhoneNumber?: string[];
    Image?: string[];
    DateOfBirth?: string[];
    Address?: string[];
  }