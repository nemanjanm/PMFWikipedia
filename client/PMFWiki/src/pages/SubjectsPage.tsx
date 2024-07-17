import { useEffect, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import { ClipLoader } from "react-spinners";
import { storageService } from "../components/StorageService";
import { getName } from "../components/Programme";
import { ListBox, ListBoxChangeEvent } from "primereact/listbox";
import { years } from "../components/Years";
import { userService } from "./UserService";
import { LoginInfo } from "../models/LoginInfo";
import { enviorment } from "../enviorment";

function SubjectsPage(){

    const programNumber = storageService.getUserInfo()?.program;
    const program = getName(programNumber);
    const [users, setUsers] = useState<Array<LoginInfo>>();
    const [temp, setTemp] = useState(false)
    const [loader, setLoader] = useState<boolean>(false);
    const [selectedYear, setSelectedYear] = useState<string>("");
    const [selectedUser, setSelectedUser] = useState<string>("");

    useEffect(() => {  
        async function getUsers(){
            setLoader(true);
            const response = await userService.getAllUsers();
            if(response.status)
                setUsers(response.data);
            setLoader(false);
        }
        getUsers()
    }, [temp])

    function handleYear(e: any){

    }

    function handleUser(e: any){

    }

    const userTemplate = (option: LoginInfo) => {
        return (
            <div className="d-flex align-items-center">
                <img alt={option.photoPath} src={enviorment.port + option.photoPath} style={{ width: '1.25rem', marginRight: '.5rem', borderRadius: "50%"}}/>
                <div>{option.firstName + " " + option.lastName}</div>
            </div>
        );
    };

    return <>
    <div className="celina" style={{height: "fit-content"}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100vh"}}>
            <SideBar></SideBar>
                <div>
                    <h1 style={{textAlign: "center", fontWeight: "bold", margin: "10px"}}>{program}</h1>
                    <div className="d-flex justify-content-around">
                        <div style={{width: "50vh"}}>
                        <h2 style={{textAlign: "center", fontWeight: "bold"}}>Godina studija</h2>
                        <ListBox value={selectedYear} onChange={(e: ListBoxChangeEvent) => handleYear(e)} options={years} optionLabel="year" className="w-full md:w-14rem" listStyle={{maxHeight: "70vh"}}/>
                        </div>
                        <div style={{width: "30vh"}}></div>
                        <div style={{width: "50vh"}}>
                        <h2 style={{textAlign: "center", fontWeight: "bold"}}>Kolege</h2>
                        <ListBox value={selectedUser} itemTemplate={userTemplate} onChange={(e: ListBoxChangeEvent) => handleUser(e)} options={users} optionLabel="email" className="w-full md:w-14rem" listStyle={{maxHeight: "70vh"}}/>
                        </div>
                    </div>
                </div>
            <div>
            </div>
        </div>
        </div>
    </>

}

export default SubjectsPage;