import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import ClipLoader from "react-spinners/ClipLoader";
import { Outlet, useNavigate, useParams } from "react-router-dom";
import SubjectNavBar from "../components/SubjectNavBar/SubjectNavBar";
import { subjectService } from "../services/SubjectService";
import { SubjectInfo } from "../models/SubjectInfo";
import PostPanel from "../components/PostPanel/PostPanel";
import { Button } from "primereact/button";
import { favoriteSubjectService } from "../services/FavoriteSubjectService";
import { CheckAllowModel } from "../models/CheckAllowModel";
import { storageService } from "../services/StorageService";
import { FavoriteSubjectInfo } from "../models/FavoriteSubjectInfo";

function SubjectPage(){

    const [loader, setLoader] = useState<boolean>(false);
    const params = useParams();
    const [subject, setSubject] = useState<SubjectInfo>();
    const [pom, setPom] = useState();
    const [pom2, setPom2] = useState();
    const navigate = useNavigate()

    useEffect(() => {
        async function getSubject(){
            setLoader(true);
            const response = await subjectService.getSubject(Number(params.id));
            if(response.status)
            {
                setSubject(response.data)
            }
            setLoader(false);
        }
        getSubject();
    }, [pom])

    async function addToFavorites(){
        const favorite: FavoriteSubjectInfo = {
            subjectId: Number(subject?.id),
            userId: Number(storageService.getUserInfo()?.id)
        }
        setLoader(true);
        const response = await favoriteSubjectService.addToFavorites(favorite);
        if(response.status)
            navigate(0);
        else
            console.log("Neka greska");
    }
    async function removeFromFavorites(){

        const favorite: FavoriteSubjectInfo = {
            subjectId: Number(subject?.id),
            userId: Number(storageService.getUserInfo()?.id)
        }
        setLoader(true);
        const response = await favoriteSubjectService.removeFavoriteSubject(favorite);
        if(response.status)
            navigate(0);
        else
            console.log("Neka greska");
    }
    return <>{!loader && <div className="celina" style={{height: "100vh", display: "flex", flexDirection: "column"}}>
    <NavBar></NavBar>
    <div className="d-flex justify-content-between" style={{height: "100vh", overflow: "hidden", display: "flex"}}>
        <SideBar></SideBar>
            <div className="sredina" style={{width: "100%", overflowY: "auto", overflowX: "hidden", flex: 1}}>
                <div className="d-flex justify-content around">
                <div style={{width: "30%"}}></div>
                <h2 className="justify-content-center" style={{textAlign: "center", fontWeight: "bold", margin: "1vh", width: "100%", height: "auto"}}>{subject?.name}</h2>
                <div style={{width: "30%", marginTop:"0.3rem", marginBottom:"0.3rem"}}>
                    {storageService.getUserInfo()?.program === subject?.programId ? subject?.allowed ? <Button onClick={removeFromFavorites} label="Ukloni iz omiljenih" icon="pi pi-star-fill"></Button> : <Button onClick={addToFavorites} label="Dodaj u omiljene" icon="pi pi-star"></Button> : <></>}</div>
                </div>
                <div style={{marginBottom: "0.5vh"}}>
                    <SubjectNavBar></SubjectNavBar>
                    <Outlet></Outlet>
                </div>
            </div>
            
        <div>
        </div>
    </div>
    
    </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default SubjectPage;