import { LoginInfo } from "./LoginInfo";

export interface LoginResponse {
    token: string,
    user: LoginInfo
}