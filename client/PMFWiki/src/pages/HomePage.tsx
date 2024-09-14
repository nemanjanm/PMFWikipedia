import "primereact/resources/themes/lara-dark-blue/theme.css";
import 'bootstrap/dist/css/bootstrap.min.css';
import "primereact/resources/primereact.min.css"
import "primereact/resources/primereact.css"
import 'primeicons/primeicons.css';
import "../index.css";
import { useLocation } from "react-router-dom";
import MenuBar from "../components/MenuBar/MenuBar";
function HomePage(){

    let location = useLocation();
    if(location.pathname === "/")
    {
    return(<>
            <MenuBar></MenuBar>
            <div className="d-flex justify-content-center">
                <div style={{textAlign: "center", fontFamily: ""}}>
                    <p style={{color: "#374151", fontWeight: "bold"}}>{"DOBRO DOŠLI NA SAJT PMF WIKIPEDIA"}</p>
                    <div className="d-flex justify-content-center" style={{width: "70vw", height: "70vh"}}>
                        <img src="../src/assets/logo4.png" style={{width: "100%", height: "100%", objectFit: "contain", border: 0}}></img>
                    </div>
                </div>
            </div>
            </>)
    }
    else{
        return(<>
                <MenuBar></MenuBar>
            </>)
    }
}

export default HomePage;