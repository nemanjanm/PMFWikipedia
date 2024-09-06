import { enviorment } from "../enviorment";
import { PostInfo } from "../models/PostInfo";
import { UserLogin } from "../models/UserLogin";
import { storageService } from "./StorageService";

const url = `${enviorment.serverUrl}Post`
const headers = {
	"Content-Type": "application/json",
    "Authorization" : "Bearer "+ storageService.getToken()
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

    async editPost(post: PostInfo){
        const realUrl = `${url}/Edit`
		const response = await fetch(realUrl ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(post)
        });

        const data = await response.json();
        return(data);
	}

    async deletePost(postId: number){
        const realUrl = `${url}/Delete?postId=${postId}`
		const response = await fetch(realUrl ,{
            method: "POST",
            headers: headers
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

    async getPost(postId: number){
		const realUrl = `${url}/single?postId=${postId}`
		const response = await fetch(realUrl, {
            method: "GET",
            headers: headers
        });
		const data = await response.json();
        return(data);
	}
}

export const postService = new PostService();