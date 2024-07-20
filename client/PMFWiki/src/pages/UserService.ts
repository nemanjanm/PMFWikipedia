import { storageService } from "../services/StorageService";
import { enviorment } from "../enviorment";

const getUrl = `${enviorment.serverUrl}`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};
const headers2 = {
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

    async getUser(id: number){
        const realUrl = `${getUrl}User/GetUser?id=${id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async changePhoto(photo: any){

        const formData = new FormData();
        formData.append("photo", photo);
        const realUrl = `${getUrl}User/ChangePhoto`
        const response = await fetch(realUrl, {
            method: "Post",
            headers: headers2,
            body: formData
        });

        const data = await response.json();
        return(data);
    }
}

export const userService = new UserService();