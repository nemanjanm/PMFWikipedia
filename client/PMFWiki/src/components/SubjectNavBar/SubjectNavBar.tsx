import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import './SubjectNavBar.css'
import { Menubar } from 'primereact/menubar';
import { Outlet, Link, useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { MenuItem } from "primereact/menuitem";
import { useEffect, useState } from "react";

function NavBar(){

    let location = useLocation();
    const navigate = useNavigate();
    const [visible, setVisible] = useState<boolean>(true);
    
    useEffect(() => {
        if(location.pathname === "/profilna-strana")
            setVisible(false);
        else
            setVisible(true);
    })
    const itemRenderer = (item : any) => (
        <span className="flex align-items-center p-menuitem-link">
            <span className="mx-2">{item.label}</span>
        </span>
    );
    

    const items : MenuItem[]= [
        {
            label: "Wikipedia",
            visible: visible,
            template: itemRenderer,
            command: () => {
                if(location.pathname.includes("kolokvijum") || location.pathname.includes("ispit"))
                    navigate(String(location.pathname).split('/').slice(0, -1).join('/') + "/wiki")  
            }
        },
        {
            label: "Kolokvijumi",
            visible: visible,
            template: itemRenderer,
            command: () => {
                if(location.pathname.includes("wiki") || location.pathname.includes("ispit"))
                    navigate(String(location.pathname).split('/').slice(0, -1).join('/') + "/kolokvijum")  
            }
        },
        {
            label: "Ispiti",
            visible: visible,
            template: itemRenderer,
            command: () => {
                if(location.pathname.includes("wiki") || location.pathname.includes("kolokvijum"))
                    navigate(String(location.pathname).split('/').slice(0, -1).join('/') + "/ispit")  
            }
        }
    ];
    
    return <>
        <div className="border" style={{width: "100%"}}>
            <Menubar className="d-flex justify-content-center" model={items}/>    
        </div> 
        </>
}

export default NavBar;