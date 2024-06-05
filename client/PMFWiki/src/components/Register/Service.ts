import { enviorment } from "../../enviorment";
import { UserRegister } from "../../models/UserRegister";

const url = `${enviorment.serverUrl}Auth/Register`
const headers = {
	"Content-Type": "application/json",
};

class RegisterService {
    
    async addUser(user: UserRegister){
        const response = await fetch(url ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(user)
        }).then(() => {
            console.log(response);
        })
    }
}

export const registerService = new RegisterService();