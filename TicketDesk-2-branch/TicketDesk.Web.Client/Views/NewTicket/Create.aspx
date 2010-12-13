<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Web.Client.Models.TicketCreateViewModel>" %>

<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Create
</asp:Content>
<asp:Content ID="head" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_css %>" />
     <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.uploadify.uploadify_css %>" />
    
    

    <script type="text/javascript" src="<%= Links.Scripts.jquery_qtip_1_0_0_beta3_1020438.jquery_qtip_1_0_0_beta3_1_min_js %>"></script>
    <script type="text/javascript" src="<%= Links.Scripts.jquery_hoverIntent_minified_js %>"></script>
    <script type="text/javascript" src="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_min_js %>"></script>
     <link rel="stylesheet" type="text/css" href="<%= Links.Scripts.prettify_small_3_Dec_2009.prettify_css %>" />
    <script type="text/javascript" src="<%= Links.Scripts.prettify_small_3_Dec_2009.prettify_js %>"></script>
    <% var Editor = "markitup"; %>
    <% if (Editor == "wmd")
       { %>


    <link rel="stylesheet" type="text/css" href="<%= Links.Scripts.openlibrary_wmd_master.wmdCustom_css %>" />
    
    <script type="text/javascript" src="<%= Links.Scripts.openlibrary_wmd_master.showdown_js %>"></script>
   
    <script type="text/javascript" src="<%= Links.Scripts.openlibrary_wmd_master.jquery_wmd_js %>"></script>
    <% }
       else if (Editor == "markitup")
       {%>

      
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.markitup_editor.markitup.skins.markdown.style_css %>" />
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.markitup_editor.markitup.sets.markdown.style_css %>" />
    <script type="text/javascript" src="<%= Links.Scripts.markitup_editor.markitup.jquery_markitup_js %>"></script>
    <script type="text/javascript" src="<%= Links.Scripts.markitup_editor.markitup.sets.markdown.set_js %>"></script>
    <%} %>
    <% if (false)
       {%>
    <script type="text/javascript" src="../../Scripts/openlibrary-wmd-master/showdown.js"></script>
    <script type="text/javascript" src="../../Scripts/openlibrary-wmd-master/jquery.wmd.js"></script>
    <script src="../../Scripts/markitup-editor/markitup/jquery.markitup.js" type="text/javascript"></script>
    <script src="../../Scripts/markitup-editor/markitup/sets/markdown/set.js" type="text/javascript"></script>
    <script src="../../Scripts/MicrosoftAjax.debug.js" type="text/javascript"></script>
    <script src="../../Scripts/MicrosoftMvcAjax.debug.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-qtip-1.0.0-beta3.1020438/jquery.qtip-1.0.0-beta3.1.min.js"
        type="text/javascript"></script>
    <script src="../../Scripts/jquery.hoverIntent.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../../../Scripts/jquery-autocomplete/jquery.autocomplete.min.js"></script>
    <% } %>
    <script type="text/javascript">
    
    var editor = "markitup";

    if(editor == "wmd")
    {
        $(document).ready(function() {
            $("#Ticket_Details").wmd({
		        "preview": true,
                //"output": true,
		        "helpLink": "http://daringfireball.net/projects/markdown/",
		        "helpHoverTitle": "Markdown Help"
		    }); 
        });
     } 
     else if(editor == "markitup")
     {
        mySettings.previewParserPath = '<%= Url.Content("~/MarkdownPreview")%>';
        
        mySettings.previewOpen = <%= Model.Settings.UserSettings.OpenEditorWithPreview.ToString().ToLower() %>;
       
        $(document).ready(function() { $("#Ticket_Details").markItUp(mySettings);}); 
     }
     
        
        $(document).ready
        (
            function() 
            {
                $("pre code").parent().each
                (
                    function()
                    {
                        if(!$(this).hasClass("prettyprint"))
                        {
                            $(this).addClass("prettyprint");
                        }
                    }
                );
                prettyPrint();
                
            }
        )
        
        $(document).ready(function() { corners(); goUploadify(); $("#attachmentsArea").show(); })
        
        function corners() {
            $(".displayContainerInner").corner("bevel 5px").parent().css('padding', '3px').corner("round keep  10px");
        }
    </script>
    <script type="text/javascript">

        function onUploadError() {
            alert("error during upload");
        }
        function onUploadComplete(event, qId, file, response) {

            $('<tr id="fileItem_' + response + '"><td><table class="formatTable" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #B3CBDF;"> <tbody> <tr> <td rowspan="2" class="PendingFileAttachmentItemContainer"> <img alt="Pending File" src="<%= Url.Content(string.Format("~/Content/pendingFlag.png")) %>" /> </td> <th> <label> File: </label> </th> <td><input id="newFileId_' + response + '" name="newFileId_' + response + '" type="hidden" value="' + response + '" /><input id="newFileName_' + response + '" name="newFileName_' + response + '" type="text" style="width: 225px;" value="' + file.name + '" /> </td> <td rowspan="2" style="text-align:right;"> <a href="" onclick="removeAttachment(' + response + ');return false;"> <img src="<%= Url.Content("~/Content/cancel.png") %>" alt="remove" /></a></td> </tr> <tr> <th> <label> Description: </label> </th> <td> <input id="newFileDescription_' + response + '" name="newFileDescription_' + response + '" type="text" style="width:200px;" /> (optional) </td> </tr> </tbody> </table></td></tr>').appendTo('#files_list');
            $('.PendingFileAttachmentItemContainer').fadeIn('normal');
            return true;

        }

        function removeAttachment(fileId) {
            var p = $('#fileItem_' + fileId);
            p.fadeOut('normal', function () { p.remove(); });
            return false;
        }

        
    </script>
    <%= Html.Uploadify(
            "fileUpload", 
            new UploadifyOptions
               {
                    UploadUrl = Url.Content("~/Uploader/AddAttachment/"),
                    FileExtensions = "*",
                    FileDescription = "All Files",
                    AuthenticationToken = Request.Cookies[FormsAuthentication.FormsCookieName] == null ?
                        string.Empty :
                        Request.Cookies[FormsAuthentication.FormsCookieName].Value,
                    ErrorFunction = "onUploadError",
                    CompleteFunction = "onUploadComplete",
                    ButtonText = "Add Files",
                   
                    
               }
        ) %>
