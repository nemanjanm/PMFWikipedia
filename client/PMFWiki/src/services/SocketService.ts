import { HubConnectionBuilder, LogLevel, HubConnection } from "@microsoft/signalr";
import { storageService } from "./StorageService";
import Messages from "../pages/Messages";
import { MessageInfo } from "../models/MessageInfo";
import { messageEmitter } from "./EventEmmiter";
import { LoginInfo } from "../models/LoginInfo";
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

        this.conn.on("MarkMessagesAsRead", (id) => {
            messageEmitter.emit('markAsRead', id);
        })

        this.conn.on("ReceiveNotification", (authorName, subjectName, postId) => {
            messageEmitter.emit("increaseNotification", 1);
        })
        this.conn.on("ReceiveSpecificMessage", (chatviewmodel) => {
            if(chatviewmodel.data.chatId !== undefined)
            {
                if(chatviewmodel.user !== null)
                {
                    const newmessage : MessageInfo = {
                        content: chatviewmodel.data.content,
                        senderId: chatviewmodel.data.senderId,
                        timeStamp: chatviewmodel.data.timeStamp,
                        chatId: chatviewmodel.data["chatId"],
                        id: chatviewmodel.data.id,
                        isRead: chatviewmodel.data.isRead,
                        user: chatviewmodel.data.user,
                        user1Id: chatviewmodel.data.user1Id,
                        user2Id: chatviewmodel.data.user2Id,
                    }
                    messageEmitter.emit('newMessage', newmessage);
                }
                else
                {
                    const newuser : LoginInfo = {
                        id: chatviewmodel.data.user.id,
                        firstName: chatviewmodel.data.user.firstName,
                        lastName: chatviewmodel.data.user.lastName,
                        email: chatviewmodel.data.user.email,
                        program: chatviewmodel.data.user.program,
                        photoPath: chatviewmodel.data.user.photoPath,
                        fullName: chatviewmodel.data.user.fullName,
                    }

                    const newmessage : MessageInfo = {
                        content: chatviewmodel.data.content,
                        senderId: chatviewmodel.data.senderId,
                        timeStamp: chatviewmodel.data.timeStamp,
                        chatId: chatviewmodel.data["chatId"],
                        id: chatviewmodel.data.id,
                        isRead: chatviewmodel.data.isRead,
                        user: newuser,
                        user1Id: chatviewmodel.data.user1Id,
                        user2Id: chatviewmodel.data.user2Id
                    }
                    messageEmitter.emit('newMessage', newmessage);
                }   
                messageEmitter.emit('increaseMessage', 1);
            }
        });

        await this.conn.start();
        await this.conn.invoke("JoinSPecificChatRoom", {username: storageService.getUserInfo()?.fullName, chatroom: "aaa", myid: storageService.getUserInfo()?.id, secondid: 1, message: "IDE GAS"});
    }                                                        

    async sendMessage(message : any, secondid: any, myid: any){
        await this.conn.invoke("SendMessage",  {message, secondid, myid});
    }

    async sendNotification(title : any, content : any, author : any, subject : any){
        await this.conn.invoke("SendNotfication", {title, content, author, subject});
    }
    async markAsRead(id : any, myId: any){
        await this.conn.invoke("MarkAsRead",  {id, myId});
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