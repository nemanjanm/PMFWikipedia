import { useEffect, useRef, useState } from "react";
import NavBar from "../components/NavBar/NavBar";
import SideBar from "../components/SideBar/SideBar";
import "./Messages.css";
import { useLocation } from "react-router-dom";
import { ListBox, ListBoxChangeEvent } from 'primereact/listbox';
import { storageService } from "../services/StorageService";
import { ClipLoader } from "react-spinners";
import { enviorment } from "../enviorment";
import { InputText } from "primereact/inputtext";
import { Button } from "primereact/button";
import { socketService } from "../services/SocketService";
import { chatService } from "../services/ChatService";
import { MessageInfo } from "../models/MessageInfo";
import { ChatInfo } from "../models/ChatInfo";
import { messageEmitter } from "../services/EventEmmiter";
import { Badge } from "primereact/badge";
import { UnreadMessage } from "../models/UnreadMessage";
import { messageService } from "../services/MessageService";
import { ChatIdModel } from "../models/ChatIdModel";

function Messages() {

    const location = useLocation();
    const [user, setUser] = useState(location.state || null);
    const [message, setMessage] = useState<string>();
    const [chats, setChats] = useState<Array<ChatInfo>>();
    const [temp, setTemp] = useState(false)
    const [messages, setMessages] = useState<Array<any>>([]);
    const [currentChatId, setCurrentChatId] = useState<number>(-1);
    const [unread, setUnread] = useState<Array<UnreadMessage>>([]);

    const scrollRef = useRef<any>();
    const buttonRef = useRef<any>();
    
    useEffect(() => {
        const handleNewMessage = (newMessage: MessageInfo) => {
            if(currentChatId === newMessage.chatId){
                setMessages(messages => [...messages, newMessage]);
                const chat = chats?.find(c => c.id === newMessage.chatId)?.id
                handleChat(chat);
            }
            else{
                const chat = chats?.find(c => c.id === newMessage.chatId)?.id
                handleChat(chat);
            }
        };

        messageEmitter.on('newMessage', handleNewMessage);

        return () => {
            messageEmitter.off('newMessage', handleNewMessage);
        };
    }, [currentChatId]);
    
    useEffect(() => {
        if (scrollRef.current) {
            scrollRef.current.scrollTop = scrollRef.current.scrollHeight;
        }
    }, [messages]);

    useEffect(() => {
        const handleKeyDown = (event : any) => {
            if (event.key === 'Enter') {
                buttonRef.current.click();
            }
        };
        window.addEventListener('keydown', handleKeyDown);
        return () => {
            window.removeEventListener('keydown', handleKeyDown);
        };
    }, []);

    useEffect(() => {
        async function getChats(){
            setLoader(true);
            const response = await chatService.getChats();
            if(response.status)
            {
                setChats(response.data);
                if(user === null)
                {
                    setUser(response.data[0].user);
                    setCurrentChatId(response.data[0].id);
                    const response2 = await chatService.getMessages(response.data[0].id);
                    
                    if(response2.status)
                    {
                        setMessages(response2.data);
                    }

                    response.data.forEach((chat : any) => {
                        const unreadMessage : UnreadMessage ={
                            id : chat.id,
                            number: chat.unread
                        }
                        setUnread(unread => [...unread, unreadMessage]);
                    })
                }
                else
                {
                    let temp = -1;
                    response.data.forEach((chat : any) => {
                        if( temp === -1 && ((chat.user1Id == user.id && chat.user2Id == storageService.getUserInfo()?.id) || (chat.user2Id == user.id && chat.user1Id == storageService.getUserInfo()?.id)))
                        {
                            temp = chat.id;
                        }
                        const unreadMessage : UnreadMessage ={
                            id : chat.id,
                            number: chat.unread
                        }
                        setUnread(unread => [...unread, unreadMessage]);
                    })
                    setCurrentChatId(temp);
                    const response2 = await chatService.getMessages(temp);
                    if(response2.status)
                    {
                        setMessages(response2.data);
                    }
                }
            }
            setLoader(false);
        }
        getChats();
    }, [temp]);

    
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

    const [loader, setLoader] = useState<boolean>(false);

    function handleMessage(message: string){
        setMessage(message);
    }

    const sendMessage = async () => {
        if(message!== "" && message!==null && message!== undefined)
        {
            socketService.sendMessage(message, user.id, storageService.getUserInfo()?.id);
            setMessages(messages => [...messages, {content: message, timeStamp: new Date(), senderId: storageService.getUserInfo()?.id}])
            setMessage("");
        }
    };

    const userTemplate = (option: any) => {
        
        const unreadMessagesNumber = unread.find(m=>m.id === option.id)?.number;
        return (
            <div className="d-flex align-items-center">
                <img alt={option.user.photoPath} src={enviorment.port + option.user.photoPath} style={{ width: '7vh', height: "7vh", marginRight: '1rem', borderRadius: "50%"}}/>
                <div>{option.user.fullName}</div>
                <Badge severity="danger" value={unreadMessagesNumber} style={{marginLeft: "1vw"}} />
            </div>
        );
    };

    async function setIsRead()
    {
        setUnread(unread => unread.map(u => u.id === currentChatId ? { ...u, number: 0} : u))
        console.log(unread);
        messageEmitter.emit("decreaseMessages", unread.find(u => u.id === currentChatId)?.number)
        const info : ChatIdModel = {
            id: currentChatId
        }
        const response = messageService.markAsRead(info);
    
    }
    async function handleChat(e: any)
    {
        if(e.user === undefined)
        {
            const chat = chats?.find(c => c.id === e)?.id;            
            setUnread(unread => unread.map(u => u.id === chat ? { ...u, number: u.number + 1} : u))
        }
        else
        {
            setUser(e.user);
            let pom = -1;
            chats?.forEach((chat : any) => {
                if( (chat.user1Id == e.user.id && chat.user2Id == storageService.getUserInfo()?.id) || (chat.user2Id == e.user.id && chat.user1Id == storageService.getUserInfo()?.id))
                {
                    pom = chat.id;
                    return
                }
            })
            setCurrentChatId(pom);
            const response2 = await chatService.getMessages(pom);
            if(response2.status)
            {
                setMessages(response2.data);
            }
        }
    }
    
    return<>
        <div className="celina" style={{display: "flex", flexDirection: "column", height: "100vh", boxSizing: "border-box"}}>
        <NavBar></NavBar>
        <div className="d-flex justify-content-between" style={{height: "100%", overflowY: "hidden"}}>
            <SideBar></SideBar>
            {!loader && <><ListBox filter emptyFilterMessage={"Nema rezultata"} value={"ide gas"} itemTemplate={userTemplate} onChange={(e: ListBoxChangeEvent) => handleChat(e.value)} options={chats} emptyMessage={"Nema rezultata"} optionLabel="user.fullName" className="w-full md:w-14rem" style={{borderRadius: "0", width: "25vw", flexShrink: 0}} listStyle={{fontSize: "3vh"}}/>
            <div style={{flexGrow: 1, display: "flex", justifyContent: "flex-end", flexDirection:"column"}}>
            <div className="d-flex" style={{alignItems: "center", borderBottom: "0.2rem solid #424b57"}}>
                <img src={enviorment.port + user?.photoPath} style={{border: "0", borderRadius: "50%", height: "10vh", width: "10vh", margin: "2vh"}}></img>
                <p style={{textAlign: "center", marginBottom: "0rem"}}>{user?.fullName}</p>
            </div>
            <div onClick={setIsRead} className="center" ref={scrollRef}> 
            {
                messages?.map((message) => {
                    const date = new Date(message.timeStamp);
                    if(message.senderId === storageService.getUserInfo()?.id)
                        return (<div className="message own">
                                    <div className="texts">
                                        <p>{message.content}</p>
                                        <span>{date.toLocaleTimeString() + " " + date.toLocaleDateString()}</span>
                                    </div>
                                </div>)
                    else
                    return (<div className="message">
                        <div className="texts">
                            <p>{message.content}</p>
                            <span>{date.toLocaleTimeString() + " " + date.toLocaleDateString()}</span>
                        </div>
                    </div>)
                })
            }
            </div>
            <div style={{marginTop: "auto", display: "flex", justifyContent: "flex-end", borderRadius: "0", width:"100%"}}>
                <InputText onMouseDown={setIsRead} value={message} onChange={(e) => handleMessage(e.target.value)} style={{width:"10vw", flex: 1, border: "0.2rem solid #424b57", borderRight: "0", borderRadius: "0", color:"#ffffffde"}} type="text" className="p-inputtext-lg" placeholder="Unesi poruku"  />
                <Button ref={buttonRef} onClick={sendMessage} style={{border: "0.2rem solid #424b57", borderRadius: "0", color:"#ffffffde"}}>Pošalji</Button>
            </div>
            </div> </>}
            {loader && <div style={{marginTop: "50px"}}><ClipLoader color="#374151" loading={loader} size={150}></ClipLoader></div>}
            <div></div>
        </div>
        </div>
    </>
}

export default Messages;