// http://andregiannico.com/typescript-constants
export class TicketActionEnum {
	static readonly ADDCOMMENT: TicketActionEnum = new TicketActionEnum("Add Comment", 2 ** 0, true, false);
	static readonly PROVIDEMOREINFO: TicketActionEnum = new TicketActionEnum("Provide More Information", 2**1, true, false);
	static readonly CANCELMOREINFO: TicketActionEnum = new TicketActionEnum("Cancel Request For More Information", 2**2, false, false);
	static readonly REQUESTMOREINFO: TicketActionEnum = new TicketActionEnum("Request More Information", 2**3, true, false);
	static readonly TAKEOVER: TicketActionEnum = new TicketActionEnum("Take Over", 2**4, false, false);
	static readonly RESOLVE: TicketActionEnum = new TicketActionEnum("Resolve", 2**5, true, false);
	static readonly ASSIGN: TicketActionEnum = new TicketActionEnum("Assign", 2**6, false, true);
	static readonly REASSIGN: TicketActionEnum = new TicketActionEnum("Reassign", 2**7, false, true);
	static readonly PASS: TicketActionEnum = new TicketActionEnum("Pass", 2**8, false, true);
	static readonly CLOSE: TicketActionEnum = new TicketActionEnum("Close", 2**9, false, false);
	static readonly REOPEN: TicketActionEnum = new TicketActionEnum("Reopen", 2**10, true, false);
	static readonly GIVEUP: TicketActionEnum = new TicketActionEnum("Give Up", 2**11, true, false);
	static readonly FORCECLOSE: TicketActionEnum = new TicketActionEnum("Force Close", 2**12, true, false);
	static readonly EDITATTACHMENTS: TicketActionEnum = new TicketActionEnum("Edit Attachments", 2**13, false, false);
	static readonly EDITACTIVITY: TicketActionEnum = new TicketActionEnum("Edit Activity", 2**14, false, false);
	static readonly CREATE: TicketActionEnum = new TicketActionEnum("Create", 2**15, false, false);
	static readonly CREATEONBEHALFOF: TicketActionEnum = new TicketActionEnum("Create On Behalf Of", 2**16, false, true);
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
TicketActionEnum.EDITACTIVITY,
TicketActionEnum.CREATE,
TicketActionEnum.CREATEONBEHALFOF];

	private constructor(public displayText: string, public enumInteger: number, public requiresComment: boolean, public specifiesUser: boolean) {};
	public static isAllowedAction(action: TicketActionEnum, activityNumber: number) {
		return (action.enumInteger & activityNumber) > 0;
	}
	public static getActivityList(activityNumber: number) {
		return TicketActionEnum.allActivities.filter(action => TicketActionEnum.isAllowedAction(action, activityNumber));	
	}
}
