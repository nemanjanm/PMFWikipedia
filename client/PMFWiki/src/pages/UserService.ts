import { storageService } from "../services/StorageService";
import { enviorment } from "../enviorment";

const getUrl = `${enviorment.serverUrl}`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};
class UserService{
    async getAllUsers(){
        const realUrl = `${getUrl}User/GetAllUsers`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
        
    }
}

export const userService = new UserService();