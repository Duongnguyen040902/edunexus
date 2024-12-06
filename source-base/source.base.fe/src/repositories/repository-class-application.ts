import { RequestResponseClassApplication, ResponseGetClassApplication } from './../types/model/class-application';
import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';

export class ClassApplicationRepository extends BaseRepository {
    public async getClassApplicationList(
        params: { classId: number; semesterId: number; categoryId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.CLASS_APPLICATION.GET_LIST}`, params, success, error);
    }
    public async getCategory( success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.get(`${API_URL.CLASS_APPLICATION.CATEGORY}`, {}, success, error);
    } 
    public async responseClassApplication(
        params: RequestResponseClassApplication ,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.put(`${API_URL.CLASS_APPLICATION.RESPONSE}`, params, success, error);
    }
    public async getPupilApplication(
        params: {  semesterId: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.get(`${API_URL.CLASS_APPLICATION.GET_LIST_BY_PUPIL}`, params, success, error);
    }
    public async createClassApplication(
        data: ResponseGetClassApplication,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.post(`${API_URL.CLASS_APPLICATION.CREATE}`, data, success, error);
    }

    public async updateClassApplication(
        data: ResponseGetClassApplication,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.put(`${API_URL.CLASS_APPLICATION.UPDATE}`, data, success, error);
    }

    public async deleteClassApplication(
        params: { id: number },
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) {
        return await this.delete(`${API_URL.CLASS_APPLICATION.DELETE}?id=${params.id}`, params, success, error);
    }
}
