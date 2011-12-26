<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" ddwrt:oob="true">
	<xsl:import href="/_layouts/xsl/thread.xsl"/>
	<!--<xsl:template match="/">
		<xmp><xsl:copy-of select="."/></xmp> 
	</xsl:template> -->
	<xsl:template name="FieldRef_Thread_BodyAndMore_Computed_Thread" match="FieldRef[@Name='BodyAndMore']" mode="Computed_body" ddwrt:dvt_mode="body" priority="10">
		<xsl:param name="thisNode" select="."/>
		<xsl:param name="Position" select="1"/>
		<xsl:if test="$Position = 1">
			<input type="hidden" name="CAML_Expand" value="{$CAML_Expand}"/>
			<input type="hidden" name="CAML_ShowOriginalEmailBody" value="{$CAML_ShowOriginalEmailBody}"/>
		</xsl:if>
		<xsl:variable name="isRootPost">
			<xsl:call-template name="IsRootPost">
				<xsl:with-param name="thisNode" select="$thisNode"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:if test="$isRootPost='TRUE'">
			<div class="discussion-title">
				<b>
					<xsl:choose>
						<xsl:when test="$thisNode/@FSObjType='1'">
							<xsl:value-of select="$thisNode/@Title"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$thisNode/@DiscussionTitleLookup"/>
						</xsl:otherwise>
					</xsl:choose>
				</b>
			</div>
		</xsl:if>
		<xsl:variable name="BodyPositioningClass">
			<xsl:choose>
				<xsl:when test="$isRootPost='TRUE'">ms-disc-root-body</xsl:when>
				<xsl:otherwise>ms-disc-reply-body</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<div class="{$BodyPositioningClass}">
			<xsl:choose>
				<xsl:when test="$thisNode/@Body=''">
					<xsl:value-of select="$thisNode/../@resource.wss.NoText"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:variable name="WasExpanded">
						<xsl:call-template name="BodyWasExpanded">
							<xsl:with-param name="thisNode" select="$thisNode"/>
						</xsl:call-template>
					</xsl:variable>
					<xsl:choose>
						<xsl:when test="$WasExpanded='TRUE'">
							<!-- fullbody -->
							<xsl:call-template name="FullBody">
								<xsl:with-param name="thisNode" select="$thisNode"/>
							</xsl:call-template>
							<xsl:variable name="CorrectBody">
								<xsl:call-template name="CorrectBodyToShow">
									<xsl:with-param name="thisNode" select="$thisNode"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="contains($CorrectBody, 'TrimmedBody')">
									<div>
										<table border="0" cellspacing="0" cellpadding="0" class="ms-disc" dir="{$XmlDefinition/List/@direction}">
											<tr valign="top">
												<td>
													<xsl:call-template name="LessLink">
														<xsl:with-param name="thisNode" select="$thisNode"/>
													</xsl:call-template>
												</td>
												<td> | </td>
												<td>
													<xsl:call-template name="ToggleQuotedText">
														<xsl:with-param name="thisNode" select="$thisNode"/>
													</xsl:call-template>
												</td>
											</tr>
										</table>
									</div>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="LessLink">
										<xsl:with-param name="thisNode" select="$thisNode"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:variable name="TextWasExpanded">
								<xsl:call-template name="QuotedTextWasExpanded">
									<xsl:with-param name="thisNode" select="$thisNode"/>
								</xsl:call-template>
							</xsl:variable>
							<xsl:choose>
								<xsl:when test="$TextWasExpanded='TRUE'">
									<xsl:call-template name="FullBody">
										<xsl:with-param name="thisNode" select="$thisNode"/>
									</xsl:call-template>
									<xsl:call-template name="ToggleQuotedText">
										<xsl:with-param name="thisNode" select="$thisNode"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:otherwise>
									<xsl:call-template name="LimitedBody">
										<xsl:with-param name="thisNode" select="$thisNode"/>
									</xsl:call-template>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:otherwise>
			</xsl:choose>
		</div>
	</xsl:template>
	<xsl:template name="ToggleQuotedText">
		<xsl:param name="thisNode" select="."/>
		<xsl:variable name="CorrectBody">
			<xsl:call-template name="CorrectBodyToShow">
				<xsl:with-param name="thisNode" select="$thisNode"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$CorrectBody='TrimmedBody'">
				<div class="toggle-quoted-text">
					<a id="ToggleQuotedText{$thisNode/@ID}" href="javascript:" onclick="javascript:return ShowQuotedText('{$thisNode/@GUID}','{$thisNode/@GUID}',this);">
						<xsl:value-of select="$thisNode/../@resource.wss.ShowQuotedMessages"/>
					</a>
				</div>
			</xsl:when>
			<xsl:when test="$CorrectBody='UnTrimmedBody'">
				<div class="toggle-quoted-text">
					<a id="ToggleQuotedText{$thisNode/@ID}" href="javascript:" onclick="javascript:return HideQuotedText('{$thisNode/@GUID}','{$thisNode/@GUID}',this);">
						<xsl:value-of select="$thisNode/../@resource.wss.HideQuotedMessages"/>
					</a>
				</div>
			</xsl:when>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="MessageBody">
		<xsl:param name="thisNode" select="."/>
		<xsl:variable name="CorrectBody">
			<xsl:call-template name="CorrectBodyToShow">
				<xsl:with-param name="thisNode" select="$thisNode"/>
			</xsl:call-template>
		</xsl:variable>
		<xsl:choose>
			<xsl:when test="$CorrectBody='TrimmedBody'">
				<xsl:value-of select="$thisNode/@TrimmedBody"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$thisNode/@Body"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template name="StatusBar">
		<xsl:param name="thisNode" select="."/>
		<xsl:param name="indent" select="1"/>
		<xsl:variable name="indentLevel">
			<xsl:call-template name="IndentLevel">
				<xsl:with-param name="thisNode" select="$thisNode"/>
			</xsl:call-template>
		</xsl:variable>
		<table class="ms-disc-bar" border="0" cellpadding="0" cellspacing="0" width="100%" margin-left="{$indentLevel}0px">
			<tr>
				<td width="100%" nowrap="TRUE">
					<div class="anchor">
						<a name="{$thisNode/@GUID}"/>
						#<xsl:value-of select="$thisNode/@GUID" />
					</div>
				</td>
				<xsl:if test="$thisNode/@Attachments='1'">
					<td width="1" nowrap="TRUE">
						<div>
							<nobr>
								<xsl:apply-templates select="." mode="Attachments_body">
									<xsl:with-param name="thisNode" select="$thisNode"/>
								</xsl:apply-templates>
							</nobr>
						</div>
					</td>
				</xsl:if>
				<td style="border-style:none" nowrap="TRUE">
					<div class="reply_options">
						<a id="DisplayLink{$thisNode/@ID}" href="{$FORM_DISPLAY}&amp;ID={$thisNode/@ID}" onclick="EditLink2(this,{$ViewCounter});return false;" target="_self">
							<nobr><xsl:value-of select="$thisNode/../@resource.wss.ViewItemLink"/></nobr>
						</a>
					</div>
				</td>
				<xsl:variable name="hasRight">
					<xsl:call-template name="IfHasRight">
						<xsl:with-param name="thisNode" select="$thisNode"/>
					</xsl:call-template>
				</xsl:variable>
				<xsl:if test="$hasRight">
					<td class="ms-separator">
						<img src="/_layouts/images/blank.gif" alt=""/>
					</td>
					<td style="border-style:none" nowrap="nowrap">
						<div class="reply_message">
							<xsl:call-template name="ReplyNoGif">
								<xsl:with-param name="thisNode" select="$thisNode"/>
							</xsl:call-template>
						</div>
					</td>
				</xsl:if>
			</tr>
		</table>
	</xsl:template>
	<xsl:template name="ReplyNoGif">
		<xsl:param name="thisNode" select="."/>
		<a id="ReplyLink{$thisNode/@ID}" href="{$ENCODED_FORM_NEW}&amp;ContentTypeId=0x0107&amp;DiscussionParentID={$thisNode/@ID}" onclick="EditLink2(this,{$ViewCounter});return false;" target="_self">
			<nobr><xsl:value-of select="$thisNode/../@resource.wss.ReplyLinkText"/></nobr>
		</a>
	</xsl:template>
	<xsl:template name="CreatedDate">
		<xsl:param name="thisNode" select="."/>
		<xsl:param name="indent" select="0"/>
		<xsl:variable name="indentLevel">
			<xsl:call-template name="IndentLevel">
				<xsl:with-param name="thisNode" select="$thisNode"/>
			</xsl:call-template>
		</xsl:variable>
		<div class="created-date">
		<xsl:choose>
			<xsl:when test="$thisNode/@Created. = $thisNode/@Modified.">
				<xsl:choose>
					<xsl:when test="$thisNode/@EmailSender=''">
						<xsl:variable name="isRootPost">
							<xsl:call-template name="IsRootPost">
								<xsl:with-param name="thisNode" select="$thisNode"/>
							</xsl:call-template>
						</xsl:variable>
						<xsl:choose>
							<xsl:when test="$isRootPost='TRUE'">
								<xsl:choose>
									<xsl:when test="$indent">
										<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Started"/>
										<xsl:value-of select="$thisNode/@Modified"/>
										<xsl:text ddwrt:whitespace-preserve="yes" xml:space="preserve"> </xsl:text>
										<xsl:value-of select="$ByText"/>
										<xsl:value-of select="$thisNode/@Editor.span" disable-output-escaping="yes"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Started"/>
										<xsl:value-of select="$thisNode/@Modified"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="$indent">
										<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Posted"/>
										<xsl:value-of select="$thisNode/@Modified"/>
										<xsl:text ddwrt:whitespace-preserve="yes" xml:space="preserve"> </xsl:text>
										<xsl:value-of select="$ByText"/>
										<xsl:value-of select="$thisNode/@Editor.span" disable-output-escaping="yes"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Posted"/>
										<xsl:value-of select="$thisNode/@Modified"/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:when>
					<xsl:otherwise>
						<xsl:choose>
							<xsl:when test="$indent">
								<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Emailed"/>
								<xsl:value-of select="$thisNode/@Modified"/>
								<xsl:text ddwrt:whitespace-preserve="yes" xml:space="preserve"> </xsl:text>
								<xsl:value-of select="$ByText"/>
								<xsl:value-of select="$thisNode/@Editor.span" disable-output-escaping="yes"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Emailed"/>
								<xsl:value-of select="$thisNode/@Modified"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:when>
			<xsl:otherwise>
				<xsl:choose>
					<xsl:when test="$indent">
						<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Edited"/>
						<xsl:value-of select="$thisNode/@Modified"/>
						<xsl:text ddwrt:whitespace-preserve="yes" xml:space="preserve"> </xsl:text>
						<xsl:value-of select="$ByText"/>
						<xsl:value-of select="$thisNode/@Editor.span" disable-output-escaping="yes"/>
					</xsl:when>
					<xsl:otherwise>
						<xsl:value-of select="$thisNode/../@resource.wss.Discussion_Edited"/>
						<xsl:value-of select="$thisNode/@Modified"/>
					</xsl:otherwise>
				</xsl:choose>
			</xsl:otherwise>
		</xsl:choose>
		</div>
	</xsl:template>
	<xsl:template mode="Item" match="Row[../../@BaseViewID='1']">
		<xsl:param name="Fields" select="."/>
		<xsl:param name="Collapse" select="."/>
		<xsl:param name="Position" select="1"/>
		<xsl:variable name="thisNode" select="."/>
		<xsl:for-each select="$Fields">
			<tr>
				<xsl:if test="$Collapse">
					<xsl:attribute name="style">display:none</xsl:attribute>
				</xsl:if>
				<td class="ms-disc-padabove">
					<xsl:call-template name="IndentStatusBar">
						<xsl:with-param name="thisNode" select="$thisNode"/>
					</xsl:call-template>
				</td>
			</tr>
			<tr>
				<xsl:if test="$Collapse">
					<xsl:attribute name="style">display:none</xsl:attribute>
				</xsl:if>
				<td>
					<xsl:attribute name="class"><xsl:choose><xsl:when test="position()=1">ms-disc-bordered</xsl:when><xsl:otherwise>ms-disc-bordered-noleft</xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:if test="@Name='Threading'">
						<xsl:attribute name="width">100%</xsl:attribute>
					</xsl:if>
					<xsl:if test="position()=2">
						<xsl:call-template name="CreatedDate">
							<xsl:with-param name="thisNode" select="$thisNode"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:apply-templates mode="PrintFieldWithDisplayFormLink" select=".">
						<xsl:with-param name="thisNode" select="$thisNode"/>
						<xsl:with-param name="Position" select="$Position"/>
					</xsl:apply-templates>
				</td>
			</tr>
			<tr>
				<td>
					<img src="/_layouts/images/blank.gif" width="1px" height="15px" alt=""/>
				</td>
			</tr>
		</xsl:for-each>
	</xsl:template>
	<xsl:template mode="Item" match="Row[../../@BaseViewID='2']">
		<xsl:param name="Fields" select="."/>
		<xsl:param name="Collapse" select="."/>
		<xsl:param name="Position" select="1"/>
		<xsl:variable name="thisNode" select="."/>
		<tr>
			<xsl:if test="$Collapse">
				<xsl:attribute name="style">display:none</xsl:attribute>
			</xsl:if>
			<td colspan="{count($Fields)}">
				<xsl:attribute name="class"><xsl:choose><xsl:when test="position()=1">ms-disc-padabove</xsl:when><xsl:otherwise>ms-disc-nopad</xsl:otherwise></xsl:choose></xsl:attribute>
				<xsl:for-each select="$XmlDefinition/ViewFields/FieldRef[@Name='StatusBar']">
					<xsl:apply-templates mode="PrintFieldWithDisplayFormLink" select=".">
						<xsl:with-param name="thisNode" select="$thisNode"/>
						<xsl:with-param name="Position" select="$Position"/>
					</xsl:apply-templates>
				</xsl:for-each>
			</td>
		</tr>
		<tr>
			<xsl:if test="$Collapse">
				<xsl:attribute name="style">display:none</xsl:attribute>
			</xsl:if>
			<xsl:for-each select="$Fields">
				<td>
					<xsl:attribute name="class"><xsl:choose><xsl:when test="position()=1">ms-disc-bordered</xsl:when><xsl:otherwise>ms-disc-bordered-noleft</xsl:otherwise></xsl:choose></xsl:attribute>
					<xsl:if test="@Name='BodyAndMore'">
						<xsl:attribute name="width">100%</xsl:attribute>
					</xsl:if>
					<xsl:if test="position()=2">
						<xsl:call-template name="CreatedDate">
							<xsl:with-param name="thisNode" select="$thisNode"/>
						</xsl:call-template>
					</xsl:if>
					<xsl:apply-templates mode="PrintFieldWithDisplayFormLink" select=".">
						<xsl:with-param name="thisNode" select="$thisNode"/>
						<xsl:with-param name="Position" select="$Position"/>
					</xsl:apply-templates>
				</td>
			</xsl:for-each>
		</tr>
		<tr>
			<xsl:if test="$Collapse">
				<xsl:attribute name="style">display:none</xsl:attribute>
			</xsl:if>
			<td>
				<img src="/_layouts/images/blank.gif" width="1px" height="15px" alt=""/>
			</td>
		</tr>
	</xsl:template>
	<xsl:template name="View_Thread_Default_RootTemplate" match="View[List/@TemplateType='108']" mode="RootTemplate" ddwrt:dvt_mode="root">
		<table width="100%" cellspacing="0" cellpadding="0" border="0">
			<xsl:attribute name="class">
				<xsl:choose>
					<!-- Threaded/Flat view with non-zero number of items = class="ms-disc" -->
					<xsl:when test="(@BaseViewID='1' or @BaseViewID='2')">discussion-container</xsl:when>
					<!-- Subject view with non-zero number of items = class="ms-listviewtable" -->
					<xsl:otherwise>discussion-board-container</xsl:otherwise>
				</xsl:choose>
			</xsl:attribute>
			<xsl:call-template name="CTXGeneration"/>
			<tr>
				<td>
					<xsl:if test="not($NoAJAX)">
						<iframe src="javascript:false;" id="FilterIframe{$ViewCounter}" name="FilterIframe{$ViewCounter}" style="display:none" height="0" width="0" FilterLink="{$FilterLink}"/>
					</xsl:if>
					<table id="{$List}-{$View}" Summary="{List/@title}" xmlns:o="urn:schemas-microsoft-com:office:office" o:WebQuerySourceHref="{$HttpPath}&amp;XMLDATA=1&amp;RowLimit=0&amp;View={$View}" width="100%" border="0" cellspacing="0" cellpadding="1" dir="{List/@Direction}">
						<xsl:if test="@BaseViewID='3'">
							<xsl:attribute name="onmouseover">EnsureSelectionHandler(event,this,<xsl:value-of select="$ViewCounter"/>)</xsl:attribute>
						</xsl:if>
						<xsl:attribute name="class"><xsl:choose><!-- Any time we display 0 items in a view = class="ms-viewEmpty" --><xsl:when test="$dvt_RowCount = 0">ms-viewEmpty</xsl:when><xsl:otherwise><xsl:choose><!-- Threaded/Flat view with non-zero number of items = class="ms-disc" --><xsl:when test="(@BaseViewID='1' or @BaseViewID='2')">ms-disc discussion-board</xsl:when><!-- Subject view with non-zero number of items = class="ms-listviewtable" --><xsl:otherwise>ms-listviewtable</xsl:otherwise></xsl:choose></xsl:otherwise></xsl:choose></xsl:attribute>
						<xsl:apply-templates select="." mode="full"/>
					</table>
				</td>
			</tr>
			<xsl:if test="$dvt_RowCount = 0">
				<tr>
					<td>
						<table width="100%" border="0" dir="{List/@Direction}">
							<xsl:call-template name="EmptyTemplate"/>
						</table>
					</td>
				</tr>
			</xsl:if>
		</table>
		<xsl:call-template name="pagingButtons"/>
		<xsl:if test="Toolbar[@Type='Freeform'] or ($MasterVersion=4 and Toolbar[@Type='Standard'])">
			<xsl:call-template name="Freeform">
				<xsl:with-param name="AddNewText">
					<xsl:choose>
						<xsl:when test="List/@TemplateType='108'">
							<xsl:value-of select="$Rows/@resource.wss.Add_New_Discussion"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="$Rows/@resource.wss.addnewitem"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
				<xsl:with-param name="ID">
					<xsl:choose>
						<xsl:when test="List/@TemplateType='108'">idHomePageNewDiscussion</xsl:when>
						<xsl:otherwise>idHomePageNewItem</xsl:otherwise>
					</xsl:choose>
				</xsl:with-param>
			</xsl:call-template>
		</xsl:if>
	</xsl:template>
</xsl:stylesheet>
