export interface NotificationModel {
    id: number,
    authorName: string,
    subjectName: string,
    subjectId: number,
    postId: number,
    isRead: boolean,
    notificationId: number,
    timeStamp: Date;
}