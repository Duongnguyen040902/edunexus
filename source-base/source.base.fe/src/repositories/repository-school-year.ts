import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestCreateAndUpdateSchoolYear, RequestSchoolYearIndex } from '@/types/model/school-year';
export class SchoolYearRepository extends BaseRepository{
    public async getAllSchoolYear(params: RequestSchoolYearIndex, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.get(API_URL.SCHOOL_YEAR.INDEX, params, success, error, false, true);
    }
    public async getSchoolYearDetail(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.get(API_URL.SCHOOL_YEAR.DETAIL(id), {}, success, error);
    }
    public async createSchoolYear(params: RequestCreateAndUpdateSchoolYear, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.post(API_URL.SCHOOL_YEAR.CREATE, params, success, error);
    }
    public async updateSchoolYear(params: RequestCreateAndUpdateSchoolYear, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.put(API_URL.SCHOOL_YEAR.EDIT, params, success, error);
    }
    public async deleteSchoolYear(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
        return await this.delete(API_URL.SCHOOL_YEAR.DELETE(id), {}, success, error);
    }
}