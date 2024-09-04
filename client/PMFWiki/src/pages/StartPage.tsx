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

    const [selectedSubject, setSelectedSubject] = useState<FavoriteSubject | null>(null);
    function handleSubject(e: any){
        navigate("/predmet/"+e.value.subjectId+"/wiki");
    }

    function deletSubject(e: any){
        console.log(e.subjectId);        
        //da se prikaze dialog da li sam siguran da zelim da obrisem! pa onda zovem za uklanjanje iz omiljenih i saljem subjectId i userId
    }
    return <>
    <div className="celina" style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{flex: 1}}>
            <SideBar></SideBar>
            {!loader && <div style={{width: "100vh"}}>
                <h1 style={{textAlign: "center", fontWeight: "bold", marginTop: "10px"}}>Moji predmeti</h1>
                {favoriteSubjects && <ListBox value={selectedSubject} onChange={(e: ListBoxChangeEvent) => handleSubject(e)} options={favoriteSubjects} itemTemplate={(option: any) => (<div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}> <span>{option.name}</span> <Button style={{border: 0, fontSize: "1rem"}} label="Ukloni" onClick={(e) => {e.stopPropagation(); deletSubject(option);}} /></div>)} className="w-full md:w-14rem" listStyle={{maxHeight: "70vh", fontSize: "1.5rem"}}/>}
                {!favoriteSubjects && <p style={{fontSize: "3vw", color: "#374151", textAlign: "center"}}>Trenutno nemate omiljene predmete</p>}
            </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
            <div></div>
        </div>
        </div>
    </>
}

export default StartPage;