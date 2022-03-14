import { Connection } from "./connection";

export interface Group {
    groupName: string;
    connections: Connection[];
}