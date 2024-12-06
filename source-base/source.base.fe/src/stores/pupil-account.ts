import { ErrorResponse, SuccessResponse } from '@/constants/api/responses';
import {
    RequestCreatePupilInterface,
    RequestGetPupilDetailInterface,
    RequestListPupilInterface,
    ResponseListPupilInterface,
    ResponsePupilDetailInterface
} from '@/types/model/pupil-account';
import { PupilRepository } from '@/repositories/repository-pupilAccount';
import RepositoryFactory from '@/repositories/repository';
import { defineStore } from 'pinia';
import { reactive } from 'vue';
import { RequestDeletePupil } from '@/types/model/pupil-account';
import { RequestImportExcelInterface } from '@/types/model/teacher-account';



export const usePupilAccountStore = defineStore('pupilAccount', () => {
    const PupilAccountFactory = RepositoryFactory.create('pupilAccount') as PupilRepository;
    const requestDeletePupil = reactive<RequestDeletePupil>({
        ids: [],
    });
    const requestImportExcelPupil = reactive<RequestImportExcelInterface>({
        file: new Blob(),
    });

    const pupils = reactive<{ value: ResponseListPupilInterface | null }>({ value: null });
    const pupilDetail = reactive<{ value: ResponsePupilDetailInterface | null }>({ value: null });

    const getListPupilAccount = async (
        params: RequestListPupilInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void
    ) => {
        PupilAccountFactory.getAllPupils(
            params,
            (res) => {
                console.log('API Response-store:', res);
                const successResponse: SuccessResponse = {
                    data: res.data as ResponseListPupilInterface,
                    message: res.message,
                    succeeded: res.succeeded
                };
                return success(successResponse);
            },
            (err) => {
                error(err);
            }
        );
    };


    const createPupil = async (
        params: RequestCreatePupilInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void) => {
        await PupilAccountFactory.createPupil(
            params,
            res => {
                success(res);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    };

    const getPupilDetail = async (
        params: RequestGetPupilDetailInterface,
        success: (res: ResponsePupilDetailInterface) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        console.log('params', params);
        await PupilAccountFactory.getPupilDetail(
            params,
            res => {
                console.log('res-store', res);
                res.data as ResponsePupilDetailInterface;
                return success(res.data as ResponsePupilDetailInterface);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    };

    const updatePupil = async (
        pupilId: number,
        params: RequestCreatePupilInterface,
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        console.log('params-store', params);

        await PupilAccountFactory.updatePupil(
            pupilId,
            params,
            res => {
                success(res);
            },
            err => {
                console.log('error', err);
                error(err);
            }
        );
    };

    const deletePupils = async (success: (res: SuccessResponse) => void, error: (err: ErrorResponse) => void) => {
        await PupilAccountFactory.deletePupil(
            requestDeletePupil,
            res => {
                requestDeletePupil.ids = [];
                success(res);
            },
            err => {
                console.log(err);
                error(err);
            },
        );
    };

    const importExcelPupilAccount = async (
        success: (res: SuccessResponse) => void,
        error: (err: ErrorResponse) => void,
    ) => {
        await PupilAccountFactory.importExcelPupil(
            requestImportExcelPupil,
            res => {
                success(res);
            },
            err => {
                if (err.data?.fileName && err.data?.fileContent) {
                    const binaryContent = atob(err.data?.fileContent);
                    const uint8Array = new Uint8Array([...binaryContent].map(char => char.charCodeAt(0)));
                    const decoder = new TextDecoder('utf-8');
                    const decodedContent = decoder.decode(uint8Array);
                    const blob = new Blob([decodedContent], { type: 'text/plain;charset=utf-8' });
                    const link = document.createElement('a');
                    link.href = URL.createObjectURL(blob);
                    link.download = err.data?.fileName;
                    link.click();
                }
                const errorResponse: ErrorResponse = {
                    ...err,
                    errors: err.errors,
                };
                return error(errorResponse);
            },
        );
    };
    return {
        requestDeletePupil,
        requestImportExcelPupil,
        deletePupils,
        getListPupilAccount,
        createPupil,
        getPupilDetail,
        updatePupil,
        importExcelPupilAccount,
    }
});
