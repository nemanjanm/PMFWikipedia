import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import ClipLoader from "react-spinners/ClipLoader";
import { useParams } from "react-router-dom";
import SubjectNavBar from "../components/SubjectNavBar/SubjectNavBar";
import { subjectService } from "../services/SubjectService";
import { SubjectInfo } from "../models/SubjectInfo";
import PostPanel from "../components/PostPanel/PostPanel";

function SubjectPage(){

    const [loader, setLoader] = useState<boolean>(false);
    const params = useParams();
    const [subject, setSubject] = useState<SubjectInfo>();
    const [pom, setPom] = useState();

    useEffect(() => {
        async function getSubject(){
            setLoader(true);
            const response = await subjectService.getSubject(Number(params.id));
            if(response.status)
            {
                setSubject(response.data)
            }
            setLoader(false);
        }
        getSubject();
    }, [pom])
    return <><div className="celina" style={{height: "100vh", display: "flex", flexDirection: "column", overflow: "hidden"}}>
    <NavBar></NavBar>
    <div className="d-flex justify-content-center" style={{height: "100vh", overflow: "hidden", display: "flex"}}>
        <SideBar></SideBar>
            {!loader && <div className="sredina" style={{width: "100%", overflowY: "scroll", overflowX: "hidden"}}>
                <h2 style={{textAlign: "center", fontWeight: "bold", margin: "1vh", width: "100%", height: "auto"}}>{subject?.name}</h2>
                <div style={{marginBottom: "1vh"}}>
                    <SubjectNavBar></SubjectNavBar>
                </div>
                <div className="d-flex align-items-center justify-content-center flex-column" style={{width: "100%"}}>
                    <div style={{width: "80%", marginBottom: "1vh"}}><PostPanel></PostPanel></div>
                    <div style={{width: "80%"}}><PostPanel></PostPanel></div>
                    <div style={{width: "80%"}}><PostPanel></PostPanel></div>
                    <div style={{width: "80%"}}><PostPanel></PostPanel></div>
                    <div style={{width: "80%"}}><PostPanel></PostPanel></div>
                    <div style={{width: "80%"}}><PostPanel></PostPanel></div>
                </div>
            </div>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
        <div>
        </div>
    </div>
    </div></>
}

export default SubjectPage;