export interface RequestListPupilInterface {
    pageNumber: number;
    pageSize: number;
    accountStatus?: number;
    searchKey?: string
};

export interface ResponseListPupilInterface {
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
            genderName: string;
            donorName: string;
            donorPhoneNumber: string;
            email: string;
            address: string;
            dateOfBirth: string;
            schoolId: number;
            accountStatusName: string;
            username: string;
            image: string;
        }
    ];
};

export interface RequestCreatePupilInterface {
    pupilId: number;
    id: number;
    firstName: string;
    lastName: string;
    gender: boolean;
    donorName: string;
    donorPhoneNumber: string;
    email: string;
    address: string;
    dateOfBirth: string;
    genderName?: string
    schoolId: number; 
    accountStatus: number;
    accountStatusName: string;
    image: string;
}


export interface RequestGetPupilDetailInterface {
    pupilId: number;
}

export interface ResponsePupilDetailInterface {
    id: number;
    firstName: string;
    lastName: string;
    gender: boolean;
    donorName: string;
    donorPhoneNumber: string;
    email: string;
    address: string;
    dateOfBirth: string;
    genderName?: string
    accountStatus: number;
    accountStatusName: string;
    image: string;
}

export interface ErrorResponsePupil {
    FirstName?: string[];
    LastName?: string[];
    Address?: string[];
    DonorName?: string[];
    DateOfBirth?: string[];
    DonorPhoneNumber?: string[];
    Email?: string[];
}

export interface RequestDeletePupil {
    ids: number[];
}