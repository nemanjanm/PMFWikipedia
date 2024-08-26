import { LoginInfo } from "./LoginInfo";

export interface ChatInfo {
    id: number,
    user: LoginInfo,
    timeStamp: Date,
    user1Id: number,
    user2Id: number,
}