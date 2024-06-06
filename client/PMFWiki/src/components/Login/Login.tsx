import "../../index.css";
import "./Login.css"
import { UserLogin } from "../../models/UserLogin";
import { useState, useRef } from "react";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Button } from "primereact/button";
import { ClipLoader } from "react-spinners";
import { Toast } from 'primereact/toast';
import { loginService } from "./Service";


function Login(){
    const toast = useRef<Toast>(null);
    const [password, setPassword] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [submit, setSubmit] = useState<boolean>(true);
    const [loader, setLoader] = useState<boolean>(false);
    const [isValidMail, setIsValidMail] = useState<boolean>(false);
    const [isValidPassword, setIsValidPassword] = useState<boolean>(false);

    function handleEmail(email: string)
    {
        const regexMail = /@pmf\.kg\.ac\.rs$/;
        setIsValidMail(!regexMail.test(email));
        setEmail(email);
    }

    function handlePassword(password: string)
    {
        const regexPassword = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{5,}$/;
        setIsValidPassword(!regexPassword.test(password)); 
        setPassword(password);
    }
        
    async function submited(){
        const user: UserLogin = {
            email: email,
            password: password
        }

        if(!isValidMail && !isValidPassword){
            setLoader(true)
            const response = await loginService.login(user);
            setLoader(false)
            
            if(response.status){

            }
            else{
                setIsValidMail(true);
                setIsValidPassword(true);
                console.log(response.error)
                toast.current?.show({severity:'error', summary: 'Greška', detail: "Pogrešni kredencijali", life: 3000});
            }
        }
    }
    return(
        <>
            <div className="d-flex justify-content-center">
                {!loader && <div className="card">
                <Card title="Prijava">
                    <InputText className="w-100 mt-1" value={email} placeholder="primer@pmf.kg.ac.rs" invalid={isValidMail}  onChange={(e) => handleEmail(e.target.value)} />
                    <Password placeholder="Primer1" value={password} onChange={(e) => handlePassword(e.target.value)} invalid={isValidPassword} feedback={false} tabIndex={1} toggleMask className="w-100 mt-1"/>
                    <Button label="Submit" onClick={() => submited()} icon="pi pi-check" className="mt-1" />
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

export default Login;