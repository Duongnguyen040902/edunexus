export interface ResponseGetBus {
    id: number;
    name: string;
    driverName?: string;
    driverPhone?: string;
    licensePlate?: string;
    seatNumber: number;
    busRouteId: number;
    status: number;
}

export interface ResponseGetBusDetail {
    id: number;
    name?: string;
    driverName?: string;
    driverPhone?: string;
    licensePlate?: string;
    seatNumber: number;
    busRouteId: number;
    status: number;
    busRouteName?: string;
    busSupervisor?: {
        id: number;
        firstName: string;
        lastName: string;
        dateOfBirth: string;
        gender: boolean;
        address: string;
        phoneNumber: string;
    };
    pupil?: {
        id: number;
        pupilId: number;
        pupilName?: string;
        image?: string;
        donorName?: string;
        donorPhoneNumber?: string;
        address?: string;
        dateOfBirth?: Date;
        gender?: boolean;
        busStopId?: number;
        busStopAddress?: string;
      }[];
  busStops?: {
    id: number;
    name: string;
    estimatedTime: string; // Use string for TimeSpan
    address: string;
    busRouteId: number;
    status: number;
  }[];
}

export interface ViewBusEnrollDetailDTO {
    id: number;
    busId: number;
    busName: string;
    semesterId: number;
    semesterName: string;
    schoolYearId: number;
    schoolYearName: string;
    busStopId?: number;
    busStopName?: string;
    pickUpTime?: string;
    returnTime?: string;
    startDate?: Date;
    endDate?: Date;
    isCurrent: boolean;
}

export interface ResponseBusData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: Bus[];
}

export interface RequestBusIndex {
  pageNumber: number | null;
  pageSize: number | null;
  status?: number | null;
  keyword?: string | null;
  busRouteId?: number | null;
}

export interface Bus {
  id: number;
  name: string;
  driverName: string;
  driverPhone: string;
  licensePlate: string;
  seatNumber: number;
  busRouteId: number;
  status: number;
}

export interface CreateBus {
  name?: string;
  driverName?: string;
  driverPhone?: string;
  licensePlate?: string;
  seatNumber?: number;
  status?: number;
  busRouteId?: number;
}

export interface ErrorResponseCreateBus {
  Name?: string[];
  DriverName?: string[];
  DriverPhone?: string[];
  LicensePlate?: string[];
  SeatNumber?: string[];
  Status?: string[];
}

