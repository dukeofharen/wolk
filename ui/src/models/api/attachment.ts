export default interface Attachment {
    id: number;
    created: Date;
    updated: Date;
    filename: string;
    mimeType: string;
    fileSize: string;
    noteId: number;
}