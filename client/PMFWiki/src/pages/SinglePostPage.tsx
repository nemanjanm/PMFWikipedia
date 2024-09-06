import ClipLoader from "react-spinners/ClipLoader";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useEffect, useState } from "react";
import { PostViewModel } from "../models/PostViewModel";
import { postService } from "../services/PostService";
import { useNavigate, useParams } from "react-router-dom";
import PostPanel from "../components/PostPanel/PostPanel";
import { socketService } from "../services/SocketService";
import { storageService } from "../services/StorageService";

function SinglePostPage(){
    const [loader, setLoader] = useState<boolean>(false);
    const [post, setPost] = useState<PostViewModel>();
    const [temp, setTemp] = useState(false)
    const [obrisan, setObrisan] = useState(false);
    const navigate = useNavigate();
    const params = useParams();

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
        async function getPost(){
            setLoader(true);
            const response = await postService.getPost(Number(params.id));
            if(response.status)
                setPost(response.data);
            else{
                setObrisan(true);
                setTimeout( () => {
                    navigate("/pocetna")} , 2000
                );
            }
            setLoader(false);
        }
        getPost()
    }, [temp])

    return <>
    {!loader && <div className="celina" style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{flex: 1}}>
            <SideBar></SideBar>
            {post!==undefined &&
                <div style={{width: "80%", margin: "1vh"}}><PostPanel info={post}></PostPanel></div>
            }
            {obrisan &&
                <p className="d-flex justify-content-center">Došlo je do greške</p>
            }
            <div></div>
        </div>
        </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default SinglePostPage;