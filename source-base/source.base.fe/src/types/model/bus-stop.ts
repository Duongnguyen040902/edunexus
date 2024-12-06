export interface ResponseBusStopData {
  pageNumber: number;
  pageSize: number;
  firstPage: string;
  lastPage: string;
  totalPages: number;
  totalRecords: number;
  nextPage: string | null;
  previousPage: string | null;
  data: BusStop[];
}

export interface RequestBusStopIndex {
  pageNumber: number | null;
  pageSize: number | null;
  status?: number | null;
  keyword?: string | null;
  busRouteId: number;
}

export interface BusStop {
  id: number;
  name: string;
  pickUpTime: string;
  returnTime: string;
  address: string;
  busRouteId: number;
  status: number;
  index: number;
}
export interface CreateBusStop {
  name?: string;
  pickUpTime?: string;
  returnTime?: string;
  address?: string;
  busRouteId?: number;
  status?: number;
}

export interface ErrorResponseCreateBusStop {
    Name?: string[];
    PickUpTime: string[];
    ReturnTime: string[];
    Address?: string[];
    BusRouteId?: string[];
    Status?: string[];
}
