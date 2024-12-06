export interface SuccessResponse {
  succeeded: boolean;
  data: unknown;
  message: string;
}

export interface ErrorResponse {
  message: string;
  code: number;
  responseCode: number;
  errors?: { [key: string]: string[] };
  data?: FileData;
}

export interface FileData {
  fileName: string; 
  fileContent: string; 
}


