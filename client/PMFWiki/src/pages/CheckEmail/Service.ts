import { enviorment } from "../../enviorment";

const registrationUrl = `${enviorment.serverUrl}Auth/ConfirmRegistration`
const resetPasswordUrl = `${enviorment.serverUrl}Auth/ForgotPassword`
const headers = {
	"Content-Type": "application/json",
};

class CheckEmailService{

    async checkEmail(registrationToken: string){
        const response = await fetch(registrationUrl ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(registrationToken)
        });

        const data = await response.json();
        return(data);
    }
    async resetPasswordEmail(email: string){
        console.log(email);
        const response = await fetch(resetPasswordUrl ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(email)
        });

        const data = await response.json();
        return(data);
    }
}

export const checkEmailService = new CheckEmailService()