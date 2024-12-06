export interface PupilFeedback {
  pupilId?: number;
  pupilName?: string;
  donorName?: string;
  image?: string;
  semesterId: number;
  description?: string;
  status?: number;
  createdDate? : Date;
}

export interface ErrorFeedback {
  PupilId?: string[];
  Description?: string[];
  SemesterId?: string[];
  Status?: string[];
}

export interface FeedbackDetail {
  id: number;
  semester: {
    id: number;
    semesterName: string;
    semesterCode: string;
    startDate: string;
    endDate: string;
    isActive: boolean;
    schoolYearId: number;
    schoolYearName: string;
  };
  description: string;
  status: number;
  createdDate: string;
}