export default interface Note {
    id: number;
    title: string;
    contents: string;
    preview: string;
    notebook_id: number;
    created: Date;
    updated: Date;
}