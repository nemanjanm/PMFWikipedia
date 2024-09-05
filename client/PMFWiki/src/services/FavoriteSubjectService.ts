import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";
import { FavoriteSubjectInfo } from "../models/FavoriteSubjectInfo";

const getUrl = `${enviorment.serverUrl}FavoriteSubject/GetFavoriteSubjects`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};
class FavoriteSubjectService{
    async getFavoriteSubjects(id: number){
        const realUrl = `${getUrl}?id=${id}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async removeFavoriteSubject(favoriteSubject: FavoriteSubjectInfo){
        const realUrl = `${enviorment.serverUrl}FavoriteSubject`
        const response = await fetch(realUrl, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(favoriteSubject)
        });

        const data = await response.json();
        return(data);
    }

    async addToFavorites(favoriteSubject: FavoriteSubjectInfo){
        const realUrl = `${enviorment.serverUrl}FavoriteSubject/Add`
        const response = await fetch(realUrl, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(favoriteSubject)
        });

        const data = await response.json();
        return(data);
    }
}

export const favoriteSubjectService = new FavoriteSubjectService();