<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PhoneBookUC.ascx.cs" Inherits="MriyaStaffWebparts.PhoneBook.PhoneBookUC" %>

<script language="javascript" type="text/javascript">

    function clearMrPBSearchText(ctrl, defaultText) {
        if (ctrl.value == defaultText) {
            ctrl.value = "";
            ctrl.style.color = "#000000";
        }
    }

    function resetMrPBSearchText(ctrl, defaultText) {
        if (ctrl.value == "") {
            ctrl.value = defaultText;
            ctrl.style.color = "#c0c0c0";
        }
    }

    function showMrPBProfileDialog(caption, url) {
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

	function findMrPBElementPos(element) {
	    var x = y = 0;
//        Works for FF
//        if (element.x && element.y)
//        {
//            x = element.x;
//            y = element.y;
//        }
//        else
        {
	        while ((element = element.offsetParent) != null) {
		        x += element.offsetLeft;
		        y += element.offsetTop;
	        }
        }

	    return {'x':x,'y':y};
    }

    function showMrPBPhotoDialog(control, url) {
        var ctlDiv = null;
        var ctlControl = null;

        if ((ctlDiv = document.getElementById('divShowMrPBPhotoDialog')) == null)
            return;

        if ((ctlControl = document.getElementById(control)) == null)
            return;

        var pos = findMrPBElementPos(ctlControl);

        ctlDiv.style.position = 'absolute';
        ctlDiv.style.left = '0px';
        ctlDiv.style.top = '0px';
        ctlDiv.style.display = 'block';

        var posOffset = findMrPBElementPos(ctlDiv);

        ctlDiv.style.left = (pos.x - posOffset.x + 28) + 'px';
        ctlDiv.style.top = (pos.y - posOffset.y - 5) + 'px';
        ctlDiv.style.display = 'block';

        var ctlImage = document.getElementById("divShowMrPBPhotoDialogImg");

        if (ctlImage) {
            ctlImage.src = url;
        }
	}

	function hideMrPBPhotoDialog(urlDefNoImage) {
        var ctlDiv = null;

        if ((ctlDiv = document.getElementById('divShowMrPBPhotoDialog')) != null) {
            ctlDiv.style.display = 'none';
        }
        var ctlImage = document.getElementById("divShowMrPBPhotoDialogImg");
        if (ctlImage) {
            ctlImage.src = urlDefNoImage;
        }
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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="styleMrPBook" id="divMrPBook">

            <asp:Panel ID="panelMrPBookCaption" runat="server">
                <h1>
                    <asp:Label ID="labelMrPBookCaption" runat="server" Text="Phone book"></asp:Label>
                </h1>
            </asp:Panel>

            <asp:Panel ID="panelSearch" runat="server">
                <div class="styleMrPBookSearchBlock">
                    <table class="styleMrPBookSearchTable" cellpadding="0" cellspacing="0" border="0">
                        <tr valign="top">
                            <td width="100%">
                                <asp:TextBox ID="textBoxSearch" runat="server" 
                                    CssClass="styleMrPBookSearch" ToolTip="Пошук..." Width="99%" 
                                    AutoPostBack="True" CausesValidation="False" AutoCompleteType="Search" 
                                    ontextchanged="buttonSearch_Click" Wrap="False"></asp:TextBox>
                                <br />
                                <asp:LinkButton ID="buttonClearSearch" runat="server" 
                                    onclick="buttonClearSearch_Click">Clear</asp:LinkButton>
                            </td>
                            <td width="45px">
                                <asp:Button ID="buttonSearch" runat="server" Text="Search" 
                                    CssClass="styleMrPBookSearchButton" CausesValidation="False" 
                                    onclick="buttonSearch_Click"/>
                            </td>
                            <td width="45px">
                                <asp:Button ID="buttonSearchFilter" runat="server" Text="Filter" 
                                    CssClass="styleMrPBookSearchFilterButton" 
                                    onclick="buttonSearchFilter_Click" CausesValidation="False" 
                                    UseSubmitBehavior="False" />
                            </td>
                        </tr>
                    </table>
                </div>

                <asp:Panel ID="panelFilterBlock" runat="server" Visible="False">
                    <div class="styleMrPBookFilterBlock">
                        <table class="styleMrPBookFilterTable" cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="labelCity" runat="server" Text="City"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="listCity" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="listCity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="labelDepartment" runat="server" Text="Department"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="listDepartment" runat="server" AutoPostBack="True" 
                                        onselectedindexchanged="listDepartment_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </asp:Panel>

            <div class="styleMrPBook">
                <asp:Panel ID="panelMrPBookStatus" runat="server">
                    <div  class="styleMrPBookStatus">
                        <asp:Label ID="labelRecords" runat="server" Text="No records were found"></asp:Label>
                    </div>
                </asp:Panel>
                <div class="styleMrPBookTableHeader">
                </div>
                <asp:Label ID="labelError" runat="server" Font-Bold="True" ForeColor="#CC0000" 
                    Text="There are no errors!" Visible="False" 
                    CssClass="styleMrPBookErrorMessage"></asp:Label>
                <asp:Table ID="tableMrPBook" runat="server" CssClass="styleMrPBookTable" cellpadding="0" cellspacing="0" border="0">
                </asp:Table>
                <div class="styleMrPBookTableFooter">
                    
                    <asp:Table ID="tableMrPBookFooter" runat="server" CssClass="styleMrPBookFooter" cellpadding="0" cellspacing="0" border="0">
                    </asp:Table>
                    
                </div>
            </div>

        </div>

    </ContentTemplate>

</asp:UpdatePanel>

<div id="divShowMrPBPhotoDialog" class="styleMrPBookPopupPhoto" style="display:none">
    <div class="styleMrPBookPopupPhotoInner">
        <img id="divShowMrPBPhotoDialogImg" src="./" alt="Фото" />
    </div>
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
