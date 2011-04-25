<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserShortVerticalListUserControl.ascx.cs" Inherits="MriyaAddressBook.UserShortVerticalList.UserShortVerticalListUserControl" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<script language="javascript" type="text/javascript">

    function ShowABShortListProfileDialog(caption, url) {
        var options = {
            url: url,
            autoSize: true,
            allowMaximize: true,
            title: caption,
            showClose: true,
            width: 800
        };
        var dialog = SP.UI.ModalDialog.showModalDialog(options);
    }

</script>

<div id="mr-short_list">
    <asp:Label ID="labelError" runat="server" Font-Bold="True" ForeColor="#CC0000" 
        Text="There are no errors!" Visible="False"></asp:Label>
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

            <asp:Panel ID="panelRefresh" runat="server" CssClass="refresh">
                <asp:LinkButton ID="linkButtonRefresh" runat="server" CausesValidation="False" 
                    onclick="linkButtonRefresh_Click">Оновити</asp:LinkButton>
                <asp:Label ID="labelRefreshDelim" runat="server" Text=" | "></asp:Label>
                <asp:LinkButton ID="linkButtonShowAll" runat="server" CausesValidation="False" 
                    onclick="linkButtonShowAll_Click">Показати всі</asp:LinkButton>
            </asp:Panel>

        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="linkButtonShowAll" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="linkButtonRefresh" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>





