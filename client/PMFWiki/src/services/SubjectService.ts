import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";

const getUrl = `${enviorment.serverUrl}Subject`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};

class SubjectService{
    async getSubjects(programId: number){
        const realUrl = `${getUrl}?programId=${programId}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
        
    }
}

export const subjectService = new SubjectService();