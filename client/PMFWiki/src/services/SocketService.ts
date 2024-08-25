import { HubConnectionBuilder, LogLevel, HubConnection } from "@microsoft/signalr";
import { storageService } from "./StorageService";
import Messages from "../pages/Messages";
import { MessageInfo } from "../models/MessageInfo";
import { messageEmitter } from "./EventEmmiter";
class SocketService{
    
    private conn: HubConnection;

    constructor() {
        this.conn = new HubConnectionBuilder().withUrl("https://localhost:7101/chat").configureLogging(LogLevel.Information).build();
    }
    async ConnectToHub(){
        
        this.conn.on("JoinSPecificChatRoom", (connId, myId) => {
            if(myId == storageService.getUserInfo()?.id)
                storageService.setConnId(connId);
        });

        this.conn.on("ReceiveSpecificMessage", (chatviewmodel) => {
            if(chatviewmodel.data.chatId !== undefined)
            {
                const newmessage : MessageInfo = {
                    content: chatviewmodel.data.content,
                    senderId: chatviewmodel.data.senderId,
                    timeStamp: chatviewmodel.data.timeStamp,
                    chatId: chatviewmodel.data["chatId"],
                    id: chatviewmodel.data.id,
                    isRead: chatviewmodel.data.isRead
                }
                messageEmitter.emit('newMessage', newmessage);
                messageEmitter.emit('increaseMessage', 1);
            }
        });

        await this.conn.start();
        await this.conn.invoke("JoinSPecificChatRoom", {username: storageService.getUserInfo()?.fullName, chatroom: "aaa", myid: storageService.getUserInfo()?.id, secondid: 1, message: "IDE GAS"});
    }                                                        

    async sendMessage(message : any, secondid: any, myid: any){
        await this.conn.invoke("SendMessage",  {message, secondid, myid});
    }

    async deleteConnection(myid: any){
        await this.conn.invoke("DeleteConnId", {myid});
        storageService.deleteConnectionId();
    }
    async reconnect() {
        if (this.conn.state !== "Connected") {
            await this.ConnectToHub();
        }
    }
}

export const socketService = new SocketService();