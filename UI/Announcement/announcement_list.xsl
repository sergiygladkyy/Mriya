<xsl:stylesheet xmlns:x="http://www.w3.org/2001/XMLSchema" xmlns:d="http://schemas.microsoft.com/sharepoint/dsp" version="1.0" exclude-result-prefixes="xsl msxsl ddwrt" xmlns:ddwrt="http://schemas.microsoft.com/WebParts/v2/DataView/runtime" xmlns:asp="http://schemas.microsoft.com/ASPNET/20" xmlns:__designer="http://schemas.microsoft.com/WebParts/v2/DataView/designer" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:SharePoint="Microsoft.SharePoint.WebControls" xmlns:ddwrt2="urn:frontpage:internal" ddwrt:oob="true">
  <xsl:import href="/_layouts/xsl/main.xsl"/>

  <!--<xsl:template match="/">
    <xmp><xsl:copy-of select="."/></xmp> 
  </xsl:template>-->
  
  <xsl:variable name="ViewClassName">
    <xsl:choose>
      <xsl:when test="$dvt_RowCount=0">announcement-list ms-emptyView</xsl:when>
      <xsl:otherwise>announcement-list ms-listviewtable</xsl:otherwise>
    </xsl:choose>
  </xsl:variable>

</xsl:stylesheet>