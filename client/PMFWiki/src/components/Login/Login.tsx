import { Outlet } from "react-router-dom";
import "../../index.css"
function Login(){
    return(
    <div>
        <p>
            Login
        </p>
        <Outlet></Outlet>
    </div>
    )
}

export default Login;