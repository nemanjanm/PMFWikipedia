import { Menu } from 'primereact/menu';
import { Link, Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import '../SideBar/SideBar.css'
import { storageService } from '../StorageService';
import { getName, programmes } from '../Programme';
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
                navigate("/")
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
            <div>
                <Menu className="" style={{height: "100vh", borderRadius: "0", padding: "0", width: "fit-content"}} model={items}/>    
            </div>
            <Outlet></Outlet>
        </>
    )
}

export default SideBar;