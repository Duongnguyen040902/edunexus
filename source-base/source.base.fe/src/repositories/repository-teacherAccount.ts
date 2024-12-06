import { RequestImportExcelInterface } from './../types/model/teacher-account';
//src/repositories/repository-teacher-account.ts
import { API_URL } from '@/constants/api/endpoint';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { BaseRepository } from '@/repositories/base-repository';
import {
    RequestCreateTeacherInterface,
    RequestDeleteTeacher,
    RequestGetListTeachersInterface,
    RequestGetTeacherDetailInterface,
} from '@/types/model/teacher-account';

export class TeacherRepository extends BaseRepository {
    public async getAllTeachers(
        params: RequestGetListTeachersInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(API_URL.TEACHER.ALL, params, success, error, false, true);
    }

    public async createTeacher(
        teacherData: RequestCreateTeacherInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.post(API_URL.TEACHER.CREATE, teacherData, success, error, true);
    }

    public async updateTeacher(
        teacherId: number,
        teacherData: RequestCreateTeacherInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.put(`${API_URL.TEACHER.UPDATE}/${teacherData.teacherId}`, teacherData, success, error, true);
    }

    public async getTeacherDetail(
        params: RequestGetTeacherDetailInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(API_URL.TEACHER.DETAIL, params, success, error, false, false);
    }

    public async deleteTeacher(
        params: RequestDeleteTeacher,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.delete(API_URL.TEACHER.DELETE, params.ids, success, error);
    }

    public async importExcelTeacher(
        file: RequestImportExcelInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.post(API_URL.TEACHER.IMPORT_EXCEL, file, success, error, true);
    }
}
