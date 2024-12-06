 export interface UpdateProfileBusSupervisor {
    id: number;
    firstName?: string;
    lastName?: string;
    email?: string;
    image?: Blob;
    phoneNumber?: string;
    address?: string;
    gender?: boolean;
  }

  export interface ErrorResponseUpdateBusSupervisor {
    FirstName?: string[];
    LastName?: string[];
    PhoneNumber?: string[];
    Address?: string[];
    Image?: string[];
  }

  export interface RequestListBusSupervisorInterface {
    pageNumber: number;
    pageSize: number;
    accountStatus?: number;
    searchKey?: string
};

export interface RequestCreateBusSupervisorInterface {
  busSupervisorId: number;
  id: number;
  firstName: string;
  lastName: string;
  gender: boolean;
  phoneNumber: string;
  email: string;
  address: string;
  genderName?: string
  schoolId: number; 
  accountStatus: number;
  accountStatusName: string;
  image: string;
}
export interface ResponseListBusSupervisorInterface {
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
          email: string;
          gender: boolean;
          genderName: string;
          phoneNumber: string;        
          address: string;
          schoolId: number;
          accountStatusName: string;
          username: string;
          image: string;
      }
  ];
};

export interface ResponseBusSupervisorDetailInterface {
  id: number;
  firstName: string;
  lastName: string;
  gender: boolean;
  phoneNumber: string;
  email: string;
  address: string;
  genderName?: string
  accountStatus: number;
  accountStatusName: string;
  image: string;
}

export interface ErrorResponseBusSupervisor {
  FirstName?: string[];
  LastName?: string[];
  Address?: string[];
  PhoneNumber?: string[];
  Email?: string[];
}

export interface RequestDeleteBusSupervisor {
  ids: number[];
}
export interface RequestGetBusSupervisorDetailInterface {
  busSupervisorId: number;
}