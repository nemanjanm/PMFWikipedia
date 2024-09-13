import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";
import { SubjectModel } from "../models/SubjectModel";

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

    async getSubject(Id: number){
        const realUrl = `${getUrl}/subject?Id=${Id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async addSubject(subject: SubjectModel){
		const response = await fetch(getUrl ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(subject)
        });

        const data = await response.json();
        return(data);
	}

    async addProgram(name: string){
        const realUrl = `${getUrl}/Program/?name=${name}`
		const response = await fetch(realUrl ,{
            method: "POST",
            headers: headers,
        });

        const data = await response.json();
        return(data);
	}
}

export const subjectService = new SubjectService();