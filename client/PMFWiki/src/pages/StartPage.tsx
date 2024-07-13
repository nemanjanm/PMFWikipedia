import NavBar from "../components/NavBar/NavBar";
import { storageService } from "../components/StorageService";
import { useNavigate } from "react-router-dom";
function StartPage(){

    const navigate = useNavigate();

    function handle(){
        storageService.deleteCredentials();
        navigate(0);
    }

    return <>
    <NavBar></NavBar>
    POCETNA STRANA
            <button onClick={handle}> Odjava </button></>
}

export default StartPage;