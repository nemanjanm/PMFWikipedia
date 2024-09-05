import ClipLoader from "react-spinners/ClipLoader";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { useEffect, useState } from "react";
import { PostViewModel } from "../models/PostViewModel";
import { postService } from "../services/PostService";
import { useParams } from "react-router-dom";
import PostPanel from "../components/PostPanel/PostPanel";

function SinglePostPage(){
    const [loader, setLoader] = useState<boolean>(false);
    const [post, setPost] = useState<PostViewModel>();
    const [temp, setTemp] = useState(false)
    const params = useParams();

    useEffect(() => {  
        async function getPost(){
            setLoader(true);
            const response = await postService.getPost(Number(params.id));
            if(response.status)
                setPost(response.data);
            setLoader(false);
        }
        getPost()
    }, [temp])

    return <>
    {!loader && <div className="celina" style={{ display: "flex", flexDirection: "column", height: "100vh" }}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{flex: 1}}>
            <SideBar></SideBar>
            {post &&
                <div style={{width: "80%", margin: "1vh"}}><PostPanel info={post}></PostPanel></div>
            }
            <div></div>
        </div>
        </div>}
    {loader && <div className="d-flex justify-content-center" style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
    </>
}

export default SinglePostPage;