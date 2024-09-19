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
import { useLocation, useNavigate, useParams } from "react-router-dom";
import { socketService } from "../services/SocketService";
import { InputText } from "primereact/inputtext";
import { Dropdown } from "primereact/dropdown";
import { SubjectModel } from "../models/SubjectModel";
function SubjectsPage(){

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
    const params = useParams();
    
    const programNumber = Number(params.id);
    const program = getName(programNumber);
    const [users, setUsers] = useState<Array<LoginInfo>>();
    const [previewUser, setPreviewUser] = useState<LoginInfo>();
    const [temp, setTemp] = useState(false)
    const [visible, setVisible] = useState<boolean>(false)
    const [visible2, setVisible2] = useState<boolean>(false)
    const [loader, setLoader] = useState<boolean>(false);
    const [selectedYear, setSelectedYear] = useState<string>("");
    const [selectedUser, setSelectedUser] = useState<string>("");
    const [subjects, setSubjects] = useState<Array<SubjectInfo>>();
    const [tree, setTree] = useState<Array<any>>();
    const [name, setName] = useState<any>();
    const [year, setYear] = useState<any>();
    const [semester, setSemester] = useState<any>();

    const years = [
        { value: 1},
        { value: 2},
        { value: 3},
        { value: 4},
        { value: 5}
    ];

    const semesters = [
        { value: 1},
        { value: 2}
    ];
    const navigate = useNavigate();

    useEffect(() => { 
        async function getUsers(){
            setLoader(true);
            const response = await userService.getAllUsers(Number(params.id));
            if(response.status)
            {
                setUsers(response.data);
            }

            const response2 = await subjectService.getSubjects(Number(params.id));
            if(response.status){
                setSubjects(response2.data);
                setTree(TreeHelper(response2.data));
            }

            setLoader(false);
        }
        getUsers()
    }, [params]);
    
    function handleUser(e: any){
        setPreviewUser(e);
        setVisible(true);
    }

    function showProfile(e: any){
        navigate("/profilna-strana/"+e.id);
    }

    function sendMessage(e: any)
    {
        navigate("/poruke-1/", {state: e});
    }

    async function addSubject(){

        const model: SubjectModel={
            programId: Number(params.id),
            name: name,
            year: year,
            semester: semester
        }
        const response = await subjectService.addSubject(model)
        if(response.status)
            navigate(0);
    }
    function handleVisible(){
        setVisible2(true);
    }
    const userTemplate = (option: LoginInfo) => {
        return (
            <div className="d-flex align-items-center">
                <img alt={option.photoPath} src={enviorment.port + option.photoPath} style={{ width: '5vh', height: "5vh", marginRight: '1rem', borderRadius: "50%"}}/>
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
                    <div>
                        <h1 style={{textAlign: "center", fontWeight: "bold", margin: "1vh", width: "100%", height: "auto"}}>{program}</h1>
                    </div>
                    <div className="d-flex justify-content-around" style={{width: "100%", height: "auto", flexGrow: 1}}>
                        <div className="d-flex flex-column" style={{width: "40vw"}}>
                            <h2 style={{textAlign: "center", fontWeight: "bold"}}>Predmeti</h2>
                            {storageService.getUserInfo()?.firstName === "Admin" && <Button onClick={handleVisible} label="Dodaj predmet"></Button>}
                            <Tree style={{maxHeight: "70vh", overflowY: "scroll", fontSize: "3vh"}} value={tree} filterPlaceholder="Lenient Filter" className="w-full md:w-14rem" />
                        </div>
                        <div style={{width: "5vw"}}></div>
                        <div style={{width: "25vw", position: "relative", overflow: 'inherit', top: 0, bottom: 0}}>
                            <h2 style={{textAlign: "center", fontWeight: "bold"}}>Kolege</h2>
                            <ListBox filter emptyFilterMessage={"Nema rezultata"} emptyMessage={"Nema rezultata"} itemTemplate={userTemplate} value={selectedUser} onChange={(e: ListBoxChangeEvent) => handleUser(e.value)} options={users} optionLabel="fullName" listStyle={{maxHeight: "50vh", fontSize: "3vh"}}/>
                        </div>
                            {visible && <Dialog visible={visible} header={previewUser?.fullName + ""} onHide={() => {if (!visible) return; setVisible(false); }}
                            style={{ width: '50vw', textAlign: "center"}}>
                                <div className="d-flex align-items-center flex-column">
                                    <img src={enviorment.port + previewUser?.photoPath} style={{borderRadius: "50%", height: "40vh", width: "40vh", marginBottom: "2vh"}}></img>
                                    <div>
                                        <Button onClick={() => sendMessage(previewUser)}  label="Pošalji poruku" icon="pi pi-send" iconPos="right" style={{marginRight: "1vh"}}/>
                                        <Button onClick={() => showProfile(previewUser)} label="Prikaži profil" icon="pi pi-user" iconPos="right" style={{marginLeft: "1vh"}}/>
                                    </div>
                                </div>
                        </Dialog>}
                        {visible2 && <Dialog headerStyle={{textAlign: "center"}} visible={visible2} header={"Dodaj predmet"} onHide={() => {if (!visible2) return; setVisible2(false); }}
                            style={{ width: '50vw', textAlign: "center", height: "70vh"}}>
                                <div className="d-flex align-items-center flex-column">
                                    <div style={{width: "20vw"}} className="d-flex justify-content-center flex-column">
                                        <InputText style={{marginBottom: "0.3rem"}} placeholder="Naziv predmeta" value={name} onChange={(e) => setName(e.target.value)} />
                                        <span style={{textAlign: "left"}}>godina</span>
                                        <Dropdown style={{textAlign: "left"}} value={year} onChange={(e) => setYear(e.value)} options={years} optionLabel="value" placeholder="Izaberi godinu" className="w-full md:w-14rem" />
                                        <span style={{textAlign: "left"}}>semestar</span>
                                        <Dropdown style={{textAlign: "left"}} value={semester} onChange={(e) => setSemester(e.value)} options={semesters} optionLabel="value" placeholder="Izaberi semestar" className="w-full md:w-14rem" />
                                    </div>
                                    <div>
                                        <Button onClick={() => addSubject()}  label="Potvrdi" icon="pi pi-send" iconPos="right" style={{border: 0, borderRadius: "10%", marginTop: "5vh"}}/>
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