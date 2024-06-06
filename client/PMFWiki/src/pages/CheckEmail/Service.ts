import { enviorment } from "../../enviorment";

const url = `${enviorment.serverUrl}Auth/ConfirmRegistration`
const headers = {
	"Content-Type": "application/json",
};

class CheckEmailService{

    async checkEmail(registrationToken: string){
        const response = await fetch(url ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(registrationToken)
        });

        const data = await response.json();
        return(data);
    }
}

export const checkEmailService = new CheckEmailService()