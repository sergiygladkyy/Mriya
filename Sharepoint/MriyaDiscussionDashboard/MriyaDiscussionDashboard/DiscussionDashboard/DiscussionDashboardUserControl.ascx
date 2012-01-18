<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DiscussionDashboardUserControl.ascx.cs" Inherits="MriyaDiscussionDashboard.DiscussionDashboard.DiscussionDashboardUserControl" %>

<%--
<div class="discussion_post">
	<h3><a href="#">«Мрія» вітала гостей на Вечірі українського бізнесу в Німеччині...</a></h3>
	08.08 12:55 <a href="#">&quot;Остання репліка з агротеми що відповідає заданій тематиці...&quot; <img src="imgs/arrow_link.gif" width="9" height="7" /></a></div>
--%> 



<div class="discussion_dashboard_records">
    <asp:Label ID="labelErrorMessage" runat="server" Font-Bold="True" ForeColor="#CC0000" 
        Text="There are no errors!" Visible="False" CssClass="discussion_dashboard_error_message"></asp:Label>
    <asp:UpdatePanel ID="updatePanelList" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="updateProgressList" runat="server" 
                AssociatedUpdatePanelID="updatePanelList" DisplayAfter="0">
                <ProgressTemplate>
                    Зачекайте...<br />
                    <br />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Literal ID="literalDiscussionRecords" runat="server"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>     