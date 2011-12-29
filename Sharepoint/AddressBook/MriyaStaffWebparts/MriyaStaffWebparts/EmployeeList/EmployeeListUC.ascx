<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeListUC.ascx.cs" Inherits="MriyaStaffWebparts.EmployeeList.EmployeeListUC" %>

<script language="javascript" type="text/javascript">

    function findMrPBElementPos(element) {
        var x = y = 0;
        {
            while ((element = element.offsetParent) != null) {
                x += element.offsetLeft;
                y += element.offsetTop;
            }
        }

        return { 'x': x, 'y': y };
    }

    function showDetailsDiv(show) {
        var divBackground = document.getElementById('divMrPBookRecordDetailsBackground');
        var divDetails = document.getElementById('divMrPBookRecordDetailsOuter');

        if (show) {
            if (divDetails) {
                var windowX, windowY;
                var divHeight = parseInt(divDetails.style.height, 10);
                var divWidth = parseInt(divDetails.style.width, 10);

                //IE
                if (!window.innerWidth) {
                    //strict mode
                    if (!(document.documentElement.clientWidth == 0)) {
                        windowX = document.documentElement.clientWidth;
                        windowY = document.documentElement.clientHeight;
                    }
                    //quirks mode
                    else {
                        windowX = document.body.clientWidth;
                        windowY = document.body.clientHeight;
                    }
                }
                //w3c
                else {
                    windowX = window.innerWidth;
                    windowY = window.innerHeight;
                }

                var leftOffset = (windowX - divWidth) / 2;
                var topOffset = (windowY - divHeight) / 2;

                divDetails.style.position = 'fixed';
                divDetails.style.display = 'block';
                divDetails.style.left = leftOffset + 'px';
                divDetails.style.top = topOffset + 'px';
            }
            if (divBackground)
                divBackground.style.display = 'block';
        }
        else {
            if (divBackground)
                divBackground.style.display = 'none';
            if (divDetails)
                divDetails.style.display = 'none';
        }
    }

</script>

<div class="styleMrEList">
    <asp:Label ID="labelError" runat="server" Font-Bold="True" ForeColor="#CC0000" 
        Text="There are no errors!" Visible="False" CssClass="styleMrEListErrorMessage"></asp:Label>
    <asp:UpdatePanel ID="updatePanelList" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="updateProgressList" runat="server" 
                AssociatedUpdatePanelID="updatePanelList" DisplayAfter="0">
                <ProgressTemplate>
                    Зачекайте...<br />
                    <br />
                </ProgressTemplate>
            </asp:UpdateProgress>

            <asp:Literal ID="literalCards" runat="server"></asp:Literal>

            <asp:Panel ID="panelRefresh" runat="server" CssClass="styleMrEListPanelRefresh">
                <asp:LinkButton ID="linkButtonRefresh" runat="server" CausesValidation="False" 
                    onclick="linkButtonRefresh_Click" CssClass="styleMrEListButtonRefresh">Refresh</asp:LinkButton>
                <asp:Label ID="labelRefreshDelim" runat="server" Text=" | "></asp:Label>
                <asp:LinkButton ID="linkButtonShowAll" runat="server" CausesValidation="False" 
                    onclick="linkButtonShowAll_Click" CssClass="styleMrEListButtonShowAll">Shaw all</asp:LinkButton>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="linkButtonShowAll" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="linkButtonRefresh" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>

<div id="divMrPBookRecordDetailsOuter" style="height:350px;width:550px;display:none">
    <asp:Panel ID="panelMrPBookRecordDetails" runat="server" 
        CssClass="styleMrPBookPopupRecDetails" Visible="False">
        <div id="divRecordDetailsCmdHeader" style="height:20px;text-align: right">
            <asp:LinkButton ID="linkButtonCloseDetailsHdr" runat="server" 
                oncommand="linkButtonCloseDetails_Command">Close</asp:LinkButton>
        </div>
        <div id="divMrPBookRecordDetailsInner">
            <div id="divRecordDetailsBody">
                <table cellpadding="0" cellspacing="0" border="0" style="height:310px;width:100%;">
                    <tr>
                        <td align="center" valign="top" width="150px">
                            <div id="divMrPBookDetailsPhoto">
                            <asp:Image ID="imageDetails" runat="server" 
                                CssClass="styleMrPBookDetailsPhoto" />
                            </div>
                        </td>
                        <td align="left" valign="top">
                            <asp:Literal ID="literalDetails" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div id="divRecordDetailsCmdFooter" style="height:20px;text-align: right">
                <asp:LinkButton ID="linkButtonCloseDetailsFtr" runat="server"
                oncommand="linkButtonCloseDetails_Command">Close</asp:LinkButton>
        </div>
    </asp:Panel>
</div>

<div id="divMrPBookRecordDetailsBackground" style="display:none">
</div>
