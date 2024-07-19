import { Card } from "primereact/card";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import { LoginInfo } from "../models/LoginInfo";
import { storageService } from "../services/StorageService";
import "./ProfilePage.css"
import { enviorment } from "../enviorment";
import { FileUpload, FileUploadHandlerEvent, FileUploadUploadEvent } from 'primereact/fileupload';
import { getName } from "../models/Programme";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import { Toast } from "primereact/toast";
import { userService } from "./UserService";

function ProfilePage(){

    const navigate = useNavigate();
    const [id, setId] = useState<number>();
    const [user, setUser] = useState<LoginInfo | null>();
    const toast = useRef<Toast>(null);
    const params = useParams();
    useEffect(() => {
        if(params.id)
        {

        }
        else
            setUser(storageService.getUserInfo());
    },[id])
    

    async function onUpload(e: FileUploadHandlerEvent){
            console.log(e.files[0]);
            const response = await userService.changePhoto(e.files[0]);
            if(response.status)
            {
                navigate(0)
            }

    };
    
    return<>
    <div className="celina" style={{height: "fit-content"}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between">
            <SideBar></SideBar>
            <div className="d-flex flex-column" style={{width: "100%", height: "auto"}}>
                
                <h1 style={{fontSize: '5vw', textAlign: "center", width: "100%", height: "auto", fontWeight: "bold"}}>{user?.fullName}</h1>
                <div className="d-flex justify-content-around">
                    <div className="d-flex justify-content-center" style={{width: "100%", height: "auto"}}>
                        <div>
                        <img style={{maxWidth: "30vw", height: "auto", borderRadius: "10%"}} src={enviorment.port + user?.photoPath}></img>
                            {/* <Toast ref={toast}></Toast> */}
                            <FileUpload style={{textAlign: "center", height: "auto", marginTop: "1vw", color: "#374151"}} auto mode="basic" accept="image/*" chooseLabel="Izmeni sliku" customUpload maxFileSize={10000000} uploadHandler={onUpload} />
                        </div>
                    </div>
                    <div className="d-flex justify-content-center" style={{width: "100%", height: "auto"}}>
                        <div className="">
                            <h2>Interesovanja</h2>

                        </div>
                    </div>
                </div>
            </div>
            <div></div>
        </div>
    </div>
    </>
}

export default ProfilePage;