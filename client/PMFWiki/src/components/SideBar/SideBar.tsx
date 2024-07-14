import { Menu } from 'primereact/menu';
import { Link, Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import '../SideBar/SideBar.css'
function SideBar(){
    const navigate = useNavigate();
    const items = [
        {
            label: "Informatika",
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
            <Menu className="" style={{height: "100vh", borderRadius: "0", padding: "0", width: "fit-content"}} model={items}/>    
            <Outlet></Outlet>
        </>
    )
}

export default SideBar;