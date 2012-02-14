<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" ddwrt:oob="true">
  <xsl:import href="/_layouts/xsl/main.xsl"/>

  <!--<xsl:template match="/">
    <xmp><xsl:copy-of select="."/></xmp> 
  </xsl:template>-->
  
  <xsl:variable name="ViewClassName">
    <xsl:choose>
      <xsl:when test="$dvt_RowCount=0">library-list ms-emptyView</xsl:when>
      <xsl:otherwise>library-list ms-listviewtable</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>
  <!--
  <xsl:template name="View_Default_RootTemplate" mode="RootTemplate" match="View" ddwrt:dvt_mode="root">
    <xsl:param name="ShowSelectAllCheckbox" select="'True'"/>-->
    <!-- Only if not doing the default view and toolbar type is standard (not freeform or none)-->
    <!--<xsl:if test="($IsGhosted = '0' and $MasterVersion=3 and Toolbar[@Type='Standard']) or $ShowAlways">
      <xsl:call-template name="ListViewToolbar"/>
    </xsl:if>
    <table width="100%" cellspacing="0" cellpadding="0" border="0">-->
      <!-- not show ctx for survey overview-->
      <!--<xsl:if test="not($NoCTX)">
        <xsl:call-template name="CTXGeneration"/>
      </xsl:if>
      <xsl:if test="List/@TemplateType=109">
        <xsl:call-template name="PicLibScriptGeneration"/>
      </xsl:if>
      <tr>
        <td>
          <xsl:if test="not($NoAJAX)">
            <iframe src="javascript:false;" id="FilterIframe{$ViewCounter}" name="FilterIframe{$ViewCounter}" style="display:none" height="0" width="0" FilterLink="{$FilterLink}"></iframe>
          </xsl:if>
          <table summary="{List/@title} {List/@description}" xmlns:o="urn:schemas-microsoft-com:office:office" o:WebQuerySourceHref="{$HttpPath}&amp;XMLDATA=1&amp;RowLimit=0&amp;View={$View}" 
                          width="100%" border="0" cellspacing="0" dir="{List/@Direction}">
            <xsl:if test="not($NoCTX)">
              <xsl:attribute name="onmouseover">EnsureSelectionHandler(event,this,<xsl:value-of select ="$ViewCounter"/>)</xsl:attribute>
            </xsl:if>
            <xsl:if test="$NoAJAX">
              <xsl:attribute name="FilterLink">
                <xsl:value-of select="$FilterLink"/>
              </xsl:attribute>
            </xsl:if>
            <xsl:attribute name="cellpadding">
              <xsl:choose>
                <xsl:when test="ViewStyle/@ID='15' or ViewStyle/@ID='16'">0</xsl:when>
                <xsl:otherwise>1</xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="id">
              <xsl:choose>
                <xsl:when test="$IsDocLib or dvt_RowCount = 0">onetidDoclibViewTbl0</xsl:when>
                <xsl:otherwise>
                  <xsl:value-of select="concat($List, '-', $View)"/>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:attribute name="class">
              <xsl:choose>
                <xsl:when test="ViewStyle/@ID='0' or ViewStyle/@ID='17'"><xsl:value-of select="$ViewClassName"/> ms-basictable</xsl:when>
                <xsl:otherwise><xsl:value-of select="$ViewClassName"/></xsl:otherwise>
              </xsl:choose>
            </xsl:attribute>
            <xsl:if test="$InlineEdit">
              <xsl:attribute name="inlineedit">javascript: <xsl:value-of select="ddwrt:GenFireServerEvent('__cancel;dvt_form_key={@ID}')"/>;CoreInvoke('ExpGroupOnPageLoad', 'true');</xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="." mode="full">
              <xsl:with-param name="ShowSelectAllCheckbox" select="$ShowSelectAllCheckbox"/>
            </xsl:apply-templates>
          </table>
              <xsl:choose>
                <xsl:when test="$IsDocLib or dvt_RowCount = 0"><script type='text/javascript'>HideListViewRows("onetidDoclibViewTbl0");</script></xsl:when>
                <xsl:otherwise>
                  <script type='text/javascript'><xsl:value-of select ="concat('HideListViewRows(&quot;', $List, '-', $View, '&quot;);')"/></script>
                </xsl:otherwise>
              </xsl:choose>
        </td>
      </tr>
      <xsl:if test="$dvt_RowCount = 0 and not (@BaseViewID='3' and List/@TemplateType='102')">
        <tr>
          <td>
             <table width="100%" border="0" dir="{List/@Direction}">
               <xsl:call-template name="EmptyTemplate" />
             </table>
          </td>
        </tr>
      </xsl:if>
    </table>-->
    <!-- rowlimit doesn't show page footer-->
    <!--<xsl:call-template name="pagingButtons" />
    <xsl:if test="Toolbar[@Type='Freeform'] or ($MasterVersion=4 and Toolbar[@Type='Standard'])">
      <xsl:call-template name="Freeform">
        <xsl:with-param name="AddNewText">
          <xsl:choose>
            <xsl:when test="List/@TemplateType='104'">-->
              <!-- announcement-->
              <!--<xsl:value-of select="$Rows/@resource.wss.idHomePageNewAnnounce"/>
            </xsl:when>
            <xsl:when test="List/@TemplateType='101' or List/@TemplateType='115'">-->
              <!-- doc lib or form lib-->
              <!--<xsl:value-of select="$Rows/@resource.wss.Add_New_Document"/>
            </xsl:when>
            <xsl:when test="List/@TemplateType='103'">-->
              <!-- link -->
              <!--<xsl:value-of select="$Rows/@resource.wss.AddNewLink"/>
            </xsl:when>
            <xsl:when test="List/@TemplateType='106'">-->
              <!-- Event -->
              <!--<xsl:value-of select="$Rows/@resource.wss.AddNewEvent"/>
            </xsl:when>
            <xsl:when test="List/@TemplateType='119'">-->
              <!-- Wiki Library -->
              <!--<xsl:value-of select="$Rows/@resource.wss.AddNewWikiPage"/>
            </xsl:when>
            <xsl:otherwise>
              <xsl:value-of select="$Rows/@resource.wss.addnewitem"/>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:with-param>
        <xsl:with-param name="ID">
          <xsl:choose>
          <xsl:when test="List/@TemplateType='104'">idHomePageNewAnnouncement</xsl:when>
          <xsl:when test="List/@TemplateType='101'">idHomePageNewDocument</xsl:when>
          <xsl:when test="List/@TemplateType='103'">idHomePageNewLink</xsl:when>
          <xsl:when test="List/@TemplateType='106'">idHomePageNewEvent</xsl:when>
          <xsl:when test="List/@TemplateType='119'">idHomePageNewWikiPage</xsl:when>
          <xsl:otherwise>idHomePageNewItem</xsl:otherwise>
          </xsl:choose>
        </xsl:with-param>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>-->

  <xsl:template match="View" mode="full">
	<xsl:param name="ShowSelectAllCheckbox" select="'True'"/>
    <xsl:variable name="ViewStyleID">
      <xsl:value-of select="ViewStyle/@ID"/>
    </xsl:variable>
    <xsl:variable name="dirClass">
      <xsl:choose>
        <xsl:when test="$XmlDefinition/List/@Direction='rtl'"> ms-vhrtl</xsl:when>
        <xsl:otherwise> ms-vhltr</xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <tr valign="top" style="display: none;" class="{concat('ms-viewheadertr',$dirClass)}">
        <xsl:if test="$MasterVersion=4 and $TabularView='1'">
          <!-- ViewStyleID ''=Default / ViewStyleID 17=Shaded -->
          <xsl:if test="($ViewStyleID = '' or $ViewStyleID = '17') and $ShowSelectAllCheckbox = 'True'">
            <th class="ms-vh-icon" scope="col"><input type="checkbox" title="{$select_deselect_all}" onclick="ToggleAllItems(event,this,{$ViewCounter})" onfocus="EnsureSelectionHandlerOnFocus(event,this,{$ViewCounter})" /></th>
          </xsl:if>
        </xsl:if>
        <xsl:if test="$InlineEdit"><th class="ms-vh2 ms-vh-inlineedit"/></xsl:if>
        <xsl:if test="not($GroupingRender)">
          <xsl:apply-templates mode="header" select="ViewFields/FieldRef[not(@Explicit='TRUE')]"/>
        </xsl:if>
    </tr>
    <xsl:apply-templates select="." mode="RenderView" />
    <xsl:apply-templates mode="footer" select="." />
  </xsl:template>

</xsl:stylesheet>