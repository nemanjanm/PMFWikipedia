import "../../index.css"
import { Card } from 'primereact/card';
import "./Register.css"
import { InputText } from "primereact/inputtext";
import { useEffect, useState, useRef } from "react";
import { Dropdown } from 'primereact/dropdown';
import { programmes } from "../Programme";
import { Password } from 'primereact/password';
import { Button } from 'primereact/button';
import { UserRegister } from "../../models/UserRegister";
import { registerService } from "./Service";
import { Toast } from 'primereact/toast';
import { useNavigate } from "react-router-dom";
import { ClipLoader } from "react-spinners";

function Register(){

    const toast = useRef<Toast>(null);
    const navigate = useNavigate();
    const [name, setName] = useState<string>("");
    const [lastname, setLastname] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    const [isValidMail, setIsValidMail] = useState<boolean>(false);
    const [password, setPassword] = useState<string>("");
    const [isValidPassword, setIsValidPassword] = useState<boolean>(false);
    const [repeatedPassword, setRepeatedPassword] = useState<string>("");
    const [isValidRepeatedPassword, setIsValidRepeatedPassword] = useState<boolean>(false);
    const [program, setProgram] = useState<string>("");
    const [sentProgram, setSentProgram] = useState<string>("");
    const [submit, setSubmit] = useState<boolean>(true);
    const [loader, setLoader] = useState<boolean>(false);

    useEffect(() => {
        if(!isValidMail && !isValidRepeatedPassword && name !== "" && lastname !== "" && email !== ""  && repeatedPassword === password && repeatedPassword !== "" && program !== "")
            setSubmit(false)
        else
            setSubmit(true)
    },[name, lastname, email, password, repeatedPassword, program])

    function checkEmail(event: any){
        const tempEmail = event.target.value;
        setEmail(e => e = tempEmail);
        const regex = /@pmf\.kg\.ac\.rs$/;
        setIsValidMail(!regex.test(tempEmail));
    }

    function handleProgram(e: any){
        setProgram(e);
        setSentProgram(e.name);
    }
    function checkPassword(event: any){
        const tempPassword = event.target.value;
        setPassword(tempPassword);
        const regex = /^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{5,}$/;
        setIsValidPassword(!regex.test(tempPassword));        
    }

    async function submited()
    {   
        const user: UserRegister = {
            firstName: name,
            lastName: lastname,
            email: email,
            password: password,
            program: sentProgram
        };
        setLoader(true);
            const response = await registerService.addUser(user);
            setName("");
            setLastname("");
            setProgram("");
            setPassword("");
            setEmail("");
            setRepeatedPassword("");
        setLoader(false);
        if(!response.status){
            toast.current?.show({severity:'error', summary: 'Greška', detail:"Neispravni kredencijali", life: 3000});
        }
        else{
            toast.current?.show({severity:'success', summary: 'Uspešna registracija', detail:"Uspešna registracija", life: 5000});
            setTimeout(() => {
                navigate("/email");
            }, 3000);
        }
    }

    function checkPasses(event: any, sign: number){
        let tempPassword = "";
        let tempRealPassword = "";
        if(sign === 2)
        {
            tempPassword = event.target.value;
            setRepeatedPassword(r => r = tempPassword);

            if(tempPassword === password)
                setIsValidRepeatedPassword(false);
            else
                setIsValidRepeatedPassword(true);
        }
        else
        {
            tempRealPassword = event.target.value;
            
            if(tempRealPassword === repeatedPassword)
                setIsValidRepeatedPassword(false);
            else
                setIsValidRepeatedPassword(true);
        }
    }

    function checkBothPass(event: any){
        checkPassword(event);
        checkPasses(event, 1);
    }

    function checkBothRepeated(event: any){
        checkPasses(event, 2);
    }

    return (
        <div className="d-flex justify-content-center">
            {!loader && <div className="card" >
                <Card title="Registracija">
                    <div className="w-100 d-flex align-items-center justify-content-between">
                        <InputText className="w-100" value={name} placeholder="Ime" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setName(e.target.value)} />
                        <InputText className="w-100" value={lastname} placeholder="Prezime" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setLastname(e.target.value)} />
                    </div>
                    <InputText className="w-100 mt-1" value={email} placeholder="primer@pmf.kg.ac.rs" invalid={isValidMail}  onChange={(e: React.ChangeEvent<HTMLInputElement>) => checkEmail(e)} />
                    <Dropdown value={program} onChange={(e) => handleProgram(e.value)} options={programmes} optionLabel="name" placeholder="Izaberi studijski program" className="w-100 md:w-14rem mt-1" style={{textAlign: "left"}}/>
                    <label className="m-1" style={{color: "#a0a3a9", fontSize: "small", textAlign: "left", lineHeight: "1"}}>*Lozinka mora sadržati minimum 5 karaktera i to jedno veliko slovo, jedno malo slovo i jedan broj.</label>
                    <Password placeholder="Primer1" value={password} onChange={(e) => checkBothPass(e)} invalid={isValidPassword} feedback={false} tabIndex={1} toggleMask className="w-100 mt-1"/>
                    <Password placeholder="Primer1" value={repeatedPassword} onChange={(e) => checkBothRepeated(e)} invalid={isValidRepeatedPassword} disabled={isValidPassword} feedback={false} tabIndex={1} toggleMask className="w-100 mt-1"/>
                    <Button label="Submit" disabled={submit} onClick={() => submited()} icon="pi pi-check" className="mt-1" />
                </Card>
            </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
            <Toast ref={toast}></Toast>
        </div>
        
    )
}

export default Register;