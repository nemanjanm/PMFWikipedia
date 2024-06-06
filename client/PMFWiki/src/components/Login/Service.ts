import { enviorment } from "../../enviorment";
import { UserLogin } from "../../models/UserLogin";

const url = `${enviorment.serverUrl}Auth/Login`
const headers = {
	"Content-Type": "application/json",
};

class LoginService{
    async login(user: UserLogin){
        const response = await fetch(url ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(user)
        });

        const data = await response.json();
        return(data);
    }
}

export const loginService = new LoginService();