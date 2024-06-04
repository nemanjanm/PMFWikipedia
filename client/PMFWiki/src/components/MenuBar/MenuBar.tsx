import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import './MenuBar.css'
import { Menubar } from 'primereact/menubar';
import { Outlet, Link } from "react-router-dom";
import { useNavigate } from "react-router-dom";

export default function MenuBar() {
    const navigate = useNavigate();
    
    const items = [
        {
            label: "Wikipedia",
            icon: 'pi pi-book',
            command: () => {
                navigate("/Wikipedia")
            }
        },
        {
            label: 'Registracija',
            icon: 'pi pi-user-plus',
            command: () => {
                navigate("/Registracija")
            }
        },
        {
            label: 'Prijava',
            icon: 'pi pi-sign-in',
            command: () => {
                navigate("/Prijava")
            }
        }
    ];
    
    const start = <div className=""><p style={{fontSize: "40px", float:"left", textAlign:"center", fontFamily: "sans-serif", color:"rgb(255, 255, 255, 0.87)"}} className="d-flex justify-content-center"><Link to="/">PMFWiki</Link></p></div>
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
        
        