import { ErrorResponse } from './../../constants/api/responses';
export interface ResponseGetListAttendance {
    name: string;
    date: Date;
    absentees: number;
    classize: number;
    semesterId?: number;
    session?: number;
    type?: number;
    entityId?: number;
}
export interface RequestGetListAttendance {
    entityId: number;
    session: number;
    type: number;
    semesterId: number;
    date: Date;
}

export interface RequestGetAttendanceRecord {
    entityId: number;
    session: number;
    type: number;
    semesterId: number;
    date: Date;
}

export interface ResponseGetAttendanceRecord {
    id: number;
    pupilId: number;
    pupilName: string;
    image: string;
    classId?: number;
    clubId?: number;
    busId?: number;
    isAttend?: boolean;
    attendanceSession: number;
    attendanceType: number;
    feedback?: string;
    createdDate: Date;
}

export interface ErrorResponseAttendance {
    pupilId?: string[];
    classId?: string[];
    clubId?: string[];
    busId?: string[];
    isAttend?: string[];
    attendanceSession?: string[];
    attendanceType?: string[];
    feedback?: string[];
    createdDate?: string[];
}


export interface PupilAttendance {
    pupilAttendanceMaterial: {
        className: string;
        clubName: string[];
        busName: string;
    };
    pupilViewAttendanceDTO: [{
        createDate: Date;
        type? :number;
        isAttendClass?: {
            classId?: number;
            isAttend?: boolean;
            feedback?: string;
        };
        isAttendClub?: [{
            clubId?: number;
            isAttend?: boolean;
            feedback?: string;
        }];
        isAttendBus?: {
            busId?: number;
            isAttend?: boolean;
            feedback?: string;
        };
    }];
}
