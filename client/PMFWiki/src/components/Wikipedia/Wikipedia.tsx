import { useEffect, useState } from "react";
import { SubjectInfo } from "../../models/SubjectInfo";
import { subjectService } from "../../services/SubjectService";
import PostPanel from "../PostPanel/PostPanel";
import { Outlet, useNavigate } from "react-router-dom";
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { InputText } from "primereact/inputtext";
import { InputTextarea } from "primereact/inputtextarea";
import { postService } from "../../services/PostService";
import { PostInfo } from "../../models/PostInfo";
import { storageService } from "../../services/StorageService";
import { PostViewModel } from "../../models/PostViewModel";
import { socketService } from "../../services/SocketService";
import ClipLoader from "react-spinners/ClipLoader";

function Wikipedia(){
    const [posts, setPosts] = useState<Array<PostViewModel>>([]);
    const [loader, setLoader] = useState<boolean>(false);
    const [pom, setPom] = useState();
    const [allowed, setAllowed] = useState<boolean>(false);
    const [visible, setVisible] = useState<boolean>(false)
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('')
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
        async function getPosts(){
            setLoader(true);
            const response = await postService.getPosts(Number((location.pathname).split('/').slice(2, -1).join('/')));
            if(response.status)
            {
                if(response.data[0].authorId !== 0)
                    setPosts(response.data)
                if(response.data[0].allowed && response.data[0].allowed === true)
                    setAllowed(true);
            }
            setLoader(false);
        }
        getPosts();
    }, [pom])

    function handleAdd(){
        setVisible(true);
    }

    async function addPost(){
        socketService.sendNotification(title, content, Number(storageService.getUserInfo()?.id), Number((location.pathname).split('/').slice(2, -1).join('/')))
        navigate(0); //proveri bolje resenj, mozda timeout
    }
    return <>{!loader && <div className="d-flex align-items-center justify-content-center flex-column" style={{width: "100%"}}>
        {allowed && <Button label="Dodaj" icon="pi pi-plus" style={{width: "20vw"}} onClick={handleAdd} ></Button>}
        {posts?.length > 0 && posts?.map(s => (
            <div style={{width: "80%", marginBottom: "1vh"}}><PostPanel info={s} subjectId={Number((location.pathname).split('/').slice(2, -1).join('/'))}></PostPanel></div>
        ))}
        {posts?.length === 0 && <p style={{fontSize: "3vw", color: "#374151", textAlign: "center"}}>Trenutno nema postova na ovom predmetu</p>}
        {visible && <Dialog visible={visible} header={"Dodaj novi post"} onHide={() => {if (!visible) return; setVisible(false); }}
            style={{ width: '70vw', textAlign: "center", height: "90vh"}}>
                <div className="d-flex align-items-center flex-column">
                    <div className="d-flex justify-content-center flex-column">
                        <InputText style={{marginBottom: "0.3rem"}} placeholder="Naslov" value={title} onChange={(e) => setTitle(e.target.value)} />
                        <InputTextarea style={{marginBottom: "0.3rem"}} placeholder="Post" id="description" value={content} onChange={(e) => setContent(e.target.value)} rows={12} cols={80} />
                    </div>
                    <div>
                        <Button onClick={() => addPost()}  label="Dodaj" icon="pi pi-send" iconPos="right" style={{marginRight: "1vh"}}/>
                    </div>
                </div>
        </Dialog>}
        <Outlet></Outlet>
    </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default Wikipedia;