import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import '../MenuBar/MenuBar.css'
import { Menubar } from 'primereact/menubar';
import { Outlet, Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";
import { storageService } from "../StorageService";
import { Avatar } from 'primereact/avatar';  
import { MenuItem } from "primereact/menuitem";
import { enviorment } from "../../enviorment";

function NavBar(){

    const userInfo = storageService.getUserInfo();
    console.log(userInfo?.firstName);
    const username = userInfo?.firstName + " " + userInfo?.lastName;
    const navigate = useNavigate();
    const avatar = enviorment.port + userInfo?.photoPath;
    console.log(avatar);
    
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
        <div>
            <Menubar className="d-flex justify-content-between" model={items} start={start}/>    
        </div> 
        <Outlet></Outlet>
        </>
}

export default NavBar;