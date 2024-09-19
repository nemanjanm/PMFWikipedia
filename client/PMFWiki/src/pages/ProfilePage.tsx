import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import { LoginInfo } from "../models/LoginInfo";
import { storageService } from "../services/StorageService";
import "./ProfilePage.css"
import { enviorment } from "../enviorment";
import { FileUpload, FileUploadHandlerEvent } from 'primereact/fileupload';
import { Toast } from "primereact/toast";
import { userService } from "./UserService";
import { Image } from 'primereact/image';
import { socketService } from "../services/SocketService";
function ProfilePage(){

    useEffect(() => {
        socketService.reconnect();
    }, []);
    
    useEffect(() => {
        const handleUnload = () => {
          socketService.deleteConnection(storageService.getUserInfo()?.id);
        };
    
        window.addEventListener('unload', handleUnload);
    
        return () => {
          window.removeEventListener('unload', handleUnload);
        };
    }, []);
    
    const navigate = useNavigate();
    const [id, setId] = useState<number>();
    const [user, setUser] = useState<any>();
    const toast = useRef<Toast>(null);
    const params = useParams();
    useEffect(() => {
        const func = async () => {
            if(params.id)
            {
                const id = Number(params.id);
                const response = await userService.getUser(id)
                if(response.status){
                    setUser(response.data);
                }
                else
                    console.log("dodaj toaster")
            }
            else
            {
                const id = Number(storageService.getUserInfo()?.id);
                const response = await userService.getUser(id)
                if(response.status){
                    setUser(response.data);
                }
                else
                    console.log("dodaj toaster")
            }
        }
        func();
    },[id])
    
    async function onUpload(e: FileUploadHandlerEvent){
            console.log(e.files[0]);
            const response = await userService.changePhoto(e.files[0]);
            if(response.status)
            {
                //navigate(0)
            }

    };
    
    return<>
    <div className="celina" style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
        <NavBar></NavBar>
        <div className="slika1 d-flex justify-content-between" style={{height: "auto", flex: 1}}>
            <SideBar></SideBar>
            <div className="slika2 d-flex flex-column" style={{flexGrow: 1, height: "auto"}}>
                <h1 style={{fontSize: '5vw', textAlign: "center", height: "auto", fontWeight: "bold"}}>{user?.fullName}</h1>
                <div className="d-flex justify-content-around">
                    <div className="d-flex justify-content-center" style={{width: "100%", height: "auto"}}>
                        <div>
                        <div>
                            <div style={{width: "30vw", height: "50vh", overflow: "hidden", position: "relative", backgroundColor: "#f0f0f0"}}>
                                <Image  preview src={enviorment.port + user?.photoPath} style={{width: "100%", height: "100%", objectFit: "cover", objectPosition: "center", position: "absolute"}}/>
                            </div>
                        </div>
                            {!params.id && <FileUpload style={{textAlign: "center", marginTop: "1vw", color: "#374151"}} auto mode="basic" accept="image/*" chooseLabel="Izmeni sliku" customUpload maxFileSize={10000000} uploadHandler={onUpload} />}
                        </div>
                    </div>
                    <div className="d-flex justify-content-center" style={{width: "100%", height: "auto"}}>
                        <div className="">
                            <h2 style={{fontWeight: "bold", margin: 0}}>Informacije o korisniku</h2>
                            <p style={{fontSize: "3vh", margin: 0}}><b>Email</b>: {user?.email}</p>
                            <p style={{fontSize: "3vh", margin: 0}}><b>Broj dodatih rešenja kolokvijuma</b>: {user?.kolokvijums}</p>
                            <p style={{fontSize: "3vh"}}><b>Broj dodatih resenja ispita</b>: {user?.ispits}</p>
                            <h2>Omiljeni predmeti</h2>
                            {user?.favoriteSubjects.length>0 && user?.favoriteSubjects?.map((fs: any) => (
                                <div className="d-flex justify-content-centar">
                                    <a style={{fontSize: "3vh", fontWeight: "bold", textDecoration: "underline"}} href={"/predmet/"+fs.subjectId+"/wiki"}>{fs.name}</a>
                                </div>
                            ))}
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