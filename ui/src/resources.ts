export const resources = {
    unauthorized: "You're not not authorized. You should log in first.",
    credentialsIncorrect: "The provided credentials are incorrect.",
    backupRestoredSuccessfully: "The backup was restored successfully. You need to log in again.",
    notebookCreated: "The notebook was added successfully.",
    notebookUpdated: "The notebook was updated successfully.",
    notebookDeleted: "The notebook was deleted successfully.",
    noteCreated: "The note was added successfully.",
    noteUpdated: "The note was updated successfully.",
    noteDeleted: "The note was deleted successfully.",
    attachmentUploaded: "The attachment '{0}' was uploaded successfully.",
    attachmentDeleted: "The attachment was deleted successfully.",
    areYouSureDeleteNote: "Are you sure you want to delete this note? The note and all attachments will be deleted! This can not be undone.",
    areYouSureDeleteNotebook: "Are you sure you want to delete this notebook? All notes will be deleted! This can not be undone.",
    areYouSureDeleteAttachment: "Are you sure you want to delete this attachment? This can not be undone.",
    areYouSureDeleteStickyNote: "Are you sure you want to delete this sticky note?",
    areYouSureDeleteTodoItem: "Are you sure you want to delete this todo item?",
    unsavedChanges: "There are unsaved changes. Continue?",
    notFound: "This item could not be found (anymore).",
    serverError: "The server returned an error. Try again.",
    attachmentAccessCreated: "You can access the attachment under URL {0}.",
    attachmentAccessClickToCopy: "Click this message to copy the link to the clipboard."
};

export const keys = {
    signedInUserKey: "SignedInUser"
};

export const eventKeys = {
    noteUpdated: "noteUpdated",
    noteCreated: "noteCreated"
};

export const timeUnitsInSeconds = {
    hour: 3600,
    day: (24 * 3600),
    week: (24 * 3600 * 7),
    month: (24 * 3600 * 31)
};