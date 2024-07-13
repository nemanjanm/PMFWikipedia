import { useParams } from "react-router-dom";
import "../index.css"
import { useState, useRef, useEffect } from "react";
import { ClipLoader } from "react-spinners";
import { checkEmailService } from "./CheckEmail/Service";
import { useNavigate } from "react-router-dom";
import { Toast } from 'primereact/toast';

function ConfirmRegisration(){

    const navigate = useNavigate();
    const toast = useRef<Toast>(null);
    const [confirmed, setConfirmed] = useState<boolean>(false)
    const params = useParams();
    const token = params.registrationToken;
    useEffect(() => {
        async function checkEmail(){
            const response = await checkEmailService.checkEmail(token as string)
            setConfirmed(true);
    
            if(!response.status){
                toast.current?.show({severity:'error', summary: 'Greška', detail:response.error, life: 3000});
                setTimeout(() => {
                    navigate("/Registracija");
                }, 3000);
            }
            else{
                toast.current?.show({severity:'success', summary: 'Uspešna registracija', detail:response.error, life: 4000});
                setTimeout(() => {
                    navigate("/Prijava");
                }, 3000);
            }
        }
        checkEmail();
    }, [])

    
    
    return(<>
        <div style={{textAlign: "center"}}>
            <p>Validacija e-mail-a</p>
            <ClipLoader color="#111827" loading={!confirmed} size={150}></ClipLoader>
            <Toast ref={toast}></Toast>
        </div>
        </>)
}   

export default ConfirmRegisration;