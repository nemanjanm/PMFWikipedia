import { PanelMenu } from 'primereact/panelmenu';
import { Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import './SideBar.css'
import { storageService } from '../../services/StorageService';
import { getName, programmes } from '../../models/Programme';
import { PanelHelper } from '../PanelHelper';

function SideBar(){
    const navigate = useNavigate();
    const programNumber = storageService.getUserInfo()?.program;

    const program = getName(programNumber);

    const items  = [
        {
            label: program,
            icon: 'pi pi-book',
            command: () => {
                navigate("/predmeti")
            },
        },
        {
            label: 'Obaveštenja',
            icon: 'pi pi-bell',
            command: () => {
                navigate("/")
            }
        },
        {
            label: 'Poruke',
            icon: 'pi pi-inbox',
            badge: 3,
            command: () => {
                navigate("/")
            }
        },
        {
            label: 'Ostale oblasti',
            items: PanelHelper(),
        }
    ];
    return (
        <>
            <div className='d-flex justify-content-centar' style={{width: "15vw", position: "relative", overflow: 'auto', top: 0, bottom: 0, height: "100vw", backgroundColor: "#374151"}}>
                <PanelMenu style={{height:"100%", width:"100%", border: 0, borderRadius: "0", padding: "0", fontSize: "1vw"}} model={items}/>    
            </div>
            <Outlet></Outlet>
        </>
    )
}

export default SideBar;