import { AuthResponse } from './auth-response.model'

export interface ApiResponse {
  success : boolean;
  message : string;
  data: AuthResponse;

}
