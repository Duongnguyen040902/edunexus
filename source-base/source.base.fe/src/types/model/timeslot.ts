export interface RequestGetTimeSlotInterface {
  schoolId: number;
}
export interface ResponseGetTimeSlotInterface {
  id: number;
  name: string;
  startTime: string;
  endTime: string;
  schoolId: number;
  isActive: boolean;
}

export interface RequestGetAllTimeSlotInterface {
  pageNumber?: number;
  pageSize?: number;
  isActive?: boolean;
  keyword?: string;
}
export interface RequestCreateTimeSlotInterface {
  name: string;
  startTime: string;
  endTime: string;
  isActive: boolean;
}
export interface ErrorResponseCreateTimeSlot {
  Name?: string[];
  StartTime?: string[];
  EndTime?: string[];
  IsActive?: string[];
}