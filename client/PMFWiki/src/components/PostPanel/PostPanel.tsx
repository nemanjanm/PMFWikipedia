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
import { InputText } from 'primereact/inputtext';
import { commentService } from '../../services/CommentService';
import { CommentInfo } from '../../models/CommentInfo';
import { CommentModel } from '../../models/CommentModel';
import { Dialog } from 'primereact/dialog';
import { InputTextarea } from 'primereact/inputtextarea';
import { PostInfo } from '../../models/PostInfo';
import { socketService } from '../../services/SocketService';
import { messageEmitter } from '../../services/EventEmmiter';



function PostPanel(props : any) {
    const { info, subjectId } = props;
    const navigate = useNavigate();
    const [loader, setLoader] = useState<boolean>(false);
    const [del, setDel] = useState<boolean>(true);
    const [visible, setVisible] = useState<boolean>(false);
    const [dialog2Visible, setDialog2Visible] = useState<boolean>(false);
    const [isDialogVisible, setDialogVisible] = useState<boolean>(false);
    const [isDialogCommentVisible, setDialogCommentVisible] = useState<boolean>(false);
    const [comment, setComment] = useState('');
    const [comments, setComments] = useState<Array<any>>([]);
    const [time, setTime] = useState<any>('');
    const [timeEdited, setTimeEdited]= useState<any>('');
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('')
    const [startTime, setStartTime] = useState<any>();
    const [changeTitle, setChangeTitle] = useState('');
    const [changeContent, setChangeContent] = useState('');
    const [commentDelete, setCommentDelete] = useState<number>(0);
    
    const [previewDialog, setPreviewDialog] = useState<boolean>(false);
    useEffect(() => {
        const checkDel = () =>{
            if(info.authorId === storageService.getUserInfo()?.id)
                setDel(false)
        }
        
        setTime(returnDate(info.timeStamp));
        if(info.editHistory.length > 0 && info.editHistory[0].dateCreated !== undefined)
            setTimeEdited(returnDate(info.editHistory[0].dateCreated));
        checkDel();
    }, [del])

    useEffect(() => {
        const handleComments = (newComm : CommentModel) => {
            setComments(comments => [newComm, ...comments]);
        };

        messageEmitter.on('addComment', handleComments);

        return () => {
            messageEmitter.off('addComment', handleComments);
        };
    }, [comments]);
    function returnDate(timeStamp: Date){
        const messageDate = new Date(timeStamp);
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

        return result;
    }
    const accept = async () =>{
        setLoader(true)
        const response = await postService.deletePost(info.postId);
        if(response.status){
            if(location.pathname.includes("post"))
                navigate("/predmet/"+info.subjectId+"/wiki"); 
        }
        else{
            //napisi nesto za gresku
        }
    }

    const acceptComment = async () =>{
        console.log("Aa");
        setLoader(true)
        const response = await commentService.deleteComment(commentDelete, info.postId)
        if(response.status)
            navigate(0);
        else
            console.log("doslo je do greske");
    }

    const editPost = () => {
        setStartTime(new Date());
        setTitle(info.title);
        setContent(info.content);
        setDialog2Visible(true);
    };

    function deleteComment(commentId: any){
        setCommentDelete(commentId);
        setDialogCommentVisible(true);
    }
    async function sendToEdit(){
        let time = returnDate(startTime);
        if(time === "sada")
            time = "1 minut"
        else
            time = time.split(' ').slice(1).join(' ');
        socketService.editPostNotification(time, info.postId, title, content, Number(storageService.getUserInfo()?.id), info.subjectId);
        navigate(0);
    }
    const confirmDelete = () => {
        setDialogVisible(true);
    };
    const handleReject = () => {
        setDialogVisible(false);
    };
    const handleRejectComment = () => {
        setDialogCommentVisible(false);
    }
    function showUser(e: any){
        navigate("/profilna-strana/"+e);
    }
    const headerTemplate = (options : any) => {
        const className = `${options.className} justify-content-space-between`;
        return (
            <div className={className}>
                {info.authorId !== storageService.getUserInfo()?.id ? <a href={"/profilna-strana/"+ info.authorId}>
                <div className="d-flex align-items-center gap-2" onClick={() => showUser(info.authorId)}>
                    <img alt={info.photoPath} src={enviorment.port + info.photoPath} style={{ width: '5vh', height: "5vh", borderRadius: "50%"}}/>
                    <span className="font-bold">{info.authorName}</span>
                </div>
                </a> :
                <div className="d-flex align-items-center gap-2">
                    <img alt={info.photoPath} src={enviorment.port + info.photoPath} style={{ width: '5vh', height: "5vh", borderRadius: "50%"}}/>
                    <span className="font-bold">{info.authorName}</span>
                </div>    
                }
                <div>
                    {info.allowed && location.pathname.includes("post") && <Button onClick={(e) => {e.stopPropagation(); editPost();}} className="izmeni" icon="pi pi-pencil" rounded severity="danger" aria-label="Izmeni" />}
                    {!del && location.pathname.includes("post") && <Button onClick={(e) => {e.stopPropagation(); confirmDelete();}} className="izmeni" icon="pi pi-trash" rounded severity="danger" aria-label="Obriši" />}
                </div>
            </div>
        );
    };

    async function insertComment(){
        socketService.sendNotificationForComment(info.postId, Number(storageService.getUserInfo()?.id), comment);
        setComment("");
    }

    function toggleVisible(){
        setVisible(!visible);
    }
    function navigateToPostPage(){
        navigate("/post/"+info.postId);
    }
    async function getComments(){
        setLoader(true);
        const response = await commentService.getComments(info.postId);
        if(response.status){
            setComments(response.data);
        }
        else
            console.log("greska neka");
        toggleVisible();
    }
    const footerTemplate = (options : any) => {
        return (
            <div className={options.className} style={{width: "100%", borderBottomLeftRadius: "0.5rem", borderBottomRightRadius: "0.5rem" }}>
                <div className='d-flex justify-content-between' >
                    <InputText value={comment} onChange={(e) => setComment(e.target.value)} placeholder='Unesi komentar' style={{width:"100%"}}/>
                    <Button onClick={insertComment} style={{border: 0}} icon="pi pi-check"></Button>
                </div >
                <div style={{marginTop: "1vh" }}>
                    {!visible && <p className='comments' style={{fontSize: "1rem", width: "fit-content"}} onClick={getComments}>Prikaži komentare</p>}
                    {visible && <p className='comments' style={{fontSize: "1rem", width: "fit-content"}} onClick={toggleVisible}>Sakrij komentare</p>}
                    {visible ? comments.length>0 ? comments?.map(c => (<Panel style={{marginBottom: "2vh"}} 
                    header={<>
                            <div style={{display: "flex", alignItems: "center", justifyContent: "space-between"}} className=''>
                                <div style={{ display: "flex", alignItems: "center" }}><img alt={c.photoPath} src={enviorment.port + c.photoPath} style={{ width: '5vh', height: "5vh", borderRadius: "50%"}}/>
                                {storageService.getUserInfo()?.id !== c.userId ?  <a href={'/profilna-strana/' + c.userId}>
                                        <span style={{marginLeft: "1vh" }}>{c.userName}</span>
                                </a> : <span style={{marginLeft: "1vh" }}>{c.userName}</span>} 
                                </div>
                                <ConfirmDialog visible={isDialogCommentVisible}onHide={handleRejectComment}message="Da li ste sigurni da želite da obrišete komentar?"header="Obriši komentar"icon="pi pi-info-circle"acceptClassName="p-button-danger"rejectLabel="Odustani"acceptLabel="Potvrdi" accept={() => acceptComment()}reject={handleRejectComment}/>
                                {c.userId === storageService.getUserInfo()?.id && <Button onClick={() => deleteComment(c.id)} style={{border: 0}} icon="pi pi-trash"></Button>}
                            </div></>}>
                        <p style={{fontSize: "3vh", color: "white"}} className="m-0">
                            {c.content}
                        </p>
                        <span className="p-text-secondary">{returnDate(c.timeStamp)}</span>
                    </Panel>)) : <p style={{color: "white"}}>Nema komentara na ovom postu</p> : <></>}
                </div>
            </div>
        );
    };
    function showChanges(title : any, content: any){
        setChangeContent(content);
        setChangeTitle(title);
        setPreviewDialog(true);
    }
    return (<>

        {previewDialog && <Dialog visible={previewDialog} header={"Istorija izmena"} onHide={() => {if (!previewDialog) return; setPreviewDialog(false); }}
            style={{ width: '70vw', textAlign: "center", height: "90vh"}}>
                <div className="d-flex align-items-center flex-column">
                    <div className="d-flex justify-content-center flex-column">
                        <h2 style={{color: "white", marginBottom: "0.3rem"}}>{changeTitle}</h2>
                        <span style={{marginBottom: "0.3rem"}} id="description">{changeContent}</span>
                    </div>
                </div>
        </Dialog>}

        {dialog2Visible && <Dialog visible={dialog2Visible} header={"Izmeni post"} onHide={() => {if (!dialog2Visible) return; setDialog2Visible(false); }}
            style={{ width: '70vw', textAlign: "center", height: "90vh"}}>
                <div className="d-flex align-items-center flex-column">
                    <div className="d-flex justify-content-center flex-column">
                        <InputText style={{marginBottom: "0.3rem"}} placeholder="Naslov" value={title} onChange={(e) => setTitle(e.target.value)} />
                        <InputTextarea style={{marginBottom: "0.3rem"}} placeholder="Post" id="description" value={content} onChange={(e) => setContent(e.target.value)} rows={12} cols={80} />
                    </div>
                    <div>
                        <Button onClick={() => sendToEdit()}  label="Izmeni" icon="pi pi-send" iconPos="right" style={{marginRight: "1vh"}}/>
                    </div>
                </div>
        </Dialog>}
        
        <ConfirmDialog visible={isDialogVisible}onHide={handleReject}message="Da li ste sigurni da želite da obrišete post?"header="Obriši post"icon="pi pi-info-circle"acceptClassName="p-button-danger"rejectLabel="Odustani"acceptLabel="Potvrdi"accept={accept}reject={handleReject}/>
        <Panel  header="Header" headerTemplate={headerTemplate} footerTemplate={footerTemplate} toggleable>
            <div className='post' onClick={navigateToPostPage} >
                <h1 style={{color: "white"}}>{info.title}</h1>
                <p style={{color: "white"}}>
                    {info.content}
                </p>
            </div>
            <div className='d-flex flex-column'>
            <span className="p-text-secondary">{"kreirano " +time}</span>
            {info.editHistory?.length > 0 && info.editHistory?.map((e : any) => (
                <div><span className="p-text-secondary">{"izmenjeno " + returnDate(e.dateCreated)==="sada" ? "1 minut" : returnDate(e.dateCreated) + " od "}</span><a style={{textDecoration: "underline"}} href={"/profile-page/"+e.editorId}>{e.editorName}</a>.<span className="p-text-secondary">Trajanje izmene {e.time}. </span><span className="change p-text-secondary" style={{fontWeight: "bold", textDecoration: "underline"}} onClick={() => showChanges(e.title, e.content)}>Pogledaj post pre izmene.</span></div>
            ))}
            </div>
        </Panel>
        </>
    )
}
 
export default PostPanel;