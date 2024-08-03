import { LoginInfo } from "../models/LoginInfo";
import { LoginResponse } from "../models/LoginResponse";

class StorageService{

    getToken(): string | null {
        return localStorage.getItem("token");
    }

    getConnId(): string | null {
        return localStorage.getItem("connectionId");
    }
    
    getUserInfo(): LoginInfo | null {
        let userInfo = localStorage.getItem("userInfo");
    
        if(userInfo != null){
          return JSON.parse(userInfo!) as LoginInfo;
        }
    
        return null;
    }

    setCredentials(info: LoginResponse){
        localStorage.setItem("token", info.token);
        localStorage.setItem("userInfo", JSON.stringify(info.user));
    }

    setConnId(id: string){
        localStorage.setItem("connectionId", id);
    }
    
    setUserInfo(userInfo: LoginInfo): void {
        localStorage.setItem("userInfo", JSON.stringify(userInfo));
    }
    deleteCredentials(): void {
        localStorage.removeItem("token");
        localStorage.removeItem("userInfo");
    }
}

export const storageService = new StorageService();