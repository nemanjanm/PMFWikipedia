import { useNavigate } from "react-router-dom";
import { storageService } from "../../services/StorageService";
import { enviorment } from "../../enviorment";
import { Button } from "primereact/button";
import { useEffect, useState } from "react";
import { Panel } from "primereact/panel";
import { Dialog } from "primereact/dialog";
import { InputText } from "primereact/inputtext";
import { FileUpload, FileUploadHandlerEvent } from "primereact/fileupload";
import { kolokvijumService } from "../../services/KolokvijumService";
import { socketService } from "../../services/SocketService";
import { ConfirmDialog } from "primereact/confirmdialog";
import { ispitService } from "../../services/Ispit";

function IspitKlkPanel(props: any){
    const { info, flag } = props;
    console.log(flag);
    const navigate = useNavigate();
    const [del, setDel] = useState<boolean>(true);
    const [loader, setLoader] = useState<boolean>(false);
    const [visible, setVisible] = useState<boolean>(false)
    const [title, setTitle] = useState('');
    const [file, setFile] = useState<FileUploadHandlerEvent>();
    const [isDialogVisible, setDialogVisible] = useState<boolean>(false);
    const [isDialogVisible2, setDialogVisible2] = useState<boolean>(false);
    useEffect(() => {
        const checkDel = () =>{
            if(info.authorId === storageService.getUserInfo()?.id)
                setDel(false)
        }
        checkDel();
    }, [del])

    function showUser(e: any){
        navigate("/profilna-strana/"+e);
    }   

    function test(e : any){
        setFile(e)
    }

    const confirmDelete = () => {
        setDialogVisible(true);
    };

    function confirmDelete2(){
        setDialogVisible2(true);
    } 
    const accept = async () =>{
        let response;
        if(flag === 1)
            response = await kolokvijumService.deleteKolokvijum(info.id);
        else
            response = await ispitService.deleteIspit(info.id);
        
            if(response.status){
            navigate(0);
        }
        else{
            //napisi nesto za gresku
        }
    }

    const accept2 = async (id: any) =>{
        let response;
        if(flag === 1)
            response = await kolokvijumService.deleteResenje(id);
        else
            response = await ispitService.deleteResenje(id);
        
            if(response.status){
            navigate(0);
        }
        else{
            //napisi nesto za gresku
        }
    }

    const handleReject = () => {
        setDialogVisible(false);
    };

    const handleReject2 = () => {
        setDialogVisible2(false);
    };
    async function addResenje() {
        let response;
        if(flag === 1)
            response = await kolokvijumService.addResenje(info.id, file?.files[0], Number(storageService.getUserInfo()?.id), Number((location.pathname).split('/').slice(2, -1).join('/')));
        else
            response = await ispitService.addResenje(info.id, file?.files[0], Number(storageService.getUserInfo()?.id), Number((location.pathname).split('/').slice(2, -1).join('/')));
        if(response.status)
        {
            if(flag === 1)
                socketService.addResenje(Number(storageService.getUserInfo()?.id), response.data, Number((location.pathname).split('/').slice(2, -1).join('/')))
            else
                socketService.addIspitResenje(Number(storageService.getUserInfo()?.id), response.data, Number((location.pathname).split('/').slice(2, -1).join('/')))
            navigate(0);
        }        
    }
    const headerTemplate = (options : any) => {
        const className = `${options.className} justify-content-space-between`;
        return (<>
            <div className={className}>
                {info.authorId !== storageService.getUserInfo()?.id ? <a href={"/profilna-strana/"+ info.authorId}>
                <div className="d-flex align-items-center gap-2" onClick={() => showUser(info.authorId)}>
                    <span className="font-bold">{info.authorName}</span>
                </div>
                </a> :
                <div className="d-flex align-items-center gap-2">
                    <span className="font-bold">{info.authorName}</span>
                </div>    
                }
                <div>
                    {!del && <Button onClick={(e) => {e.stopPropagation(); confirmDelete();}} className="izmeni" icon="pi pi-trash" rounded severity="danger" aria-label="Obriši" />}
                </div>
            </div>
            </>
        );
    };

    return(<>
        <Panel  header="Header" headerTemplate={headerTemplate} toggleable>
            <h2 className="d-flex justify-content-center" style={{color: "white"}}>{info.title}</h2>
            <div className='d-flex justify-content-between align-items-center' style={{fontSize: "5vh"}}>
                <a href={enviorment.port + info.filePath} download={info.title}>
                    {"preuzmi postavku"}
                </a>
                <div></div>
                <div className="d-flex flex-column align-items-end">
                {info.resenja?.length > 0 && info.resenja?.map((r : any) => (
                    <div className="d-flex flex-column align-items-end" >
                    <span >{storageService.getUserInfo()?.id === r.authorId && <Button onClick={confirmDelete2} style={{width: "5vh", height: "5vh", border: 0, marginRight: "1vh"}} icon="pi pi-trash"></Button>}<a href={enviorment.port + r.filePath} download={enviorment.port + r.filePath} >{"preuzmi rešenje"}</a></span>
                    {r.authorId !== storageService.getUserInfo()?.id ? <a href={"/profilna-strana/"+ r.authorId} style={{fontSize: "2vh"}}>{r.authorName}</a> : <a style={{fontSize: "2vh"}}>{r.authorName}</a>}
                    <ConfirmDialog visible={isDialogVisible2}onHide={handleReject2}message="Da li ste sigurni da želite da obrišete rešenje?"header="Obriši post"icon="pi pi-info-circle"acceptClassName="p-button-danger"rejectLabel="Odustani"acceptLabel="Potvrdi" accept={() => accept2(r.id)}reject={handleReject2}/>
                    </div>
                ))}
                <Button onClick={() => setVisible(true)} style={{fontSize: "3vh", width: "10vw", marginTop: "2vh", height: "8vh"}} icon="pi pi-plus" label="Rešenje"></Button>
                </div>
            </div>
        </Panel>
        <ConfirmDialog visible={isDialogVisible}onHide={handleReject}message="Da li ste sigurni da želite da obrišete kolokvijum?"header="Obriši post"icon="pi pi-info-circle"acceptClassName="p-button-danger"rejectLabel="Odustani"acceptLabel="Potvrdi"accept={accept}reject={handleReject}/>
        {visible && <Dialog headerStyle={{textAlign: "center"}} visible={visible} header={"Dodaj rešenje kolokvijuma"} onHide={() => {if (!visible) return; setVisible(false); }}
            style={{ width: '50vw', textAlign: "center", height: "50vh"}}>
                <div className="d-flex align-items-center flex-column">
                    <div style={{width: "20vw"}} className="d-flex justify-content-center flex-column">
                        <InputText style={{marginBottom: "0.3rem"}} placeholder="Naslov" value={title} onChange={(e) => setTitle(e.target.value)} />
                        <FileUpload style={{marginBottom: "0.3rem", width: "100%"}} chooseLabel="Dodaj file" mode="basic" name="demo[]" accept="*" customUpload onSelect={(e) => test(e)}  />
                    </div>
                    <div>
                        <Button onClick={() => addResenje()}  label="Potvrdi" icon="pi pi-send" iconPos="right" style={{border: 0, borderRadius: "10%", marginTop: "5vh"}}/>
                    </div>
                </div>
        </Dialog>}
    </>)
}

export default IspitKlkPanel;