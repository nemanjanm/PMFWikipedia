import { useNavigate } from "react-router-dom"
import { programmes } from "../models/Programme";
import { storageService } from "../services/StorageService";
import { Dialog } from "primereact/dialog";
import { InputText } from "primereact/inputtext";
import { FileUpload } from "primereact/fileupload";
import { Button } from "primereact/button";
import { useState } from "react";

export function PanelHelper()
{   
    const navigate = useNavigate();
    const [visible, setVisible] = useState<boolean>(true)
    const [title, setTitle] = useState<string>();
    function addProgram(){

    }

    const items : any = [];
    programmes.forEach((p: any, index: number) => {
        const item = {label: p.name, id: p.id, icon: "pi pi-book", command: () => {
            navigate(`/predmeti/${p.id}`);
        }}
        if(p.id !== storageService.getUserInfo()?.program)
            items.push(item);
    })
    return items;

}