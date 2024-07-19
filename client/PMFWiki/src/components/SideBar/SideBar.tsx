import { Menu } from 'primereact/menu';
import { Link, Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import '../SideBar/SideBar.css'
import { storageService } from '../../services/StorageService';
import { getName, programmes } from '../../models/Programme';
import { useState } from 'react';

function SideBar(){
    const navigate = useNavigate();
    const programNumber = storageService.getUserInfo()?.program;

    const program = getName(programNumber);

    const items = [
        {
            label: program,
            icon: 'pi pi-book',
            command: () => {
                navigate("/predmeti")
            }
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
            icon: 'pi pi-list',
            command: () => {
                navigate("/")
            }
        }
    ];
    return (
        <>
            <div className='d-flex justify-content-centar' style={{width: "fit-content", position: "relative", overflowY: 'auto', top: 0, bottom: 0, height: "100vw", backgroundColor: "#374151"}}>
                <Menu style={{width:"fit-content", border: 0, borderRadius: "0", padding: "0", fontSize: "1vw"}} model={items}/>    
            </div>
            <Outlet></Outlet>
        </>
    )
}

export default SideBar;