import { storageService } from "./StorageService";
import { enviorment } from "../enviorment";
import { CommentInfo } from "../models/CommentInfo";

const getUrl = `${enviorment.serverUrl}Comment`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
};

class CommentService{
    async getComments(postId: number){
        const realUrl = `${getUrl}?postId=${postId}`
        const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

        const data = await response.json();
        return(data);
    }

    async deleteComment(commentId: number){
        const realUrl = `${getUrl}Delete?commentId=${commentId}`
        const response = await fetch(realUrl, {
            method: "POST",
            headers: headers,
        });

        const data = await response.json();
        return(data);
    }

    async addComment(commentInfo: CommentInfo){
        const realUrl = `${getUrl}`
        const response = await fetch(realUrl, {
            method: "POST",
            headers: headers,
            body: JSON.stringify(commentInfo)
        });

        const data = await response.json();
        return(data);
    }
}
export const commentService = new CommentService();