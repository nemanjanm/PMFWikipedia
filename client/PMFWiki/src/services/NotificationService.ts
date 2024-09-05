import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";
const getUrl = `${enviorment.serverUrl}Notification`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};

class NotificationService{
    async getNotifications(){
        const realUrl = `${getUrl}?id=${storageService.getUserInfo()?.id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async getAllNotifications(){
        const realUrl = `${getUrl}/all-notification?id=${storageService.getUserInfo()?.id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async setIsRead(nottId: number){
        const realUrl = `${getUrl}/read?nottId=${nottId}`
        const response = await fetch(realUrl ,{
            method: "POST",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }
}

export const notificationService = new NotificationService()