import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { storageService } from "../components/StorageService";
import { useNavigate } from "react-router-dom";
function StartPage(){

    const navigate = useNavigate();


    ///LISTBOX, ORDERLIST
    return <>
        <NavBar></NavBar>
        <SideBar></SideBar>
    </>
}

export default StartPage;