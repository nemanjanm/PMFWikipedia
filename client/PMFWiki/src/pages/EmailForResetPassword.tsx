import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { InputText } from "primereact/inputtext";
import { Toast } from "primereact/toast";
import { useRef, useState } from "react";
import { ClipLoader } from "react-spinners";
import { checkEmailService } from "./CheckEmail/Service";
import { useNavigate } from "react-router-dom";

function EmailForResetPassword(){

    const toast = useRef<Toast>(null);
    const [email, setEmail] = useState<string>("");
    const [loader, setLoader] = useState<boolean>(false);
    const [isValidMail, setIsValidMail] = useState<boolean>(false);
    const [submit, setSubmited] =  useState<boolean>(true);
    const navigate = useNavigate();

    function handleEmail(email: string)
    {
        const regexMail = /@pmf\.kg\.ac\.rs$/;
        setIsValidMail(!regexMail.test(email));
        setSubmited(!regexMail.test(email));
        setEmail(email);
    }

    async function submited(){
        setLoader(true);
        const response : any = await checkEmailService.resetPasswordEmail(email);

        if(response.status){
            navigate("/email");
        }
        else{
            setLoader(false);
            toast.current?.show({severity:'error', summary: 'Greška', detail:"Email je nepostojeći", life: 3000});
            setEmail("");
            setIsValidMail(true);

        }
    }
    return(
        <>
            <div className="d-flex justify-content-center">
                {!loader && <div className="card">
                <Card title="Reset Lozinke">
                    <InputText className="w-100 mt-1" value={email} placeholder="primer@pmf.kg.ac.rs" invalid={isValidMail}  onChange={(e) => handleEmail(e.target.value)} />
                    <label style={{fontStyle: "italic"}}>Unesite email na koji će vam stići dalje instrukcije kako biste izmenili lozinku</label>
                    <Button  label="Submit" onClick={() => submited()} icon="pi pi-check" className="mt-1" />
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

export default EmailForResetPassword;