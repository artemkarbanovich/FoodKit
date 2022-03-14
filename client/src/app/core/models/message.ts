export interface Message {
    id: number;
    content: string;
    dateSent: Date;
    dateRead: Date | null;
    senderId: number;
    senderName: string;
    recipientId: number;
    recipientName: string;
}