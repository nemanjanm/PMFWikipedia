export interface CommentModel {
    id: number,
    content: string,
    userId: number,
    userName: string,
    photoPath: string,
    timeStamp: Date
}