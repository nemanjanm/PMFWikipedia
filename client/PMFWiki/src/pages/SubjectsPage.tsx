import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { ClipLoader } from "react-spinners";
import { storageService } from "../services/StorageService";
import { getName } from "../models/Programme";
import { ListBox, ListBoxChangeEvent } from "primereact/listbox";
import { years } from "../models/Years";
import { userService } from "./UserService";
import { LoginInfo } from "../models/LoginInfo";
import { enviorment } from "../enviorment";
import { Tree } from 'primereact/tree';
import { TreeNode } from 'primereact/treenode';
import { Button } from "primereact/button";
import { Dialog } from "primereact/dialog";
import { TreeHelper } from "../components/TreeHelper/TreeHelper";
import "./SubjectPage.css"
import { SubjectInfo } from "../models/SubjectInfo";
import { subjectService } from "../services/SubjectService";
function SubjectsPage(){

    const programNumber = storageService.getUserInfo()?.program;
    const program = getName(programNumber);
    const [users, setUsers] = useState<Array<LoginInfo>>();
    const [previewUser, setPreviewUser] = useState<LoginInfo>();
    const [temp, setTemp] = useState(false)
    const [visible, setVisible] = useState<boolean>(false)
    const [loader, setLoader] = useState<boolean>(false);
    const [selectedYear, setSelectedYear] = useState<string>("");
    const [selectedUser, setSelectedUser] = useState<string>("");
    const [subjects, setSubjects] = useState<Array<SubjectInfo>>();
    const [tree, setTree] = useState<Array<any>>();

    useEffect(() => {  
        async function getUsers(){
            setLoader(true);
            const response = await userService.getAllUsers();
            if(response.status)
            {
                setUsers(response.data);
            }

            const response2 = await subjectService.getSubjects();
            if(response.status){
                setSubjects(response2.data);
                setTree(TreeHelper(response2.data));
            }

            setLoader(false);
        }
        getUsers()
    }, [temp])

    function handleYear(e: any){

    }

    function handleUser(e: any){
        setPreviewUser(e);
        setVisible(true);
    }

    const userTemplate = (option: LoginInfo) => {
        return (
            <div className="d-flex align-items-center">
                <img alt={option.photoPath} src={enviorment.port + option.photoPath} style={{ width: '2rem', marginRight: '1rem', borderRadius: "50%"}}/>
                <div>{option.fullName}</div>
            </div>
        );
    };

    return <>
    <div className="celina" style={{height: ""}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100vh"}}>
            <SideBar></SideBar>
                {!loader && <div>
                    <h1 style={{textAlign: "center", fontWeight: "bold", margin: "10px", width: "100%", height: "auto"}}>{program}</h1>
                    <div className="d-flex justify-content-around" style={{width: "100%", height: "auto"}}>
                        <div style={{width: "70vh"}}>
                            <h2 style={{textAlign: "center", fontWeight: "bold"}}>Predmeti</h2>
                            <Tree style={{maxHeight: "70vh", overflowY: "scroll", fontSize: "2vw"}} value={tree} filterPlaceholder="Lenient Filter" className="w-full md:w-14rem" />
                        </div>
                        <div style={{width: "30vh"}}></div>
                        <div style={{width: "70vh"}}>
                            <h2 style={{textAlign: "center", fontWeight: "bold"}}>Kolege</h2>
                            <ListBox filter itemTemplate={userTemplate} value={selectedUser} onChange={(e: ListBoxChangeEvent) => handleUser(e.value)} options={users} optionLabel="fullName" className="w-full md:w-14rem" listStyle={{maxHeight: "50vh"}}/>
                        </div>
                            {visible && <Dialog visible={visible} header={previewUser?.fullName + ""} onHide={() => {if (!visible) return; setVisible(false); }}
                            style={{ width: '50vw', textAlign: "center"}}>
                                <div className="d-flex align-items-center flex-column">
                                    <img src={enviorment.port + previewUser?.photoPath} style={{borderRadius: "50%", height: "30vh", marginBottom: "2vh"}}></img>
                                    <div>
                                        <Button label="Pošalji poruku" icon="pi pi-send" iconPos="right" style={{marginRight: "1vh"}}/>
                                        <Button label="Prikaži profil" icon="pi pi-user" iconPos="right" style={{marginLeft: "1vh"}}/>
                                    </div>
                                </div>
                        </Dialog>}
                    </div>
                </div>}
                {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#111827" loading={loader} size={150}></ClipLoader></div>}
            <div>
            </div>
        </div>
        </div>
    </>

}

export default SubjectsPage;