export interface MessageInfo{
    chatId: number;
    id: number;
    content: string;
    isRead: boolean;
    timeStamp: Date;
    senderId: number;
}