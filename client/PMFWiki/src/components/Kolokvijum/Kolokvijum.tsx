import { useEffect, useState } from "react";
import { socketService } from "../../services/SocketService";
import { storageService } from "../../services/StorageService";
import { Outlet, useNavigate } from "react-router-dom";
import { Button } from "primereact/button";
import ClipLoader from "react-spinners/ClipLoader";
import { kolokvijumService } from "../../services/KolokvijumService";
import { Dialog } from "primereact/dialog";
import { InputText } from "primereact/inputtext";
import { FileUpload, FileUploadHandlerEvent } from "primereact/fileupload";
import './Kolokvijum.css'
import IspitKlkPanel from "../IspitKlkPanel/IspitKlkPanel";

function Kolokvijum(){

    const [loader, setLoader] = useState<boolean>(false);
    const [kolokvijumi, setKolokvijumi] = useState<Array<any>>([]);
    const [visible, setVisible] = useState<boolean>(false)
    const [title, setTitle] = useState('');
    const [file, setFile] = useState<FileUploadHandlerEvent>();
    const [pom, setPom] = useState();
    const [allowed, setAllowed] = useState<boolean>(false);
    const navigate = useNavigate();

    useEffect(() => {
        socketService.reconnect();
    }, []);

    useEffect(() => {
        const handleUnload = () => {
          socketService.deleteConnection(storageService.getUserInfo()?.id);
        };
    
        window.addEventListener('unload', handleUnload);
    
        return () => {
          window.removeEventListener('unload', handleUnload);
        };
    }, []);

    useEffect(() => {
        async function getKolokvijum(){
            const response = await kolokvijumService.getKolokvijum(Number((location.pathname).split('/').slice(2, -1).join('/')));
            if(response.status)
            {
                if(response.data[0].authorId !== 0)
                    setKolokvijumi(response.data)
                if(response.data[0].allowed && response.data[0].allowed === true)
                    setAllowed(true);
            }
        }
        getKolokvijum();
    }, [pom]);

    function handleAdd(){
        setVisible(true);
    }

    function test(e : any){
        setFile(e)
    }

    async function addPost() {
        const response = await kolokvijumService.addKolokvijum(title, file?.files[0], Number(storageService.getUserInfo()?.id), Number((location.pathname).split('/').slice(2, -1).join('/')));
        if(response.status)
        {
            socketService.addKolokvijum(Number(storageService.getUserInfo()?.id), response.data, Number((location.pathname).split('/').slice(2, -1).join('/')))
            navigate(0);
        }        
    }
    return <>{!loader && <div className="d-flex align-items-center justify-content-center flex-column" style={{width: "100%"}}>
        {allowed && <Button label="Dodaj" icon="pi pi-plus" style={{width: "20vw"}} onClick={handleAdd} ></Button>}
        {kolokvijumi?.length > 0 && kolokvijumi?.map(k => (
            <div style={{width: "80%", marginBottom: "1vh"}}><IspitKlkPanel info={k} flag={1}></IspitKlkPanel></div>
        ))}
        {kolokvijumi?.length === 0 && <p style={{fontSize: "3vw", color: "#374151", textAlign: "center"}}>Trenutno nema kolokvijuma na ovom predmetu</p>}
        {visible && <Dialog visible={visible} header={"Dodaj postavku kolokvijuma"} onHide={() => {if (!visible) return; setVisible(false); }}
            style={{ width: '50vw', textAlign: "center", height: "50vh"}}>
                <div className="d-flex align-items-center flex-column">
                    <div style={{width: "20vw"}} className="d-flex justify-content-center flex-column">
                        <InputText style={{marginBottom: "0.3rem"}} placeholder="Naslov" value={title} onChange={(e) => setTitle(e.target.value)} />
                        <FileUpload style={{marginBottom: "0.3rem", width: "100%"}} chooseLabel="Dodaj file" mode="basic" name="demo[]" accept="*" customUpload onSelect={(e) => test(e)}  />
                    </div>
                    <div>
                        <Button onClick={() => addPost()}  label="Potvrdi" icon="pi pi-send" iconPos="right" style={{border: 0, borderRadius: "10%", marginTop: "5vh"}}/>
                    </div>
                </div>
        </Dialog>}
        <Outlet></Outlet>
    </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default Kolokvijum;