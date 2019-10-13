export default interface Note {
    id: number;
    title: string;
    content: string;
    preview: string;
    notebookId: number;
    created: Date;
    updated: Date;
}