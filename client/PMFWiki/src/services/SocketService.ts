import { HubConnectionBuilder, LogLevel, HubConnection } from "@microsoft/signalr";
import { storageService } from "./StorageService";

class SocketService{
    
    private conn: HubConnection;

    constructor() {
        this.conn = new HubConnectionBuilder().withUrl("https://localhost:7101/chat").configureLogging(LogLevel.Information).build();
    }
    async ConnectToHub(){
        
        //da proverim ako je moj id, onda se samo setuje connId
        this.conn.on("JoinSPecificChatRoom", (connId, myId) => {
            console.log(connId + myId);
            if(myId == storageService.getUserInfo()?.id)
                storageService.setConnId(connId);
        });

        this.conn.on("ReceiveSpecificMessage", (username, msg) => {
            console.log(username);
            console.log(msg);
        });

        await this.conn.start();
        await this.conn.invoke("JoinSPecificChatRoom", {username: storageService.getUserInfo()?.fullName, chatroom: "aaa", myid: storageService.getUserInfo()?.id, secondid: "aaa", message: "IDE GAS"});
    }                                                        

    async sendMessage(message : any, secondid: any){
        await this.conn.invoke("SendMessage",  {message, secondid});
    }

    async reconnect() {
        if (this.conn.state !== "Connected") {
            await this.ConnectToHub();
        }
    }
}

export const socketService = new SocketService();