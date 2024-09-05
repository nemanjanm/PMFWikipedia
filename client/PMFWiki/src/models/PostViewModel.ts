export interface PostViewModel{
    title: string;
    content: string;
    authorName: string;
    subjectName: string;
    authorId: number;
    postId: number;
    photoPath: string;
    timeStamp: Date;
    allowed: boolean;
}