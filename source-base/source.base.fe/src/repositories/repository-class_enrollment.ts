import { API_URL } from '@/constants/api/endpoint';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { BaseRepository } from './base-repository';
import { RequestDeleteTeacherInterface, RequestGetTeacherIdInterface } from '@/types/model/class-enrollment';

export class ClassEnrollmentRepository extends BaseRepository {
  public async assignTeacherToClass(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.CLASS_ENROLLMENT.ASSIGN_TEACHER, data, success, error);
  }
  public async assignPupilToClass(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.post(API_URL.CLASS_ENROLLMENT.ASSIGN_PUPIL, data, success, error);
  }
  public async removeTeacherFromClass(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.delete(`${API_URL.CLASS_ENROLLMENT.REMOVE_TEACHER}/${data.value.classId}/${data.value.semesterId}/${data.value.teacherId}`, data, success, error);
  }
  public async getTeacherId(data: RequestGetTeacherIdInterface, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.CLASS_ENROLLMENT.GET_TEACHER_ID, data, success, error);
  }
  public async updateTeacherAssign(data: object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.put(API_URL.CLASS_ENROLLMENT.UPDATE_ASSIGN_TEACHER, data, success, error);
  }
  public async getListTeacherInClassAssign(data:object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS_ENROLLMENT.GET_TEACHER_SWAP}`, data, success, error,true);
  }
  public async swapTeacherInClassAssign(data:object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS_ENROLLMENT.SWAP_TEACHER}`, data, success, error,true);
  }
  public async getListMemberInClass(data:object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS_ENROLLMENT.GET_MEMBER_IN_CLASS}`, data, success, error,false,true);
  }
  public async getListMemberInNextSemester(data:object, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(`${API_URL.CLASS_ENROLLMENT.GET_MEMBER_IN_NEXT_SEMESTER}?nextSemesterId=${data}`, [], success, error);
  }
  public async removeMemberFromClass(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.delete(`${API_URL.CLASS_ENROLLMENT.REMOVE_MEMBER}/${id}`, [], success, error);
  }
  public async pupilsToGradute(data: number[], success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.put(API_URL.CLASS_ENROLLMENT.PUPILS_TO_GRADUATE, data, success, error);
  }
}