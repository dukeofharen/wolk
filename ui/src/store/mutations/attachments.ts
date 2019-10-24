import { StateModel } from '@/models/store/stateModel';
import Attachment from '@/models/api/attachment';

export function SET_ATTACHMENTS(state: StateModel, attachments: Attachment[]) {
    state.attachments = attachments;
};