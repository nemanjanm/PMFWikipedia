import "../../index.css"
import { Card } from 'primereact/card';
import "./Register.css"
import { InputText } from "primereact/inputtext";
import { useState } from "react";
import { Dropdown } from 'primereact/dropdown';

function Register(){

    const programmes = [
        {name: "Informatika"},
        {name: "Matematika"},
        {name: "Biologija"},
        {name: "Ekologija"},
        {name: "Fizika"},
        {name: "Hemija"},
        {name: "Psigologija"}
    ]
    const [name, setName] = useState<string>("");
    const [surname, setSurname] = useState<string>("");
    const [email, setEmail] = useState<string>("");
    //const [password, setPassword] = useState<string>("");
    const [program, setProgram] = useState<string>("");

    return (
        <div className="d-flex justify-content-center">
            <div className="card">
                <Card title="Registracija">
                    <div className="w-100 d-flex align-items-center justify-content-between">
                        <InputText className="w-100" value={name} placeholder="Ime" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setName(e.target.value)} />
                        <InputText className="w-100" value={surname} placeholder="Prezime" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSurname(e.target.value)} />
                    </div>
                    <InputText className="w-100" value={email} placeholder="Email" onChange={(e: React.ChangeEvent<HTMLInputElement>) => setEmail(e.target.value)} />
                        <Dropdown value={program} onChange={(e) => setProgram(e.value)} options={programmes} optionLabel="name" showClear placeholder="Studijski program" className="w-100 md:w-14rem" style={{textAlign: "left"}}/>
                </Card>
            </div>
        </div>
    )
}

export default Register;