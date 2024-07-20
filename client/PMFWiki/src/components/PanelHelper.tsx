import { useNavigate } from "react-router-dom"
import { programmes } from "../models/Programme";
import { storageService } from "../services/StorageService";

export function PanelHelper()
{   
    const navigate = useNavigate();

    const items : any = [];
    programmes.forEach((p: any, index: number) => {
        const item = {label: p.name, id: p.id, icon: "pi pi-book", command: () => {
            navigate("/profilna-strana")
        }}
        if(p.id !== storageService.getUserInfo()?.program)
            items.push(item);
    })
    const lastItem = {label: "Dodaj oblast", icon: "pi pi-plus"}
    items.push(lastItem);
    return items;

}