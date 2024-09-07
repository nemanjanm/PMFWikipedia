import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";

const getUrl = `${enviorment.serverUrl}Kolokvijum`
const headers = {
    "Authorization" : "Bearer "+ storageService.getToken()
};

class KolokvijumService{
    async getKolokvijum(id: number){
        const realUrl = `${getUrl}?subjectId=${id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });
        const data = await response.json();
        return(data);
    }
    async addKolokvijum(title: any, file : any, authorId: any, subjectId: any){
        const formData = new FormData();
        formData.append("title", title);
        formData.append("file", file);
        formData.append("authorId", authorId);
        formData.append("subjectId", subjectId);
        const response = await fetch(getUrl, {
            method: "Post",
            headers: headers,
            body: formData
        });

        const data = await response.json();
        return(data);
    }

    async addResenje(kolokvijumId: any, file : any, authorId: any, subjectId: any){
        const realUrl = `${getUrl}/resenje`
        const formData = new FormData();
        formData.append("kolokvijumId", kolokvijumId);
        formData.append("file", file);
        formData.append("authorId", authorId);
        formData.append("subjectId", subjectId);
        const response = await fetch(realUrl, {
            method: "Post",
            headers: headers,
            body: formData
        });

        const data = await response.json();
        return(data);
    }

    async deleteKolokvijum(id: any){
        const realUrl = `${getUrl}/delete?id=${id}`
        const response = await fetch(realUrl, {
            method: "Post",
            headers: headers,
        });

        const data = await response.json();
        return(data);
    }

    async deleteResenje(id: any){
        const realUrl = `${getUrl}/delete/resenje?id=${id}`
        const response = await fetch(realUrl, {
            method: "Post",
            headers: headers,
        });

        const data = await response.json();
        return(data);
    }
}

export const kolokvijumService = new KolokvijumService();