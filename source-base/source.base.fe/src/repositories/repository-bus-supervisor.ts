import { RequestDeleteBusSupervisor, RequestGetBusSupervisorDetailInterface } from './../types/model/bus-supervisor';
import { API_URL } from '@/constants/api/endpoint';
import { BaseRepository } from '@/repositories/base-repository';
import { SuccessResponse, ErrorResponse } from '@/constants/api/responses';
import { RequestCreateBusSupervisorInterface, RequestListBusSupervisorInterface, UpdateProfileBusSupervisor } from '@/types/model/bus-supervisor';
import { RequestImportExcelInterface } from '@/types/model/teacher-account';

export class BusSupervisorRepository extends BaseRepository {
  public async getBusSupervisorProfile(
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void
  ) {
    return await this.get(`${API_URL.BUS_SUPERVISOR.GET}`, {}, success, error);
  }

  public async updateBusSupervisor(
    busSupervisorData: UpdateProfileBusSupervisor,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.BUS_SUPERVISOR.UPDATE}`, busSupervisorData, success, error, true);
  }
  public async getAllBusSupervisors(
    params: RequestListBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_SUPERVISOR.ALL}`, params, success, error, false, true);
  }

  public async createBusSupervisor(
    params: RequestCreateBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.BUS_SUPERVISOR.CREATE, params, success, error, true);
  }

  public async getBusSupervisorDetail(
    params: RequestGetBusSupervisorDetailInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.get(`${API_URL.BUS_SUPERVISOR.DETAIL}/${params.busSupervisorId}`, params, success, error, false, false);
  }

  public async updateBusSupervisorAccount(
    busSupervisorId: number,
    params: RequestCreateBusSupervisorInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.put(`${API_URL.BUS_SUPERVISOR.UPDATE_ACCOUNT}/${busSupervisorId}`, params, success, error, true);
  }
  
  public async deleteBusSupervisor(
    params: RequestDeleteBusSupervisor,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.delete(API_URL.BUS_SUPERVISOR.DELETE, params.ids, success, error);
  }

  public async importExcelBusSupervisor(
    file: RequestImportExcelInterface,
    success: (res: SuccessResponse) => void,
    error: (err: ErrorResponse) => void,
  ) {
    return await this.post(API_URL.BUS_SUPERVISOR.IMPORT_EXCEL, file, success, error, true);
  }
}