import { enviorment } from "../enviorment";
import { PostInfo } from "../models/PostInfo";
import { UserLogin } from "../models/UserLogin";
import { storageService } from "./StorageService";

const url = `${enviorment.serverUrl}Post`
const headers = {
	"Content-Type": "application/json",
};

class PostService{
	async addPost(post: PostInfo){
		const response = await fetch(url ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(post)
        });

        const data = await response.json();
        return(data);
	}

	async getPosts(subjectId: number){
		const realUrl = `${url}?subjectId=${subjectId}`
		const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });

		const data = await response.json();
        return(data);
	}
}

export const postService = new PostService();