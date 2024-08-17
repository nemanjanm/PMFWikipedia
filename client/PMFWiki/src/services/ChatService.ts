import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";

const getUrl = `${enviorment.serverUrl}Chat`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};

class ChatService{
    async getChats(){
        const realUrl = `${getUrl}?id=${storageService.getUserInfo()?.id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
        
    }

    async getMessages(chatId: number){
        const realUrl = `${getUrl}/Messages?id=${chatId}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }
}

export const chatService = new ChatService();