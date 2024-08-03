import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import '../MenuBar/MenuBar.css'
import { Menubar } from 'primereact/menubar';
import { Outlet, Link, useLocation } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { storageService } from "../../services/StorageService";
import { Avatar } from 'primereact/avatar';  
import { MenuItem } from "primereact/menuitem";
import { enviorment } from "../../enviorment";
import { useEffect, useState } from "react";

function NavBar(){

    let location = useLocation();
    const userInfo = storageService.getUserInfo();
    const username = userInfo?.firstName + " " + userInfo?.lastName;
    const navigate = useNavigate();
    const [visible, setVisible] = useState<boolean>(true);
    const avatar = enviorment.port + userInfo?.photoPath;
    
    useEffect(() => {
        if(location.pathname === "/profilna-strana")
            setVisible(false);
        else
            setVisible(true);
    })
    const itemRenderer = (item : any) => (
        <span className="flex align-items-center p-menuitem-link">
            <Avatar image={avatar} shape="circle" />
            <span className="mx-2">{item.label}</span>
        </span>
    );
    

    const items : MenuItem[]= [
        {
            label: username,
            icon: 'pi pi-user',
            visible: visible,
            template: itemRenderer,
            command: () => {
                navigate("/profilna-strana")
            }
        },
        {
            label: "odjava",
            icon: 'pi pi-sign-out',
            command: () => {
                storageService.deleteCredentials();
                navigate(0);
            }
        }
    ];
    
    const start = <div className=""><p style={{fontSize: "40px", float:"left", textAlign:"center", fontFamily: "sans-serif", color:"rgb(255, 255, 255, 0.87)"}} className="d-flex justify-content-center"><Link to="/">PMFWiki</Link></p></div>
    
    return <>
        <div style={{width: "100%"}}>
            <Menubar className="d-flex justify-content-between" model={items} start={start}/>    
        </div> 
        <Outlet></Outlet>
        </>
}

export default NavBar;