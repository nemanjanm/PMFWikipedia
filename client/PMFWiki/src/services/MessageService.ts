import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";
import { ChatIdModel } from "../models/ChatIdModel";

const getUrl = `${enviorment.serverUrl}Message`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};

class MessageService{
    async markAsRead(chatId : ChatIdModel){
        const realUrl = `${getUrl}/Read`
        const response = await fetch(realUrl, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(chatId)
        });
        
        const data = await response.json();
        return(data);
    }
}

export const messageService = new MessageService();