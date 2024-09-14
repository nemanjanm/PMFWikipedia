import { useEffect, useRef, useState } from "react";
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
import { Dropdown } from "primereact/dropdown";
import { Toast } from "primereact/toast";

function Kolokvijum(){

    const [loader, setLoader] = useState<boolean>(false);
    const [kolokvijumi, setKolokvijumi] = useState<Array<any>>([]);
    const [visible, setVisible] = useState<boolean>(false)
    const [title, setTitle] = useState('');
    const [klk, setKlk] = useState('');
    const [year, setYear] = useState('');
    const [pop, setPop] = useState('');
    const [group, setGroup] = useState('');
    const [file, setFile] = useState<FileUploadHandlerEvent>();
    const [pom, setPom] = useState();
    const [allowed, setAllowed] = useState<boolean>(false);
    const toast = useRef<Toast>(null);
    const navigate = useNavigate();

    const years = [
        { value: "2010/2011"},
        { value: "2011/2012"},
        { value: "2012/2013"},
        { value: "2013/2014"},
        { value: "2014/2015"},
        { value: "2015/2016"},
        { value: "2016/2017"},
        { value: "2017/2018"},
        { value: "2018/2019"},
        { value: "2019/2020"},
        { value: "2020/2021"},
        { value: "2021/2022"},
        { value: "2022/2023"},
        { value: "2023/2024"},
        { value: "2024/2025"},
    ];

    const klks = [
        { value: "1."},
        { value: "2."},
        { value: "3."},
        { value: "4."},
    ];

    const pops = [
        { value: "regularni"},
        { value: "popravni"},
        { value: "januar"},
        { value: "februar"},
        { value: "mart"},
        { value: "april"},
        { value: "maj"},
        { value: "jun"},
        { value: "jul"},
        { value: "avgust"},
        { value: "septembar"},
        { value: "oktobar"},
        { value: "novembar"},
        { value: "decembar"},
    ];

    const groups = [
        { value: "I grupa"},
        { value: "II grupa"},
        { value: "III grupa"},
        { value: "IV grupa"},
    ];
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
            setLoader(true);
            const response = await kolokvijumService.getKolokvijum(Number((location.pathname).split('/').slice(2, -1).join('/')));
            if(response.status)
            {
                if(response.data[0].authorId !== 0)
                    setKolokvijumi(response.data)
                if(response.data[0].allowed && response.data[0].allowed === true)
                    setAllowed(true);
            }
            else{
                toast.current?.show({severity:'error', summary: "Greška", detail:response.errors[0], life: 4000});
            }
            setLoader(false)
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
        let title = klk + " " + pop + " kolokvijum " + year + " "+ group; 
        const response = await kolokvijumService.addKolokvijum(year ,title, file?.files[0], Number(storageService.getUserInfo()?.id), Number((location.pathname).split('/').slice(2, -1).join('/')));
        if(response.status)
        {
            socketService.addKolokvijum(Number(storageService.getUserInfo()?.id), response.data, Number((location.pathname).split('/').slice(2, -1).join('/')))
            navigate(0);
        }
        else{
            setVisible(false);
            toast.current?.show({severity:'error', summary: "Greška", detail:response.errors[0], life: 4000});
        }        
    }
    return <>{!loader && <div className="d-flex align-items-center justify-content-center flex-column" style={{width: "100%"}}>
        {allowed && <Button label="Dodaj" icon="pi pi-plus" style={{width: "20vw"}} onClick={handleAdd} ></Button>}
        {kolokvijumi?.length > 0 && kolokvijumi?.map(k => (
            <div style={{width: "80%", marginBottom: "1vh"}}><IspitKlkPanel info={k} flag={1}></IspitKlkPanel></div>
        ))}
        {kolokvijumi?.length === 0 && <p style={{fontSize: "3vw", color: "#374151", textAlign: "center"}}>Trenutno nema kolokvijuma na ovom predmetu</p>}
        {visible && <Dialog visible={visible} header={"Dodaj postavku kolokvijuma"} onHide={() => {if (!visible) return; setVisible(false); }}
            style={{ width: '70vw', textAlign: "center", height: "50vh"}}>
                <div className="d-flex align-items-center flex-column"> 
                    <div style={{width: "20vw"}} className="d-flex justify-content-center flex-column">
                        <div style={{marginBottom: "1vh"}} className="d-flex justify-content-center">
                        <Dropdown style={{textAlign: "left", marginRight: "2vh"}} value={klk} onChange={(e) => setKlk(e.value)} options={klks} optionLabel="value" placeholder="kolokvijum" className="w-full md:w-14rem" />
                        <Dropdown style={{textAlign: "left", marginRight: "2vh"}} value={year} onChange={(e) => setYear(e.value)} options={years} optionLabel="value" placeholder="godina" className="w-full md:w-14rem" />
                        <Dropdown style={{textAlign: "left", marginRight: "2vh"}} value={pop} onChange={(e) => setPop(e.value)} options={pops} optionLabel="value" placeholder="popravni/regularni" className="w-full md:w-14rem" />
                        <Dropdown style={{textAlign: "left"}} value={group} onChange={(e) => setGroup(e.value)} options={groups} optionLabel="value" placeholder="grupa" className="w-full md:w-14rem" />
                        </div>
                        <FileUpload style={{marginBottom: "0.3rem", width: "100%"}} chooseLabel="Dodaj file" mode="basic" name="demo[]" accept="*" customUpload onSelect={(e) => test(e)}  />
                    </div>
                    <div>
                        <Button onClick={() => addPost()}  label="Potvrdi" icon="pi pi-send" iconPos="right" style={{border: 0, borderRadius: "10%", marginTop: "5vh"}}/>
                    </div>
                </div>
        </Dialog>}
        <Outlet></Outlet>
        <Toast ref={toast}></Toast>
    </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default Kolokvijum;