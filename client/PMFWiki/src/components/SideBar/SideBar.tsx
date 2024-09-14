import { PanelMenu } from 'primereact/panelmenu';
import { Outlet } from 'react-router-dom';
import { useNavigate } from "react-router-dom";
import './SideBar.css'
import { storageService } from '../../services/StorageService';
import { getName } from '../../models/Programme';
import { getNotName } from '../../models/Notification';
import { PanelHelper } from '../PanelHelper';
import { Badge } from 'primereact/badge';
import { useEffect, useState } from 'react';
import { messageEmitter } from '../../services/EventEmmiter';
import { Sidebar } from 'primereact/sidebar';
import { chatService } from '../../services/ChatService';
import { notificationService } from '../../services/NotificationService';
import { ListBox } from 'primereact/listbox';
import { NotificationModel } from '../../models/NotificationModel';
import { postService } from '../../services/PostService';

function SideBar(){
    const navigate = useNavigate();
    const programNumber = storageService.getUserInfo()?.program;
    const [visible, setVisible] = useState(false);
    const [unreadMessages, setUnreadMessages] = useState<number>(0);
    const [unreadNotifications, setUnreadNotifications] = useState<number>(0);
    const [notifications, setNotifications] = useState<Array<NotificationModel>>([]);
    const [temp, setTemp] = useState();

    useEffect(() => {
        async function getChats(){
            const response = await chatService.getUnreadMessages();
            if(response.status){
                setUnreadMessages(response.data);
                const response2 = await notificationService.getNotifications();
                if(response2.status){
                    setUnreadNotifications(response2.data);
                }
            }
        }
        getChats();
    },[temp])
    
    useEffect(() => {
        async function getNotification(){
            const response = await chatService.getUnreadMessages();
            if(response.status){
                setUnreadMessages(response.data);
            }
        }
        getNotification();
    },[temp])

    useEffect(() => {
        const handleNewMessage = () => {
            setUnreadMessages( unreadMessages + 1 );

        };

        messageEmitter.on('increaseMessage', handleNewMessage);

        return () => {
            messageEmitter.off('increaseMessage', handleNewMessage);
        };
    }, [unreadMessages]);

    useEffect(() => {
        const handleNewMessage = (number: number) => {
            setUnreadMessages( unreadMessages - number );
        };

        messageEmitter.on('decreaseMessages', handleNewMessage);

        return () => {
            messageEmitter.off('decreaseMessages', handleNewMessage);
        };
    }, [unreadMessages]);

    useEffect(() => {
        const handleNotification = () => {
            setUnreadNotifications( unreadNotifications + 1);
        };

        messageEmitter.on('increaseNotification', handleNotification);

        return () => {
            messageEmitter.off('increaseNotification', handleNotification);
        };
    }, [unreadNotifications]);

    const program = getName(programNumber);

    function returnDate(timeStamp: Date){
        const messageDate = new Date(timeStamp);
        const thisDate = new Date();
        let result : string;
        const diffInMilliseconds: number = thisDate.getTime() - messageDate.getTime();
        const diffInMinutes: number = Math.floor(diffInMilliseconds / 1000 / 60);
        const diffInHours: number = Math.floor(diffInMilliseconds / 1000 / 60 / 60);
        const diffInDays: number = Math.floor(diffInMilliseconds / 1000 / 60 / 60 / 24);
        if(diffInMinutes < 1)
            result = "sada";
        else if(diffInMinutes < 60)
            result = "pre " + diffInMinutes.toString() + " minuta";
        else if(diffInHours < 24 && diffInHours >= 1)
            result = "pre " + diffInHours.toString() + " sati";
        else if(diffInDays >=1 && diffInDays <= 5)
            result = "pre " + diffInDays.toString() + " dana";
        else
            result = messageDate.toLocaleDateString();

        return result;
    }
    const items  = [
        {
            label: program,
            icon: 'pi pi-book',
            command: () => {
                navigate(`/predmeti/${storageService.getUserInfo()?.program}`);
            },
        },
        {
            label: <>{'Obaveštenja'} <Badge severity="danger" value={unreadNotifications} /></>,
            icon: 'pi pi-bell',
            command: async () => {
                const response = await notificationService.getAllNotifications();
                if(response.status)
                    setNotifications(response.data);
                setVisible(true);
            }
        },
        {
            label: <>{'Poruke'} <Badge severity="danger" value={unreadMessages} /></>,
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

    async function showPost(e: any){
        const response = await notificationService.setIsRead(e.id);
        if(response.status){
            if(e.notificationId === 1 || e.notificationId === 7){
                navigate("/predmet/"+e.subjectId+"/kolokvijum");
                window.location.reload();
            }
            else if(e.notificationId === 2 || e.notificationId === 8){
                navigate("/predmet/"+e.subjectId+"/ispit");
                window.location.reload();
            }
            else{
                navigate("/post/"+e.postId);
                window.location.reload();
            }
            
        }
        else
            console.log("Greska neka") //IZMENI MOZDA!
    }
    return (
        <>
            {visible && 
                <Sidebar className='sideBar' visible={visible} onHide={() => setVisible(false)}>
                    <h2 style={{color: 'white'}}>Obaveštenja</h2>
                    <ListBox emptyMessage={"Nema obaveštenja"} style={{border: 0}} onChange={(e : any) => showPost(e.value)} options={notifications} itemTemplate={(option: any) => ( <><div className={option.isRead ? 'read-notification' : 'unread-notification'}>{option.authorName} {option.notificationId === 6 ? "je izmenio" : "je dodao"} {getNotName(option.notificationId)} {option.title!==undefined ? option.title : ""} u {option.subjectName}</div><span style={{fontSize: "2vh"}}>{returnDate(option.timeStamp)}</span></>)} className="w-full md:w-14rem" />
                </Sidebar>}
            <div className='d-flex justify-content-centar' style={{flexShrink: 0,  position: "relative", overflow: 'auto', top: 0, bottom: 0, height: "auto", backgroundColor: "#374151"}}>
                <PanelMenu style={{height:"100%", width:"100%", border: 0, borderRadius: "0", padding: "0", fontSize: "1vw"}} model={items}/>    
            </div>
        </>
    )
}

export default SideBar;