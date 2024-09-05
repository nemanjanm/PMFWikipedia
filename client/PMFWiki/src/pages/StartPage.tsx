import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useNavigate } from "react-router-dom";
import { ListBox, ListBoxChangeEvent } from 'primereact/listbox';
import { favoriteSubjectService } from "../services/FavoriteSubjectService";
import { storageService } from "../services/StorageService";
import { FavoriteSubject } from "../models/FavoriteSubject";
import { ClipLoader } from "react-spinners";
import { socketService } from "../services/SocketService";
import { Button } from "primereact/button";
import { confirmDialog, ConfirmDialog } from 'primereact/confirmdialog';
import { FavoriteSubjectInfo } from "../models/FavoriteSubjectInfo";
import "./StartPage.css"

function StartPage(){

    const navigate = useNavigate();
    const [favoriteSubjects, setFavoriteSubjcets] = useState<Array<FavoriteSubject>>();
    const [temp, setTemp] = useState(false)
    const [loader, setLoader] = useState<boolean>(false);
    const id = storageService.getUserInfo()?.id;
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
    
    useEffect(() => {  
        async function getFavoriteSubjects(){
            setLoader(true);
            const response = await favoriteSubjectService.getFavoriteSubjects(id as number);
            if(response.status)
                setFavoriteSubjcets(response.data);
            setLoader(false);
        }
        getFavoriteSubjects()
    }, [temp])

    const accept = async (e: any) =>{
        const subject: FavoriteSubjectInfo = {
            subjectId: e.subjectId,
            userId: Number(storageService.getUserInfo()?.id)
        }

        setLoader(true)
        const response = await favoriteSubjectService.removeFavoriteSubject(subject);
        if(response.status){
            navigate(0); 
        }
        else{
            //napisi nesto za gresku
        }
    }
    const confirm = (e : any) => {
        confirmDialog({
            message: 'Da li ste sigurni da želite da uklonite predmet iz omiljenih predmeta?',
            header: 'Ukloni predmet',
            icon: 'pi pi-info-circle',
            acceptClassName: 'p-button-danger',
            rejectLabel: "Odustani",
            acceptLabel: "Potvrdi",
            accept: () => accept(e)
        });
    };

    const [selectedSubject, setSelectedSubject] = useState<FavoriteSubject | null>(null);
    function handleSubject(e: any){
        navigate("/predmet/"+e.value.subjectId+"/wiki");
    }
    return <>
    <div className="celina" style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{flex: 1}}>
            <SideBar></SideBar>
            <ConfirmDialog />
            {!loader && <div style={{width: "100vh"}}>
                <h1 style={{textAlign: "center", fontWeight: "bold", marginTop: "10px"}}>Moji predmeti</h1>
                {favoriteSubjects && <ListBox value={selectedSubject} onChange={(e: ListBoxChangeEvent) => handleSubject(e)} options={favoriteSubjects} itemTemplate={(option: any) => (<div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}> <span>{option.name}</span> <Button style={{border: 0, fontSize: "1rem"}} label="Ukloni" onClick={(e) => {e.stopPropagation(); confirm(option);}} /></div>)} className="w-full md:w-14rem" listStyle={{maxHeight: "70vh", fontSize: "1.5rem"}}/>}
                {!favoriteSubjects && <p style={{fontSize: "3vw", color: "#374151", textAlign: "center"}}>Trenutno nemate omiljene predmete</p>}
            </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
            <div></div>
        </div>
        </div>
    </>
}

export default StartPage;