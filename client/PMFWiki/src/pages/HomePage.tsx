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
                    <p style={{color: "#374151", fontWeight: "bold"}}>{"PMF WIKIPEDIA"}</p>
                    <img src="../src/assets/logo4.png" width="350px"></img>
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