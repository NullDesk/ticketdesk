<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TicketDesk.Domain.Models.Ticket>" %>

<%@ Import Namespace="TicketDesk.Web.Client.Helpers" %>
<asp:Content ID="title" ContentPlaceHolderID="TitleContent" runat="server">
    Ticket:
    <%: Model.TicketId %>
</asp:Content>
<asp:Content ID="head" ContentPlaceHolderID="CustomHeadContent" runat="server">
    <% var Editor = "markitup"; %>
    <% if (Editor == "wmd")
       { %>
    <link rel="stylesheet" type="text/css" href="<%= Links.Scripts.openlibrary_wmd_master.wmdCustom_css %>" />
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_css %>" />
    <script type="text/javascript" src="<%= Links.Scripts.jquery_hoverIntent_minified_js %>"></script>
    <script type="text/javascript" src="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_min_js %>"></script>
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
    <script type="text/javascript" src="<%= Links.Scripts.valums_ajax_upload_6f977de.ajaxupload_js %>"></script>
    <script type="text/javascript" src="<%= Links.Scripts.prettify_small_3_Dec_2009.prettify_js %>"></script>
    <link rel="stylesheet" type="text/css" href="<%= Links.Scripts.prettify_small_3_Dec_2009.prettify_css %>" />
    <link rel="Stylesheet" type="text/css" media="all" href="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_css %>" />
    <script type="text/javascript" src="<%= Links.Scripts.jquery_autocomplete.jquery_autocomplete_min_js %>"></script>
    <script type="text/javascript">


        function onUploadComplete(filename, response) {

            $('<tr id="fileItem_' + response + '"><td><table class="formatTable" cellpadding="0" cellspacing="0" style="width: 100%; border: solid 1px #B3CBDF;"> <tbody> <tr> <td rowspan="2" class="PendingFileAttachmentItemContainer"> <img alt="Pending File" src="<%= Url.Content(string.Format("~/Content/pendingFlag.png")) %>" /> </td> <th> <label> File: </label> </th> <td><input id="newFileId_' + response + '" name="newFileId_' + response + '" type="hidden" value="' + response + '" /><input id="newFileName_' + response + '" name="newFileName_' + response + '" type="text" style="width: 325px;" value="' + filename + '" /> </td> <td rowspan="2" style="text-align:right;"> <a class="noLink" href="" onclick="removeAttachment(' + response + ');return false;"> <img src="<%= Url.Content("~/Content/cancel.png") %>" alt="remove" /></a></td> </tr> <tr> <th> <label> Description: </label> </th> <td> <input id="newFileDescription_' + response + '" name="newFileDescription_' + response + '" type="text" style="width:325px;" /> (optional) </td> </tr> </tbody> </table></td></tr>').appendTo('#files_list');

            $('.PendingFileAttachmentItemContainer').fadeIn('normal', function () { CheckActivityHeight(); });


            return true;

        }
    </script>
    <% 
        if (false)
        {
    %>
    <script src="../../Scripts/MicrosoftAjax.debug.js" type="text/javascript"></script>
    <script src="../../Scripts/MicrosoftMvcAjax.debug.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../../Scripts/markitup/markitup/jquery.markitup.js" type="text/javascript"></script>
    <script src="../../Scripts/markitup/markitup/sets/markdown/set.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-autocomplete/jquery.autocomplete.min.js" type="text/javascript"></script>
    <script src="../../Scripts/openlibrary-wmd-master/jquery.wmd.js" type="text/javascript"></script>
    <%
        }
    %>
    <script type="text/javascript">

        function makePretty() {
            $("pre code").parent().each
                (
                    function () {
                        if (!$(this).hasClass("prettyprint")) {
                            $(this).addClass("prettyprint");
                        }
                    }
                );
            prettyPrint();
        }

        function weaponize() {
            $("#details").wmd({
                "preview": true,
                //"output": true,
                "helpLink": "http://daringfireball.net/projects/markdown/",
                "helpHoverTitle": "Markdown Help"
            });

            $("#comment").wmd({
                "preview": true,
                //"output": true,
                "helpLink": "http://daringfireball.net/projects/markdown/",
                "helpHoverTitle": "Markdown Help"
            });
        }

        var editor = "markitup";
        if (editor == "markitup") {
            mySettings.previewParserPath = '<%= Url.Content("~/MarkdownPreview")%>';
            mySettings.nameSpace = 'commentEditor';
            mySettings.previewOpenCallback = CheckActivityHeight;
            mySettings.previewCloseCallback = CheckActivityHeight;
            mySettings.previewOpen = true;
        }

        var WMDCallbackWaiting = false;
        var WMDCallback =
        function () {
            if (!WMDCallbackWaiting) {
                WMDCallbackWaiting = true;
                window.setTimeout(function () {
                    CheckActivityHeight();
                    WMDCallbackWaiting = false;
                }, 250);
            }
        };

        $("document").ready(function () { $("#ModifyAttachmentsLink").show(); CheckScrollDetails(); AddStyles(); CheckActivityHeight(); });

        function goUploadify() {

            var button = $('#fileUploader'), interval;
            if (button.length > 0) {
                new AjaxUpload(button, {
                    action: '<%= Url.Content("~/Uploader/AddAttachment/") %>',
                    name: 'myfile',
                    data: { 'id': <%= Model.TicketId %> },
                    responseType: 'json',
                    onSubmit: function (file, ext) {
                        button.text('Uploading');
                        $("#progress").show();

                        this.disable();

                        interval = window.setInterval(function () {
                            var text = button.text();
                            if (text.length < 13) {
                                button.text(text + '.');
                            } else {
                                button.text('Uploading');
                            }
                        }, 200);
                    },
                    onComplete: function (file, response) {
                        
                        button.text('Upload');
                        $("#progress").hide();
                        window.clearInterval(interval);

                        this.enable();

                        onUploadComplete(file, response.id);
                    }
                });
            }
        }

        function AddStyles() {
            $("head").append("<style>#comment{display:none;}</style>"); //style hides the comment boxes by default, but only when javascript is enabled on client
            $("#activitySizer").css("overflow", "hidden").css("height", "88px");
            makePretty();
        }

        

        var detailsLastHeight = 0;
        var detailsMinHeight = 200;
        var hasExpander = false;
        var isExpanded = false;

        function CheckScrollDetails() {
            $("#detailTextExpander").hide();
           
            $("#detailsText").each(function () {
                
                if ((this.scrollHeight > this.clientHeight)) {
                    hasExpander = true;
                    detailsLastHeight = this.scrollHeight;

                    $("#detailTextExpander").show();
                }
            });
        }

        function CheckActivityHeight() {
            var newHeight = 150;
            newHeight = $('#activityArea').get(0).scrollHeight + 10;
            $("#activitySizer").animate({ height: newHeight }, 500);
        }

        function expandDetails() {
            if ($("#detailTextExpander").hasClass("collapserButton")) {
                isExpanded = false;
                $("#detailsText").animate({ height: detailsMinHeight }, 300, function () { $("#detailsText").css('overflow', 'auto'); $("#detailTextExpander").toggleClass("collapserButton"); });
               
            }
            else {
                isExpanded = true;
                $("#detailsText").animate({ height: detailsLastHeight }, 300, function () { $("#detailsText").css('overflow', 'visible'); $("#detailTextExpander").toggleClass("collapserButton"); });
                
            }
        }

        function setupAutocomplete() {
            $('#TagList').autocomplete('<%= Url.Action("AutoComplete", "TagList") %>',
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
        };

        function beginChangeActivity(args) {

            $(".validation-summary-errors").attr("style", "display:none;").text("");
            $("#activityArea").animate({ opacity: 0.5 }, 200);
        }

        function beginChangeToModifyDetailsActivity() {
            beginChangeActivity();
        }

        function beginRefreshAttachments(args) {
            $('#attachmentsWrapper').animate({ opacity: 0.5 }, 200);
        }

        function beginRefreshHistory(args) {
            $('#activityHistoryArea').animate({ opacity: 0.5 }, 200);
        }

        function beginRefreshDetails(args) {
            $('#detailsWrapper').animate({ opacity: 0.5 }, 200, 'linear', function () { });
        }

        function beginRefreshStats(args) {
            $('#statsWrapper').animate({ opacity: 0.5 }, 200);
        }

        function completeChangeToEditActivity() {
            completeChangeActivity();
        }

        function completeChangeActivity() {

            $("#activityArea").animate({ opacity: 1 }, 200, function () {

                if (editor == "wmd") {
                    $("#comment").show("fast", function () { CheckActivityHeight(); });
                    weaponize();
                }
                else if (editor == "markitup") {
                    $("#comment").show().markItUp(mySettings, function () { CheckActivityHeight(); });
                    $("#details").markItUp(mySettings);
                }
                goUploadify();
                setupAutocomplete();
                $("#ModifyAttachmentsLink").show();
                //TODO: Need to wait until div has been replaced with new content, then focus either details editor or comment editor.
                //        waiting for stackoverflow question before working out how to do this here


            });

        }

        function completeRefreshAttachments() {
            $('#attachmentsWrapper').animate({ opacity: 1 }, 300, function () { corners(); });
        }

        function completeRefreshHistory() {
            $('#activityHistoryArea').animate({ opacity: 1 }, 300);
            makePretty();
        }

        function completeRefreshDetails() {
            $('#detailsWrapper').animate({ opacity: 1 }, 300, function () { CheckScrollDetails(); });
            makePretty();
        }

        function restoreDetailsHeight() {
            if (hasExpander && !isExpanded) {
                detailsMinHeight = 180;
                $("#detailsText").animate({ height: detailsMinHeight }, 300, 'linear', function () { CheckScrollDetails(); });
            }
        }

        function completeRefreshStats() {
            //alert("completeRefreshDetails");
            $('#statsWrapper').animate({ opacity: 1 }, 300);
        }

        function completeActivity(data) {
            $("#activityArea").animate({ opacity: 1 }, 200, function () {

                var commentBox = $("#comment");

                if (commentBox.length < 1) {
                    $("#ModifyAttachmentsLink").show();

                }


            });
            CheckActivityHeight();
        }

        function completeModifyTicketActivity(data) {
            var commentBox = $("#comment");
            if (commentBox.length > 0) {

                if (editor == "wmd") {
                    commentBox.show();
                    weaponize();
                }
                else if (editor == "markitup") {
                    commentBox.show().markItUp(mySettings);
                    $("#details").markItUp(mySettings);
                }

                goUploadify();
                setupAutocomplete();
            }
            else {
                $("#refreshHistoryButton").click();

            }
            completeActivity();
        }

        function completeModifyTicketActivityAttachmentsAndDetails(data) {
            completeModifyTicketActivityAndDetails(data);
            var n = $('#files_list').length;

            if (n < 1) {

                $("#refreshAttachmentsButton").click();
            }
        }

        function completeEditTicketDetails(data) {
            var commentBox = $("#comment");
            if (commentBox.length < 1) {
                $("#refreshDetailsButton").click();
                $("#refreshStatsButton").click();
            }
            completeModifyTicketActivity(data);
        }

        function completeModifyTicketActivityAndDetails(data) {
            completeModifyTicketActivity(data);
            var commentBox = $("#comment");
            if (commentBox.length < 1) {
                $("#refreshDetailsButton").click();
                $("#refreshStatsButton").click();
            }
        }

        function failModifyTicketActivity() {
            $(".validation-summary-errors").removeAttr("style").text("An unknown error has occured while communicating with the server, please try again.");
        }

        function removeAttachment(fileId) {
            var p = $('#fileItem_' + fileId);
            p.fadeOut('normal', function () { p.remove(); });
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        var activity = ViewData["activity"] as string;        
    %>
    <% 
        if (TempData.ContainsKey("TicketCenterList") && TempData.ContainsKey("TicketCenterPage") && TempData.ContainsKey("TicketCenterListDisplayName"))
        {             
            
    %>
    <div style="padding: 2px;">
        <%= Html.ActionLink("Back to: " + TempData["TicketCenterListDisplayName"], MVC.TicketCenter.Actions.List(Convert.ToInt32(TempData["TicketCenterPage"]), (string)TempData["TicketCenterList"]), new { Style = "margin:2px;" })%>
    </div>
    <%
        }
    %>
    <div class="contentContainer">
        <div id="detailsWrapper" style="width:100%;">
            <div class="displayContainerOuter">
                <div class="displayContainerInner" style="background-color: #FBFCFD;">
                    <div class="displayHeaderSideBar" >
                      <%  using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.RefreshAttachments, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "attachmentsArea", OnBegin = "beginRefreshAttachments", OnSuccess = "completeRefreshAttachments" }))
                            {
                        %><div id="attachmentsArea" >
                            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Attachments, Model, ViewData);%>
                        </div>
                        <input type="submit" style="display: none;" id="refreshAttachmentsButton" value="Refresh Attachments" />
                        <%
                            }
                        %>


                        <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.RefreshStats, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "statsArea", OnBegin = "beginRefreshStats", OnSuccess = "completeRefreshStats" }))
                           {
                        %>
                        <div id="statsArea"  >
                            <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.TicketStats, Model, ViewData); %>
                        </div>
                        <input type="submit" style="display: none;" id="refreshStatsButton" value="Refresh Stats" />
                        <%
                            }
                        %>
                      

                    </div>
                    <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.RefreshDetails, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "detailsArea", OnBegin = "beginRefreshDetails", OnSuccess = "completeRefreshDetails" }))
                       {
                    %>
                    <div id="detailsArea">
                        <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.Details, Model, ViewData);%>
                    </div>
                    <input type="submit" style="display: none;" id="refreshDetailsButton" value="Refresh Details" />
                    <%
                        }
                    %>
                </div>
            </div>
        </div>
        <div id="activityWrapper">
            <div class="displayContainerOuter">
                <div class="displayContainerInner">
                    <div id="activitySizer">
                        <div id="activityArea">
                            <% Html.RenderPartial(string.Format("~/Views/TicketEditor/Controls/{0}.ascx", activity), Model); %>
                        </div>
                        <span class="validation-summary-errors" style="display: none;"></span>
                    </div>
                </div>
            </div>
        </div>
        <% using (Ajax.BeginForm(MVC.TicketEditor.ActionNames.RefreshHistory, new { ID = Model.TicketId }, new AjaxOptions { UpdateTargetId = "activityHistoryArea", OnBegin = "beginRefreshHistory", OnSuccess = "completeRefreshHistory" }))
           {
        %>
        <div id="activityHistoryWrapper">
            <div class="displayContainerOuter">
                <div class="displayContainerInner">
                    <div id="activityHistoryArea">
                        <% Html.RenderPartial(MVC.TicketEditor.Views.Controls.ActivityHistory, Model, ViewData); %>
                    </div>
                </div>
            </div>
            <input type="submit" style="display: none;" id="refreshHistoryButton" value="Refresh History" />
        </div>
        <%
            }
        %>
    </div>
</asp:Content>
