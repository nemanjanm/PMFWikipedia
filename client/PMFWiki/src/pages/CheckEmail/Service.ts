import { enviorment } from "../../enviorment";
import { ResetPasswordInfo } from "../../models/ResetPasswordInfo";

const registrationUrl = `${enviorment.serverUrl}Auth/ConfirmRegistration`
const resetPasswordUrl = `${enviorment.serverUrl}Auth/ForgotPassword`
const changePasswordUrl = `${enviorment.serverUrl}Auth/ChangePassword`
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
    async changePassword(info: ResetPasswordInfo){
        const response = await fetch(changePasswordUrl ,{
            method: "POST",
            headers: headers,
            body: JSON.stringify(info)
        });

        const data = await response.json();
        return(data);
    }
}

export const checkEmailService = new CheckEmailService()