import { useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useNavigate } from "react-router-dom";
import { ListBox, ListBoxChangeEvent } from 'primereact/listbox';

interface City {
    name: string;
    code: string;
} 

function StartPage(){

    const navigate = useNavigate();
    const [selectedCity, setSelectedCity] = useState<City | null>(null);
    const cities: City[] = [
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
        { name: 'Rome', code: 'RM' },
        { name: 'London', code: 'LDN' },
        { name: 'Istanbul', code: 'IST' },
        { name: 'Paris', code: 'PRS' },
        { name: 'New York', code: 'NY' },
        { name: 'Rome', code: 'RM' },
    
    ];
    ///LISTBOX, ORDERLIST
    return <>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100vh"}}>
            <SideBar></SideBar>
            <div>
                <h1 style={{textAlign: "center", fontWeight: "bold", marginTop: "10px"}}>Moji predmeti</h1>
                <div className="card flex justify-content-center" >  
                    <ListBox value={selectedCity} onChange={(e: ListBoxChangeEvent) => setSelectedCity(e.value)} options={cities} optionLabel="name" className="w-full md:w-14rem" listStyle={{maxHeight: "70vh"}}/>
                </div>
            </div>
            <div></div>
        </div>
    </>
}

export default StartPage;