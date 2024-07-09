import { storageService } from "../components/StorageService";
import { useNavigate } from "react-router-dom";
function StartPage(){

    const navigate = useNavigate();

    function handle(){
        storageService.deleteCredentials();
        navigate(0);
    }

    return <>POCETNA STRANA
            <button onClick={handle}> Odjava </button></>
}

export default StartPage;