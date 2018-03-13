import { actionURLs } from '../app-settings';

// http://andregiannico.com/typescript-constants

export class TicketActionEnum {
  static readonly ADDCOMMENT = new TicketActionEnum(
    'Add Comment', 2 ** 0, true, false, {ticketId: 0, comment: ''}
  );
  static readonly PROVIDEMOREINFO = new TicketActionEnum(
    'Provide More Information', 2 ** 1, true, false, {ticketId: 0, comment: '', reactive: true}
  );
  static readonly CANCELMOREINFO = new TicketActionEnum(
    'Cancel Request For More Information', 2 ** 2, false, false, {ticketId: 0, comment: ''}
  );
  static readonly REQUESTMOREINFO = new TicketActionEnum(
    'Request More Information', 2 ** 3, true, false, {ticketId: 0, comment: ''}
  );
  static readonly TAKEOVER = new TicketActionEnum(
    'Take Over', 2 ** 4, false, false, {ticketId: 0, comment: '', priority: ''}
  );
  static readonly RESOLVE = new TicketActionEnum(
    'Resolve', 2 ** 5, true, false, {ticketId: 0, comment: ''}
  );
  static readonly ASSIGN = new TicketActionEnum(
    'Assign', 2 ** 6, false, true, {ticketId: 0, comment: '', assignedTo: '', priority: ''}
  );
  static readonly REASSIGN = new TicketActionEnum(
    'Reassign', 2 ** 7, false, true, {ticketId: 0, comment: '', assignedTo: '', priority: ''}
  );
  static readonly PASS = new TicketActionEnum(
    'Pass', 2 ** 8, false, true, {ticketId: 0, comment: '', priority: '', assignedTo: ''}
  );
  static readonly CLOSE = new TicketActionEnum(
    'Close', 2 ** 9, false, false, {ticketId: 0, comment: ''}
  );
  static readonly REOPEN = new TicketActionEnum(
    'Reopen', 2 ** 10, true, false, {ticketId: 0, comment: '', assignToMe: true}
  );
  static readonly GIVEUP = new TicketActionEnum(
    'Give Up', 2 ** 11, true, false, {ticketId: 0, comment: ''}
  );
  static readonly FORCECLOSE = new TicketActionEnum(
    'Force Close', 2 ** 12, true, false, {ticketId: 0, comment: ''}
  );
  static readonly EDITATTACHMENTS = new TicketActionEnum(
    'Edit Attachments', 2 ** 13, false, false, {ticketId: 0, comment: ''}
  );
  static readonly EDITTICKET = new TicketActionEnum(
    'Edit Activity', 2 ** 14, false, false, {ticketId: 0, comment: ''}
  );
  static readonly CREATE = new TicketActionEnum(
    'Create', 2 ** 15, false, false, {ticketId: 0, comment: ''}
  );
  static readonly CREATEONBEHALFOF = new TicketActionEnum(
    'Create On Behalf Of', 2 ** 16, false, true, {ticketId: 0, comment: ''}
  );
  static readonly allActivities: TicketActionEnum[] = [
    TicketActionEnum.ADDCOMMENT,
    TicketActionEnum.PROVIDEMOREINFO,
    TicketActionEnum.CANCELMOREINFO,
    TicketActionEnum.REQUESTMOREINFO,
    TicketActionEnum.TAKEOVER,
    TicketActionEnum.RESOLVE,
    TicketActionEnum.ASSIGN,
    TicketActionEnum.REASSIGN,
    TicketActionEnum.PASS,
    TicketActionEnum.CLOSE,
    TicketActionEnum.REOPEN,
    TicketActionEnum.GIVEUP,
    TicketActionEnum.FORCECLOSE,
    TicketActionEnum.EDITATTACHMENTS,
    TicketActionEnum.EDITTICKET,
    TicketActionEnum.CREATE,
    TicketActionEnum.CREATEONBEHALFOF
  ];

  private constructor(
    public displayText: string,
    public enumInteger: number,
    public requiresComment: boolean,
    public specifiesUser: boolean,
    public formTemplate) {}
  public static isAllowedAction(action: TicketActionEnum, activityNumber: number) {
    return (action.enumInteger & activityNumber) > 0;
  }
  public static getActivityList(activityNumber: number) {
    return TicketActionEnum.allActivities.filter(action => TicketActionEnum.isAllowedAction(action, activityNumber));
  }
  
  public getURL(): string {
    return actionURLs[this.enumInteger];
  }
}
