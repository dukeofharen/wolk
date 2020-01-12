import {KeyValuePair} from "@/models/keyValuePair";

export enum DueStatusType {
    NOT_SET,
    NOT_DUE_YET,
    DUE_IN_A_MONTH,
    DUE_IN_A_WEEK,
    DUE_IN_A_DAY,
    DUE_TODAY,
    OVERDUE
}

export const DueStatusTypeMap = new Map<DueStatusType, string>([
    [DueStatusType.NOT_DUE_YET, 'Not due yet'],
    [DueStatusType.DUE_IN_A_MONTH, 'Due in a month'],
    [DueStatusType.DUE_IN_A_WEEK, 'Due in a week'],
    [DueStatusType.DUE_IN_A_DAY, 'Due in a day'],
    [DueStatusType.DUE_TODAY, 'Due today'],
    [DueStatusType.OVERDUE, 'Overdue']
]);

export function getDueStatusTypeArray(): Array<KeyValuePair<DueStatusType, string>> {
    let result: Array<KeyValuePair<DueStatusType, string>> = [];
    let keys = Array.from(DueStatusTypeMap.keys());
    for (let key of keys) {
        result.push({
            key: key,
            value: DueStatusTypeMap.get(key) || ''
        });
    }

    return result;
}