</asp:Content>
<asp:Content ID="main" ContentPlaceHolderID="MainContent" runat="server">
    <% var Editor = "markitup"; %>
    <div class="contentContainer">
    <% Html.EnableClientValidation(); %>
        <% using (Html.BeginForm(MVC.NewTicket.Create(), FormMethod.Post, new { id = "createTicketForm", enctype = "multipart/form-data" }))
           { %>
        <div class="displayContainerOuter">
            <div class="displayContainerInner">
                <div class="activityHeadWrapper">
                    <div class="activityHead">
                        Create New Ticket
                    </div>
                </div>
                <div class="activityBody" style="padding: 15px;">
                    <table class="formatTable">
                        <tbody>
                            <tr>
                                <th>
                                   <%: Html.ValidationMessageFor(m => m.Ticket.Title,"*") %><%=  Html.LabelFor(m => m.Ticket.Title) %>
                                </th>
                                <td colspan="2">
                                    <%: Html.TextBoxFor(m => m.Ticket.Title, new { style = "min-width:300px;width:450px;" })%>
                                    
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <%: Html.ValidationMessageFor(m => m.Ticket.Type, "*")%><%= Html.LabelFor(m => m.Ticket.Type) %>
                                </th>
                                <td>
                                    <%: Html.DropDownListFor(m => m.Ticket.Type, Model.TicketTypeList) %>
                                    
                                </td>
                                <td style="width: 100%; padding: 0px;">
                                    <table class="formatTable" cellpadding="0" cellspacing="0">
                                        <tbody>
                                            <tr>
                                                <th>
                                                    <%: Html.ValidationMessageFor(m => m.Ticket.Category, "*")%><%= Html.LabelFor(m => m.Ticket.Category) %>
                                                </th>
                                                <td>
                                                    <%:  Html.DropDownListFor(m => m.Ticket.Category, Model.CategoryList) %>
                                                    
                                                </td>
                                                <%
                                                    if (Model.DisplayPriorityList)
                                                    {
                                                %>
                                                <th>
                                                    <%: Html.ValidationMessageFor(m => m.Ticket.Priority, "*")%><%= Html.LabelFor(m => m.Ticket.Priority) %>
                                                </th>
                                                <td>
                                                    <%:  Html.DropDownListFor(m => m.Ticket.Priority, Model.PriorityList)%>
                                                    
                                                </td>
                                                <%
                                                    } 
                                                %>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <%: Html.ValidationMessageFor(m => m.Ticket.TagList, "*")%><%= Html.LabelFor(m => m.Ticket.TagList) %>
                                </th>
                                <td colspan="2">
                                    <%: Html.TextBoxFor(m => m.Ticket.TagList, new { style = "min-width:300px;width:450px;" })%>
                                    
                                    <script type="text/javascript">
                                        $(document).ready(function () {
                                            $('#Ticket_TagList').autocomplete('<%= Url.Action("AutoComplete", "TagList") %>',
                                                {
                                                    dataType: 'json',
                                                    parse: function (data) {
                                                        var rows = new Array();
                                                        for (var i = 0; i < data.length; i++) {
                                                            rows[i] = { data: data[i], value: data[i].TagName, result: data[i].TagName };
                                                        }
                                                        return rows;
                                                    },
                                                    formatItem: function (row, i, n) {
                                                        return row.TagName;
                                                    },
                                                    minChars: 2,
                                                    delay: 200
                                                }
                                            );
                                        });

                                    </script>
                                </td>
                            </tr>
                            <%
                                if (Model.DisplayOwner)
                                {
                            %>
                            <tr>
                                <th>
                                    <%: Html.LabelFor(m => m.Ticket.Owner) %>
                                </th>
                                <td colspan="2">
                                    <%: Html.DropDownListFor(m => m.Ticket.Owner, Model.OwnersList)%>
                                    <%: Html.ValidationMessageFor(m => m.Ticket.Owner, "*")%>
                                </td>
                            </tr>
                            <%
                                }
                            %>
                            <tr>
                                <th>
                                    <%: Html.ValidationMessageFor(m => m.Ticket.Details, "*")%><%= Html.LabelFor(m => m.Ticket.Details) %>
                                    
                                </th>
                                <td colspan="2">
                                    <%if (Editor == "markitup")
                                      { %>
                                    <%: Html.TextAreaFor(m => m.Ticket.Details, new { @Class = "markItUpEditor" })%>
                                    <%}
                                      else if (Editor == "wmd")
                                      { %>
                                    <div id="wmd-container">
                                        <%: Html.TextAreaFor(m => m.Ticket.Details, new { @Class = "wmd-input", Cols = "92", Rows = "15" })%>
                                    </div>
                                    <%} %>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                    <%: Html.LabelFor(m => m.Ticket.TicketAttachments) %>
                                </th>
                                <td colspan="2">
                                    <%: Html.ValidationMessageFor(m => m.Ticket.TicketAttachments, "*")%>
                                    <div id="fileUpload">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <th>
                                </th>
                                <td colspan="2">
                                    <div id="attachmentsArea" style="padding-left: 15px;">
                                        <table id="files_list">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                            <!-- This is where the output will appear -->
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <%: Html.ValidationSummary("Ticket creation was unsuccessful. Please correct the errors and try again.") %>
                    <input type="submit" value="Create Now" class="activityButton" />
                </div>
            </div>
        </div>
        <%
            } 
        %>
    </div>
</asp:Content>
