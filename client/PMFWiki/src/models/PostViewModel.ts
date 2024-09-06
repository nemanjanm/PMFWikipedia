export interface PostViewModel{
    title: string;
    content: string;
    authorName: string;
    subjectName: string;
    subjectId: number;
    authorId: number;
    postId: number;
    photoPath: string;
    timeStamp: Date;
    allowed: boolean;
    timeEdited: Date;
    editorName: string;
    editorId: number;
}