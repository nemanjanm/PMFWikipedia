import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useNavigate } from "react-router-dom";
import { ListBox, ListBoxChangeEvent } from 'primereact/listbox';
import { favoriteSubjectService } from "./FavoriteSubjectService";
import { storageService } from "../components/StorageService";
import { FavoriteSubject } from "../models/FavoriteSubject";
import { ClipLoader } from "react-spinners";

function StartPage(){

    const [favoriteSubjects, setFavoriteSubjcets] = useState<Array<FavoriteSubject>>();
    const [temp, setTemp] = useState(false)
    const [loader, setLoader] = useState<boolean>(false);
    const id = storageService.getUserInfo()?.id;
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
        console.log(e);
        console.log(e.value);
        //U ZAVISNOSTI STA MI TREBA
    }
    return <>
    <div className="celina" style={{height: "fit-content"}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100vh"}}>
            <SideBar></SideBar>
            {!loader && <div style={{width: "100vh"}}>
                <h1 style={{textAlign: "center", fontWeight: "bold", marginTop: "10px"}}>Moji predmeti</h1>
                {favoriteSubjects && <ListBox value={selectedSubject} onChange={(e: ListBoxChangeEvent) => handleSubject(e)} options={favoriteSubjects} optionLabel="name" className="w-full md:w-14rem" listStyle={{maxHeight: "70vh"}}/>}
                {!favoriteSubjects && <p style={{fontSize: "30px", color: "#ffffffde"}}>Trenutno nemate omiljene predmete</p>}
            </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
            <div></div>
        </div>
        </div>
    </>
}

export default StartPage;