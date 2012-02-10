<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" ddwrt:oob="true">
  <xsl:import href="/_layouts/xsl/main.xsl"/>

  <!--<xsl:template match="/">
    <xmp><xsl:copy-of select="."/></xmp> 
  </xsl:template>-->
  
  <xsl:variable name="ViewClassName">
    <xsl:choose>
      <xsl:when test="$dvt_RowCount=0">custom-news-list ms-emptyView</xsl:when>
      <xsl:otherwise>custom-news-list ms-listviewtable</xsl:otherwise>
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
  
  
  <xsl:template match="View" mode="RenderView">
    <xsl:variable name="ViewStyleID">
      <xsl:value-of select="ViewStyle/@ID"/>
    </xsl:variable>
    <xsl:variable name="HasExtraColumn" select="$TabularView='1' and $MasterVersion=4 and ($ViewStyleID = '' or $ViewStyleID = '17')"/>
    <!-- total first -->
    <xsl:if test="Aggregations[not(@Value='Off')]/FieldRef">
      <tr>
        <xsl:if test="$HasExtraColumn">
          <td/>
        </xsl:if>
        <xsl:if test="$InlineEdit">
          <td width="1%"/>
        </xsl:if >
        <xsl:apply-templates mode="aggregate" select="ViewFields/FieldRef[not(@Explicit='TRUE')]">
          <xsl:with-param name="Rows" select="$AllRows"/>
          <xsl:with-param name="GroupLevel" select="0"/>
        </xsl:apply-templates>
      </tr>
    </xsl:if>
    <xsl:variable name="Fields" select="ViewFields/FieldRef[not(@Explicit='TRUE')]"/>
    <xsl:variable name="Groups" select="Query/GroupBy/FieldRef"/>
    <xsl:variable name="Collapse" select="Query/GroupBy[@Collapse='TRUE']"/>
    <xsl:variable name="GroupCount" select="count($Groups)"/>
	<tr>
	  <td class="list-items">
	  <xsl:attribute name="colspan">
	    <xsl:value-of select="$dvt_RowCount" />
	  </xsl:attribute>
    <xsl:for-each select="$AllRows">
      <xsl:variable name="thisNode" select="."/>
      <xsl:if test="$GroupCount &gt; 0">
        <xsl:call-template name="GroupTemplate">
          <xsl:with-param name="Groups" select="$Groups"/>
          <xsl:with-param name="Collapse" select="$Collapse"/>
          <xsl:with-param name="HasExtraColumn" select="$HasExtraColumn"/>
        </xsl:call-template>
      </xsl:if>
      <xsl:if test="not(not($NoAJAX) and not($InlineEdit) and $Collapse and $GroupCount &gt; 0)">
        <xsl:apply-templates mode="Item" select=".">
          <xsl:with-param name="Fields" select="$Fields"/>
          <xsl:with-param name="Collapse" select="$Collapse"/>
          <xsl:with-param name="Position" select="position()"/>
          <xsl:with-param name="Last" select="last()"/>
        </xsl:apply-templates>
      </xsl:if>
    </xsl:for-each>
	  </td>
	</tr>
    <xsl:if test="$InlineEdit and not($IsDocLib) and $ListRight_AddListItems = '1'">
      <xsl:call-template name="rowinsert">
        <xsl:with-param name="Fields" select="$Fields"/>
      </xsl:call-template>
    </xsl:if>
  </xsl:template>
  
  <xsl:template mode="Item" match="Row">
    <xsl:param name="Fields" select="."/>
    <xsl:param name="Collapse" select="."/>
    <xsl:param name="Position" select="1" />
    <xsl:param name="Last" select="1" />
    <xsl:variable name="thisNode" select="."/>
    <xsl:variable name="ID">
      <xsl:call-template name="ResolveId"><xsl:with-param name="thisNode" select ="."/></xsl:call-template>
    </xsl:variable>
    <xsl:variable name="FSObjType">
      <xsl:choose>
        <xsl:when test="$EntityName != ''">0</xsl:when>
        <xsl:otherwise>
          <xsl:value-of select="./@FSObjType"/>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="altClass">
      <xsl:choose>
        <xsl:when test="$Position mod 2 = 0">ms-alternating</xsl:when>
        <xsl:otherwise></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="hoverClass">
      <xsl:choose>
        <xsl:when test="($TabularView='1' and $MasterVersion=4) or $InlineEdit">ms-itmhover</xsl:when>
        <xsl:otherwise></xsl:otherwise>
      </xsl:choose>
    </xsl:variable>
    <xsl:variable name="EditMode" select="$dvt_form_key = @ID or $dvt_form_key = @BdcIdentity"/>
    <div>
      <xsl:if test="$Collapse">
        <xsl:attribute name="style">display:none</xsl:attribute>
      </xsl:if>
      <xsl:attribute name="class">
        <xsl:value-of select="normalize-space(concat($altClass, ' ', $hoverClass))"/>
      </xsl:attribute>
      <xsl:if test="($TabularView='1' and $MasterVersion=4) or $InlineEdit">
        <xsl:attribute name="iid">
          <xsl:value-of select="$ViewCounter"/>,<xsl:value-of select="$ID"/>,<xsl:value-of select="$FSObjType"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$EditMode">
        <xsl:attribute name="automode">
          <xsl:value-of select ="$ViewCounter"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$TabularView='1' and $MasterVersion=4">
        <td class="ms-vb-itmcbx ms-vb-firstCell"><input type="checkbox" class="s4-itm-cbx"/></td>
      </xsl:if>
      <xsl:if test="$InlineEdit">
        <xsl:call-template name="AutoModeHeader"/>
      </xsl:if>
	  
	  <xsl:choose>
        <xsl:when test="$thisNode/@PublishingRollupImage=''">
		  <xsl:call-template name="NewsWithoutImage">
            <xsl:with-param name="thisNode" select="$thisNode" />
            <xsl:with-param name="Position" select="$Position"/>
            <xsl:with-param name="Fields" select="$Fields"/>
          </xsl:call-template>
		</xsl:when>
		<xsl:otherwise>
		  <xsl:call-template name="NewsWithImage">
            <xsl:with-param name="thisNode" select="$thisNode" />
            <xsl:with-param name="Position" select="$Position"/>
            <xsl:with-param name="Fields" select="$Fields"/>
          </xsl:call-template>
		</xsl:otherwise>
      </xsl:choose>
    </div>
  </xsl:template>
  
  <xsl:template name="customPrintAverageRating" match="FieldRef" mode="printAverageRating" ddwrt:dvt_mode="body">
    <xsl:param name="thisNode" select="."/>
	<xsl:apply-templates select="." mode="PrintFieldWithECB">
      <xsl:with-param name="thisNode" select="$thisNode"/>
    </xsl:apply-templates>
  </xsl:template>
  
	<xsl:template name="NewsWithImage">
	    <xsl:param name="thisNode" select="."/>
		<xsl:param name="Position" select="."/>
		<xsl:param name="Fields" select="."/>
		
		<div class="news_block">
			<div class="left_cell">
				<div class="news_image">
					<xsl:value-of select="$thisNode/@PublishingRollupImage" disable-output-escaping="yes" />
				</div>
				<div class="rating">
					<div class="stars">
						<xsl:apply-templates select="$Fields[@Name='AverageRating']" mode="printAverageRating">
							<xsl:with-param name="thisNode" select="$thisNode"/>
						</xsl:apply-templates>
					</div>
					<xsl:text>Оцінити статтю</xsl:text>
				</div>
			</div>
			<div class="right_cell">
				<a href="#" class="news_header_link">
					<xsl:attribute name="href">
						<xsl:value-of select="$thisNode/@FileRef.urlencodeasurl"/>
					</xsl:attribute>
					<xsl:value-of select="$thisNode/@FileLeafRef.Name" />
				</a>
				<br />
				<span class="news_date"><xsl:value-of select="$thisNode/@ArticleStartDate" /></span>
				<div class="news_text">
					<xsl:value-of select="$thisNode/@_x041e__x043f__x0438__x0441__x0020__x043d__x043e__x0432__x0438__x043d__x0438_" />
				</div>
			</div>
		</div>
	</xsl:template>
	
	<xsl:template name="NewsWithoutImage">
		<xsl:param name="thisNode" select="."/>
		<xsl:param name="Position" select="."/>
		<xsl:param name="Fields" select="."/>
		
		<div class="news_block">
			<div class="news_content">
				<a href="#" class="news_header_link">
					<xsl:attribute name="href">
						<xsl:value-of select="$thisNode/@FileRef.urlencodeasurl"/>
					</xsl:attribute>
					<xsl:value-of select="$thisNode/@FileLeafRef.Name" />
				</a>
				<br />
				<span class="news_date"><xsl:value-of select="$thisNode/@ArticleStartDate" /></span>
				<div class="news_text">
					<xsl:value-of select="$thisNode/@_x041e__x043f__x0438__x0441__x0020__x043d__x043e__x0432__x0438__x043d__x0438_" />
				</div>
			</div>
			<div class="rating">
				<div class="stars">
					<xsl:apply-templates select="$Fields[@Name='AverageRating']" mode="printAverageRating">
						<xsl:with-param name="thisNode" select="$thisNode"/>
					</xsl:apply-templates>
				</div>
				<xsl:text>Оцінити статтю</xsl:text>
			</div>
		</div>
	</xsl:template>
  

</xsl:stylesheet>