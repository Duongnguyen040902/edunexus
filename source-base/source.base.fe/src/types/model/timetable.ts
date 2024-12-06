export interface RequestGetTimetableInterface {
  semesterId: number;
  classId: number;
}

export interface ResponseGetTimetableInterface {
  classId: number;
  className: string;
  semesterId: number;
  semesterName: string;
  timeSlotId: number;
  timeSlotName: string;
  subjectId: number;
  subjectName: string;
  dayOfWeek: number;
}

export interface RequestCreateAndUpdateTimetableInterface {
  classId: number;
  semesterId: number;
  timeSlotId: number;
  subjectId: number;
  dayOfWeek: number;
}
