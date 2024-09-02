import { PanelMenu } from 'primereact/panelmenu';
import { Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import './SideBar.css'
import { storageService } from '../../services/StorageService';
import { getName, programmes } from '../../models/Programme';
import { PanelHelper } from '../PanelHelper';
import { Badge } from 'primereact/badge';
import { useEffect, useState } from 'react';
import { messageEmitter } from '../../services/EventEmmiter';
import { socketService } from '../../services/SocketService';
import { chatService } from '../../services/ChatService';

function SideBar(){
    const navigate = useNavigate();
    const programNumber = storageService.getUserInfo()?.program;
    const [unread, setUnread] = useState<number>(0);
    const [temp, setTemp] = useState();

    useEffect(() => {
        async function getChats(){
            const response = await chatService.getUnreadMessages();
            if(response.status){
                setUnread(response.data);
            }
        }
        getChats();
    },[temp])
    
    useEffect(() => {
        const handleNewMessage = () => {
            setUnread( unread + 1 );

        };

        messageEmitter.on('increaseMessage', handleNewMessage);

        return () => {
            messageEmitter.off('increaseMessage', handleNewMessage);
        };
    }, [unread]);

    useEffect(() => {
        const handleNewMessage = (number: number) => {
            console.log(number);
            setUnread( unread - number );
        };

        messageEmitter.on('decreaseMessages', handleNewMessage);

        return () => {
            messageEmitter.off('decreaseMessages', handleNewMessage);
        };
    }, [unread]);

    const program = getName(programNumber);

    const items  = [
        {
            label: program,
            icon: 'pi pi-book',
            command: () => {
                navigate(`/predmeti/${storageService.getUserInfo()?.program}`);
            },
        },
        {
            label: <>{'Obaveštenja'} <Badge severity="danger" value={null} /></>,
            icon: 'pi pi-bell',
            command: () => {
                navigate("/")
            }
        },
        {
            label: <>{'Poruke'} <Badge severity="danger" value={unread} /></>,
            icon: 'pi pi-inbox',
            command: () => {
                navigate("/poruke-1")
            }
        },
        {
            label: 'Ostale oblasti',
            items: PanelHelper(),
        }
    ];
    return (
        <>
            <div className='d-flex justify-content-centar' style={{flexShrink: 0,  position: "relative", overflow: 'auto', top: 0, bottom: 0, height: "auto", backgroundColor: "#374151"}}>
                <PanelMenu style={{height:"100%", width:"100%", border: 0, borderRadius: "0", padding: "0", fontSize: "1vw"}} model={items}/>    
            </div>
        </>
    )
}

export default SideBar;