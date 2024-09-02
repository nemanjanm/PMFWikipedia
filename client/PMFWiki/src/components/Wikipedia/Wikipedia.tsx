import { useEffect, useState } from "react";
import { SubjectInfo } from "../../models/SubjectInfo";
import { subjectService } from "../../services/SubjectService";
import PostPanel from "../PostPanel/PostPanel";
import { Outlet } from "react-router-dom";
import { Button } from "primereact/button";

function Wikipedia(){
    const [subjects, setSubjects] = useState<Array<SubjectInfo>>();
    const [loader, setLoader] = useState<boolean>(false);
    const [pom, setPom] = useState();
    
    useEffect(() => {
        async function getSubjects(){
            setLoader(true);
            const response = await subjectService.getSubjects(Number(1));
            if(response.status)
            {
                setSubjects(response.data)
            }
            setLoader(false);
        }
        getSubjects();
    }, [pom])

    return <div className="d-flex align-items-center justify-content-center flex-column" style={{width: "100%"}}>
        <Button label="Dodaj" icon="pi pi-plus" style={{width: "20vw"}}></Button>
        {subjects?.map(s => (
            <div style={{width: "80%", marginBottom: "1vh"}}><PostPanel info={s}></PostPanel></div>
        ))}
        <Outlet></Outlet>
    </div>
}

export default Wikipedia;