import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import { Menubar } from 'primereact/menubar';
import './MenuBar.css'
import { Outlet, Link } from "react-router-dom";
import { ReactElement } from "react";

interface Item {
    label: string | ReactElement;
    icon: string;
}

export default function BasicDemo() {

    const items: Item[] = [
        {
            label: <Link to="/Wikipedia" className="Link">Wikipedia</Link>,
            icon: 'pi pi-book',
        },
        {
            label: 'Registracija',
            icon: 'pi pi-user-plus',
        },
        {
            label: 'Prijava',
            icon: 'pi pi-sign-in',
            
        }
    ];
    
    const start = <div className=""><p style={{fontSize: "40px", float:"left", textAlign:"center"}} className="d-flex justify-content-center">PMFWiki</p></div>
    //<img src="../src/assets/logo4.png" height="76" style={{margin: "10px"}}></img>
    return (
        <>
            <div>
                <Menubar className="d-flex justify-content-between" model={items} start={start}/>    
            </div> 
            <Outlet></Outlet>
        </>
    )
}
        
        