import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { Password } from "primereact/password";
import { Toast } from "primereact/toast";
import { useRef, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { ClipLoader } from "react-spinners";
import { ResetPasswordInfo } from "../models/ResetPasswordInfo";
import { checkEmailService } from "../services/CheckEmailService";

function PasswordChange(){

    const [submit, setSubmit] = useState<boolean>(true);
    const [loader, setLoader] = useState<boolean>(false);
    const [password, setPassword] = useState<string>("");
    const [isValidPassword, setIsValidPassword] = useState<boolean>(false);
    const [repeatedPassword, setRepeatedPassword] = useState<string>("");
    const [isValidRepeatedPassword, setIsValidRepeatedPassword] = useState<boolean>(false);
    const params = useParams();
    const token = params.resetToken;
    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    function checkPassword(event: any){
        const tempPassword = event.target.value;
        setPassword(tempPassword);
        const regex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{5,}$/;
        setIsValidPassword(!regex.test(tempPassword));        
    }

    function checkPasses(event: any, sign: number){
        let tempPassword = "";
        let tempRealPassword = "";
        if(sign === 2)
        {
            tempPassword = event.target.value;
            setRepeatedPassword(r => r = tempPassword);

            if(tempPassword === password && tempPassword!=="")
            {
                setSubmit(false);
                setIsValidRepeatedPassword(false);
            }
            else
            {
                setSubmit(true);
                setIsValidRepeatedPassword(true);
            }
            
        }
        else
        {
            tempRealPassword = event.target.value;
            
            if(tempRealPassword === repeatedPassword && tempRealPassword!=="")
            {
                setSubmit(false);
                setIsValidRepeatedPassword(false);
            }
            else
            {
                setSubmit(true);
                setIsValidRepeatedPassword(true);
            }
        }
    }

    function checkBothPass(event: any){
        checkPassword(event);
        checkPasses(event, 1);
    }

    function checkBothRepeated(event: any){
        checkPasses(event, 2);
    }
    async function submited()
    {   
        const info: ResetPasswordInfo = {
            token: token as string,
            password: password,
            repeatPassword: repeatedPassword
        };
        setLoader(true);
            const response = await checkEmailService.changePassword(info);            
        if(!response.status){
            setLoader(false);
            toast.current?.show({severity:'error', summary: 'Greška', detail:"Unesite ponovo email za izmenu lozinke", life: 3000});
            setTimeout(() => {
                navigate("/email/reset");
            }, 3000);
        }
        else{
            toast.current?.show({severity:'success', summary: 'Uspešna izmena', detail:"Uspešno ste izmenili lozinku", life: 3000});
            setTimeout(() => {
                navigate("/Prijava");
            }, 3000);
            
        }
    }

    return(
        <>
            <div className="d-flex justify-content-center">
                {!loader && <div className="card">
                <Card title="Reset Lozinke">
                    <Password placeholder="Primer1" value={password} onChange={(e) => checkBothPass(e)} invalid={isValidPassword} feedback={false} tabIndex={1} toggleMask className="w-100 mt-1"/>
                    <Password placeholder="Primer1" value={repeatedPassword} onChange={(e) => checkBothRepeated(e)} invalid={isValidRepeatedPassword} disabled={isValidPassword} feedback={false} tabIndex={1} toggleMask className="w-100 mt-1"/>
                    <Button label="Submit" disabled={submit} onClick={() => submited()} icon="pi pi-check" className="mt-1" />
                </Card>       
                </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
                <div >
                    <Toast ref={toast}></Toast>
                </div>
                </div>
        </>
    )
}

export default PasswordChange;