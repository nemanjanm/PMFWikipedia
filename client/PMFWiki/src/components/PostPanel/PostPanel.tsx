import { Panel } from 'primereact/panel';
import { Avatar } from 'primereact/avatar';
import './PostPanel.css'
import { Button } from 'primereact/button';
import { enviorment } from '../../enviorment';
import { storageService } from '../../services/StorageService';
import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { postService } from '../../services/PostService';
import { confirmDialog, ConfirmDialog } from 'primereact/confirmdialog';
function PostPanel(info : any) {

    const navigate = useNavigate();
    const [loader, setLoader] = useState<boolean>(false);
    const [del, setDel] = useState<boolean>(true);
    const [isDialogVisible, setDialogVisible] = useState<boolean>(false);
    useEffect(() => {
        const checkDel = () =>{
            if(info.info.authorId === storageService.getUserInfo()?.id)
                setDel(false)
        }
        checkDel();
    }, [del])
   
    const messageDate = new Date(info.info.timeStamp);
    const thisDate = new Date();
    let result : string;
    const diffInMilliseconds: number = thisDate.getTime() - messageDate.getTime();
    const diffInMinutes: number = Math.floor(diffInMilliseconds / 1000 / 60);
    const diffInHours: number = Math.floor(diffInMilliseconds / 1000 / 60 / 60);
    const diffInDays: number = Math.floor(diffInMilliseconds / 1000 / 60 / 60 / 24);
    if(diffInMinutes < 1)
        result = "sada";
    else if(diffInMinutes < 60)
        result = "pre " + diffInMinutes.toString() + " minuta";
    else if(diffInHours < 24 && diffInHours >= 1)
        result = "pre " + diffInHours.toString() + " sati";
    else if(diffInDays >=1 && diffInDays <= 5)
        result = "pre " + diffInDays.toString() + " dana";
    else
        result = messageDate.toLocaleDateString();

    const accept = async () =>{
        setLoader(true)
        const response = await postService.deletePost(info.info.postId);
        if(response.status){
            navigate(0); 
        }
        else{
            //napisi nesto za gresku
        }
    }
    const confirmDelete = () => {
        setDialogVisible(true);
    };
    const handleReject = () => {
        setDialogVisible(false);
    };
    function showUser(e: any){
        navigate("/profilna-strana/"+e);
    }
    const headerTemplate = (options : any) => {
        const className = `${options.className} justify-content-space-between`;
        return (
            <div className={className}>
                {info.info.authorId !== storageService.getUserInfo()?.id ? <a href={"/profilna-strana/"+ info.info.authorId}>
                <div className="d-flex align-items-center gap-2" onClick={() => showUser(info.info.authorId)}>
                    <img alt={info.info.photoPath} src={enviorment.port + info.info.photoPath} style={{ width: '5vh', height: "5vh", borderRadius: "50%"}}/>
                    <span className="font-bold">{info.info.authorName}</span>
                </div>
                </a> :
                <div className="d-flex align-items-center gap-2" onClick={() => showUser(info.info.authorId)}>
                    <img alt={info.info.photoPath} src={enviorment.port + info.info.photoPath} style={{ width: '5vh', height: "5vh", borderRadius: "50%"}}/>
                    <span className="font-bold">{info.info.authorName}</span>
                </div>    
                }
                <div>
                    {info.info.allowed && <Button className="izmeni" icon="pi pi-pencil" rounded severity="danger" aria-label="Izmeni" />}
                    {!del && <Button onClick={(e) => {e.stopPropagation(); confirmDelete();}} className="izmeni" icon="pi pi-trash" rounded severity="danger" aria-label="Obriši" />}
                </div>
            </div>
        );
    };

    const footerTemplate = (options : any) => {
        const className = `${options.className} flex flex-wrap align-items-center justify-content-between gap-3`;
        return (
            <div className={className} style={{borderBottomLeftRadius: "0.5rem", borderBottomRightRadius: "0.5rem" }}>
                <span className="p-text-secondary">{result}</span>
            </div>
        );
    };

    return (<>
        <ConfirmDialog visible={isDialogVisible}onHide={handleReject}message="Da li ste sigurni da želite da obrišete post?"header="Obriši post"icon="pi pi-info-circle"acceptClassName="p-button-danger"rejectLabel="Odustani"acceptLabel="Potvrdi"accept={accept}reject={handleReject}/>
        <Panel header="Header" headerTemplate={headerTemplate} footerTemplate={footerTemplate} toggleable>
            <h1 style={{color: "white"}}>{info.info.title}</h1>
            <p style={{color: "white"}} className="m-0">
                {info.info.content}
            </p>
        </Panel>
        </>
    )
}
 
export default PostPanel;