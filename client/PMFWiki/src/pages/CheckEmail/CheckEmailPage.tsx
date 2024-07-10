import "../../index.css"
import MenuBar from "../../components/MenuBar/MenuBar";

function ChceckEmailPage(){
    return (<>
    <MenuBar></MenuBar>
        <div className="d-column justify-content-center" style={{textAlign: "center", margin: "10px", fontWeight: "bold"}}> 
            <p>Proverite vaš e-mail</p>
        </div>
    </>)
}

export default ChceckEmailPage;