using System;
using System.Web.Http;
using TicketDesk.Domain;
using TicketDesk.Domain.Model;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using ngWebClientAPI.Models;
using System.Web.Mvc;
using System.Net;

/*
 * This needs refactor....badly
 * Need to refactor it to return success/depending on whether or not the action could or could not
 * be completed.  This will involve HTTPstatusCodes being passed to the front end w/messages
 * indicating what happened.  For success not much is needed other than like 200 or somehting close.
 * Failure needs more detailed messages and catch conditions....need better error checking
 */

namespace ngWebClientAPI.Controllers
{
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/actions")]
    public class ActionsController : ApiController
    {
        TicketActivityController ticketActivityController;
        public ActionsController()
        {            
            TicketDeskContextSecurityProvider secur = new TicketDeskContextSecurityProvider();
            ticketActivityController = new TicketActivityController(new TdDomainContext(secur));
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("activity-buttons/{ticketId}")]
        public JObject ActivityButtons(long ticketId)
        {
            int id = APITicketConversion.ConvertTicketId(ticketId);
            var permissions = new JObject();
            int activities = (int) ticketActivityController.ActivityButtons(id);
            permissions.Add("actionPermissions", activities);
            return permissions;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("resolve")]
        public async Task<HttpStatusCodeResult> Resolve([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            //convert data to comment and ID
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                int ticketId = data["ticketId"].ToObject<int>();
                string comment = data["comment"].ToObject<string>();
                Ticket ticket = await ticketActivityController.Resolve(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.Conflict, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("add-comment")]
        public async Task<HttpStatusCodeResult> AddComment([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                Ticket ticket = await ticketActivityController.AddComment(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.BadRequest, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("assign")]
        public async Task<HttpStatusCodeResult> Assign([FromBody] JObject data)
        {
            //convertAssign
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                string comment = data["comment"].ToObject<string>();
                string assignedTo = data["assignedTo"].ToObject<string>();
                string priority = data["priority"].ToObject<string>();
                Ticket ticket = await ticketActivityController.Assign(ticketId, comment, assignedTo, priority);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return result;
            
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("cancel-more-info")]
        public async Task<HttpStatusCodeResult> CancelMoreInfo([FromBody] JObject data) {
            //convertInfo
            HttpStatusCodeResult result;
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                await ticketActivityController.CancelMoreInfo(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("edit-ticket-info")]
        public Task<Ticket> EditTicketInfo([FromBody] JObject data)
        {
            Int64 semanticId = data["ticketId"].ToObject<Int64>();
            int ticketId = APITicketConversion.ConvertTicketId(semanticId);
            int projectId = data["projectId"].ToObject<int>();
            string comment = data["comment"].ToObject<string>();
            string title = data["title"].ToObject<string>();
            string details = data["details"].ToObject<string>();
            string priority = data["priority"].ToObject<string>();
            string ticketType = data["ticketType"].ToObject<string>();
            string category = data["category"].ToObject<string>();
            string owner = data["owner"].ToObject<string>();
            string tagList = data["tagList"].ToObject<string>();
            Task<Ticket> ticket = ticketActivityController.EditTicketInfo(ticketId, projectId, comment, title, details, priority, ticketType, category, owner, tagList);
            return ticket;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("close")]
        public async Task<HttpStatusCodeResult> Close([FromBody]JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                await ticketActivityController.Close(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch (Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("force-close")]
        public async Task<HttpStatusCodeResult> ForceClose([FromBody]JObject data)
        {
            //convertInfo
            HttpStatusCodeResult result;
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                await ticketActivityController.ForceClose(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("give-up")]
        public async Task<HttpStatusCodeResult> GiveUp([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                InfoObject info = APIActionsConversion.ConvertInfo(data);
                await ticketActivityController.GiveUp(info.ticketId, info.comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("pass")]
        public async Task<HttpStatusCodeResult> Pass([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                string comment = data["comment"].ToObject<string>();
                string priority = data["priority"].ToObject<string>();
                string assignedTo = data["assignedTo"].ToObject<string>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                Ticket ticket = await ticketActivityController.Pass(ticketId, comment, assignedTo, priority);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("reassign")]
        public async Task<HttpStatusCodeResult> ReAssign([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                //convertAssign
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                string comment = data["comment"].ToObject<string>();
                string assignedTo = data["assignedTo"].ToObject<string>();
                string priority = data["priority"].ToObject<string>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                await ticketActivityController.ReAssign(ticketId, comment, assignedTo, priority);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch (Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("request-more-info")]
        public async Task<HttpStatusCodeResult> RequestMoreInfo([FromBody] JObject data)
        {
            //convertinfo
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                string comment = data["comment"].ToObject<string>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                await ticketActivityController.RequestMoreInfo(ticketId, comment);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("reopen")]
        public async Task<HttpStatusCodeResult> ReOpen([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                string comment = data["comment"].ToObject<string>();
                bool assignToMe = data["assignToMe"].ToObject<bool>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                await ticketActivityController.ReOpen(ticketId, comment, assignToMe);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("supply-more-info")]
        public async Task<HttpStatusCodeResult> SupplyMoreInfo([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                string comment = data["comment"].ToObject<string>();
                bool reactive = data["reactive"].ToObject<bool>();
                await ticketActivityController.SupplyMoreInfo(ticketId, comment, reactive);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("take-over")]
        public async Task<HttpStatusCodeResult> TakeOver([FromBody] JObject data)
        {
            HttpStatusCodeResult result;
            try
            {
                Int64 semanticId = data["ticketId"].ToObject<Int64>();
                int ticketId = APITicketConversion.ConvertTicketId(semanticId);
                string comment = data["comment"].ToObject<string>();
                string priority = data["priority"].ToObject<string>();
                await ticketActivityController.TakeOver(ticketId, comment, priority);
                result = new HttpStatusCodeResult(HttpStatusCode.OK);
            } catch(Exception ex)
            {
                result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, ex.ToString());
            }
            return result;
        }

    }
}
