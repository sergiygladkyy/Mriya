<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserListUserControl.ascx.cs" Inherits="MriyaAddressBook.UserList.UserListUserControl" %>
<%@ Register assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.WebControls" tagprefix="asp" %>

<script language="javascript" type="text/javascript">

    function ShowABProfileDialog(caption, url)
	{
        var options = {
	        url: url,
	        autoSize:true,
	        allowMaximize:true,
	        title: caption,
	        showClose: true,
            width: 800
        };
	    var dialog = SP.UI.ModalDialog.showModalDialog(options);
	}
	
    function FindElementPos(element) 
    {
	    var x = y = 0;
//        Works for FF
//        if (element.x && element.y)
//        {
//            x = element.x;
//            y = element.y;
//        }
//        else
        {
	        while ((element = element.offsetParent) != null) 
            {
		        x += element.offsetLeft;
		        y += element.offsetTop;
	        }
        }

	    return {'x':x,'y':y};
    }

    function ShowABPhotoDialog(control, url)
	{
        var ctlDiv = null;
        var ctlControl = null;

        if((ctlDiv = document.getElementById('divShowABPhotoDialog')) == null)
            return;

        if ((ctlControl = document.getElementById(control)) == null)
            return;

        var pos = FindElementPos(ctlControl);

        ctlDiv.style.position = 'absolute';
        ctlDiv.style.left = '0px';
        ctlDiv.style.top = '0px';
        ctlDiv.style.display = 'block';

        var posOffset = FindElementPos(ctlDiv);

        ctlDiv.style.left = (pos.x - posOffset.x + 28) + 'px';
        ctlDiv.style.top = (pos.y - posOffset.y - 5) + 'px';
        ctlDiv.style.display = 'block';

        var ctlImage = document.getElementById("divShowABPhotoDialogImg");

        if (ctlImage)
        {
            ctlImage.src = url;
        }
	}

    function HideABPhotoDialog(urlDefNoImage)
    {
        var ctlDiv = null;

        if((ctlDiv = document.getElementById('divShowABPhotoDialog')) != null)
        {
            ctlDiv.style.display = 'none';
        }
        var ctlImage = document.getElementById("divShowABPhotoDialogImg");
        if (ctlImage) 
        {
            ctlImage.src = urlDefNoImage;
        }
    }
</script>

<div id="mr-address_book">
    <asp:Panel ID="panelCaption" runat="server">
        <h1>
            <asp:Label ID="labelCaption" runat="server" Text="Адресна книга"></asp:Label>
        </h1>
    </asp:Panel>
    <asp:Panel ID="panelSearch" runat="server">
        <br />
        <asp:Label ID="labelSearch" runat="server" Text="Пошук "></asp:Label>
        <asp:TextBox ID="textBoxSearch" runat="server" AutoPostBack="True" 
            Width="215px" ontextchanged="textBoxSearch_TextChanged"></asp:TextBox>
    &nbsp;<asp:Button ID="buttonSearch" runat="server" onclick="buttonSearch_Click" 
            Text="Знайти" CausesValidation="False" />
        <asp:Button ID="buttonShowAll" runat="server" CausesValidation="False" 
            onclick="buttonShowAll_Click" Text="Показати всі" Visible="False" />
        <br />
        </asp:Panel>
    <br />
    <asp:Label ID="labelError" runat="server" Font-Bold="True" ForeColor="#CC0000" 
        Text="There are no errors!" Visible="False"></asp:Label>
    <asp:Table ID="tableProfiles" runat="server" BorderStyle="None" 
        BorderWidth="0px" CellPadding="0" CellSpacing="0">
    </asp:Table>
</div>

<div id="divShowABPhotoDialog" class="mr-ab-popup_photo">
    <div class="mr-ab-popup_padd">
        <img id="divShowABPhotoDialogImg" height="80" width="80" src="./" alt="Фото" />
    </div>
</div>

