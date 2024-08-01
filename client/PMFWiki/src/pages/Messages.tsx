import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import "./Messages.css";
import { useLocation, useNavigate } from "react-router-dom";
import { ListBox, ListBoxChangeEvent } from 'primereact/listbox';
import { favoriteSubjectService } from "../services/FavoriteSubjectService";
import { storageService } from "../services/StorageService";
import { FavoriteSubject } from "../models/FavoriteSubject";
import { ClipLoader } from "react-spinners";
import { LoginInfo } from "../models/LoginInfo";
import { enviorment } from "../enviorment";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";

function Messages(){

    const location = useLocation();
    const user = location.state || {};

    const [selectedCity, setSelectedCity] = useState(null);
    const cities = [
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' }
    ];

    const [loader, setLoader] = useState<boolean>(false);
    const [selectedUser, setSelectedUser] = useState<string>("");

    return<>
        <div className="celina" style={{display: "flex", flexDirection: "column", height: "100vh", boxSizing: "border-box"}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100%"}}>
            <SideBar></SideBar>
            <ListBox filter emptyFilterMessage={"Nema rezultata"} options={cities} emptyMessage={"Nema rezultata"} value={selectedCity} optionLabel="name" className="w-full md:w-14rem" style={{borderRadius: "0", width: "25vw"}} listStyle={{fontSize: "3vh"}}/>
            <div style={{backgroundColor: "", flexGrow: 1, display: "flex", justifyContent: "flex-end", flexDirection:"column"}}>
            <div className="d-flex" style={{alignItems: "center", borderBottom: "0.2rem solid #424b57"}}>
                <img src={enviorment.port + user?.photoPath} style={{border: "0", borderRadius: "50%", height: "10vh", width: "10vh", margin: "2vh"}}></img>
                <p style={{textAlign: "center", marginBottom: "0rem"}}>{user?.fullName}</p>
            </div>
            <div style={{marginTop: "auto", display: "flex", justifyContent: "flex-end", borderRadius: "0", width:"100%"}}>
                <InputText style={{width:"10vw", flex: 1, border: "0.2rem solid #424b57", borderRight: "0", borderRadius: "0", color:"#ffffffde"}} type="text" className="p-inputtext-lg" placeholder="Unesi poruku"  />
                <Button style={{border: "0.2rem solid #424b57", borderRadius: "0", color:"#ffffffde"}}>Pošalji</Button>
            </div>
            </div>
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
            <div></div>
        </div>
        </div>
    </>
}

export default Messages;