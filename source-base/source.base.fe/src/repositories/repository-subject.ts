import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestCreateSubjectInterface } from '@/types/model/subject.ts';

export class SubjectRepository extends BaseRepository {
  public async getAllSubject(success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.get(API_URL.SUBJECT.ALL, {}, success, error);
  }

  public async EditSubject(
    id: number,
    data: RequestCreateSubjectInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(API_URL.SUBJECT.EDIT(id), data, success, error);
  }

  public async createSubject(
    data: RequestCreateSubjectInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.SUBJECT.CREATE, data, success, error);
  }

  public async deleteSubject(id: number, success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) {
    return await this.delete(API_URL.SUBJECT.DELETE(id), {}, success, error);
  }
}
