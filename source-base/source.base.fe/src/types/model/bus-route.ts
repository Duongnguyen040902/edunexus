export interface BusRoute {
  id: number;
  name: string;
  description: string;
  status: number;
}

export interface CreateBusRoute {
  name?: string;
  description?: string;
  status?: number;
}
export interface ErrorResponseCreateBusRoute {
  Name?: string[];
  Description?: string[];
  Status?: number[];
  }

export interface ResponseBusRouteData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: BusRoute[];
}

export interface RequestBusRouteIndex {
  pageNumber: number | null;
  pageSize: number | null;
  status?: number | null;
  keyword?: string | null;
}
export interface BusStop {
  id: number;
  name: string;
  pickUpTime: string;
  returnTime: string;
  address: string;
  busRouteId: number;
  status: number;
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

export interface BusRouteDetail {
  id: number;
  name: string;
  description: string;
  status: number;
  busStops: BusStop[] | null;
  buses: Bus[] | null;
}