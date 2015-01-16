<?xml version='1.0' encoding="utf-8"?>

<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:path="urn:test">
  <xsl:param name="swc"/>
  <msxsl:script implements-prefix="path" language="JScript">
    function URL(oObj) {
    try{
    return oObj.item(0).url;
    }
    catch(err){
    return "";
    }
    }
  </msxsl:script>

  <xsl:template match="/">


    <xsl:param name ="url">
      <xsl:value-of select="path:URL(.)"/>
    </xsl:param>

    <html>
      <head>
        <title>SAP IT Package Template</title>
        <xsl:if test="not(contains($swc,'true'))">
          <style type="text/css">

            html { margin:0px 0px 0px 0px; padding:0px; }
            body {font-family: arial,helvetica,sans-serif; background-color:#FFFFFF; min-width:500px; padding:10px; margin:0px; font-size:12px; }
            h1,h2,h3,h4,h5,h6 { font-weight:bold; }
            h1 {  font-size:20px; color:#44697D; text-transform:uppercase; overflow:hidden; }
            h2 {  font-size:14px; color:#04357B; background-color:#CCCCCC;  margin:2px 0px 7px 0px;  text-transform:uppercase;  }
            h3 {  font-size:16px;  color:#333; margin-top:6px; margin-bottom:2px;}
            h4,h5,h6 {  margin-top:7px;  margin-bottom:2px;  color:#333; }
            h4 {  font-size:15px; }
            h5 {  font-size:13px; }
            h6 {  font-size:10px; }
            hr {	background-color:blue; }
            p  {  margin:4px 10px 4px 0px; }
            strong,b {  font-weight:bold;}
            ul,ol {  margin:4px 0px 10px 24px; }
            ul {  list-style-type:square; }
            ul ul {  margin:0px 0px 0px 17px; }
            li {  margin:2px 0px 0px 0px; }
            strong 	    {color: #33586C; font-weight: bold; }
            img 	    { border: 3px solid #DDDDDD; margin: 0px; }

            a {          text-decoration:underline;  color:#04357B; margin:0px; }
            a:visited {  color:#04357B; }
            a:hover {    text-decoration:none; }

            .ini 	    {     display: none; }
            .ini div 	    { display: none; }

            .linkbox     	{ float:right;  clear:right;
            width:160px;
            margin:10px 0px 0px 0px; background-color: #FFFFFF;
            border: #ccc 1px solid; }
            .linkbox h6  	{ font-size:11px; color:#333;	text-transform:uppercase; margin:4px 0px 0px 4px; }
            .linkbox div 	{ display:block; margin:1px; width:158px;
            filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=1,StartColorStr='#FFE5E5E5',EndColorStr='#FFBECBD2');  }

            div.contentbox {
            min-width:250px;
            padding: 5px 5px 5px 5px;
            margin:10px 170px 0px 0px;
            background: #fff; border: #ccc 1px solid;
            filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0,StartColorStr='#77DBE0E1',EndColorStr='#00FFFFFF');


            titleDiv { color:#04357B; }
            }

            #header {	margin:10px 0px 0px 0px; border-bottom:#000 1px solid; }
            #header h1 {margin:0px 0px 0px 0px; }
            #footer { margin:10px 0px 10px 0px; clear:both; border-top:1px #000 solid; font-size:10px; text-transform:uppercase; color:#aaa; }

          </style>
        </xsl:if>
      </head>

      <body>

        <div>

          <xsl:if test="(not(contains($url,'install'))and not(contains($url,'uninstall'))) ">
            <xsl:if test="not(contains($swc,'true'))">
              <table border="0" >
                <tr>
                  <td>
                    <img src="supporting_files/SAP_logo.gif" style="border:0px;"></img>
                  </td>
                  <td valign="middle" align="right" style="font-size:20px; color:#44697D; font-weight:bold; text-transform:uppercase; overflow:hidden;" >
                    <xsl:value-of select="Package/Application/@Vendor"/>&#160;
                    <xsl:value-of select="Package/Application/@Name"/>&#160;
                    <xsl:value-of select="Package/Application/@Version"/>
                  </td>
                </tr>
              </table>
            </xsl:if>

            <table border="0">
              <tr >
                <xsl:for-each select="Package/Popup/Buttons/LinkButton">
                  <td valign="middle">
                    <xsl:variable name="LinkSource" select="@link"/>
                    <xsl:variable name="LinkName" select="."/>
                    <FORM METHOD="LINK" ACTION="{$LinkSource}">
                      <INPUT TYPE="submit" VALUE="{$LinkName}" />
                    </FORM>
                  </td>
                </xsl:for-each>
                <xsl:if test="not(contains($swc,'true'))">
                  <td valign="top">
                    <h1>|</h1>
                  </td>
                  <td valign="middle">
                    <FORM METHOD="LINK" ACTION="SAPITSetup.exe">
                      <INPUT TYPE="submit" VALUE="Install Now" />
                    </FORM>
                  </td>
                </xsl:if>
              </tr>
            </table>
          </xsl:if>

          <xsl:if test="Package/@LicenseType='Single'">
            <H2>Notice - Read first</H2>
            <ul>
              <li>According to our data, you have ordered and purchased this software product following your regional procurement process. This has been approved by the responsible managers and the software is now ready for installation on this PC.</li>
              <li>As this software requires the purchase of a separate end user license, it is important that you have ordered a license for every single computer you install this software product on.</li>
              <li>
                <strong>
                  If you have received this notification in error then <font color="#FF0000">Click Exit</font> and notify your local IT department.
                </strong>
              </li>
            </ul>
          </xsl:if>

          <h2>
            General Information
          </h2>
          <div id="general">
            <ul>
              <xsl:if test="not(contains($url,'uninstall'))">
                <xsl:copy-of select="Package/Popup/GeneralInfo" />
              </xsl:if>
              <xsl:if test="contains($url,'uninstall')">
                <xsl:copy-of select="Package/Popup/GeneralInfoUninstall" />
              </xsl:if>
              
            </ul>
          </div>

          <h2>
            Known Issues
          </h2>
          <div id="issues">
            <ul>
              <xsl:if test="not(contains($url,'uninstall'))">
                <xsl:copy-of select="Package/Popup/KnownIssues" />
              </xsl:if>
              <xsl:if test="contains($url,'uninstall')">
                <xsl:copy-of select="Package/Popup/KnownIssuesUninstall" />
              </xsl:if>
            </ul>
          </div>

          <h2>
            Requirements
          </h2>
          <div id="reqs">
            <ul>
              <xsl:if test="not(contains($url,'uninstall'))">
                <xsl:copy-of select="Package/Popup/Requirements" />
                <li>
                  Reboot Required:
                  <xsl:if test="Package/Application/Installation/@RebootOnComplete='True'">
                    <b> Yes</b>
                  </xsl:if>
                  <xsl:if test="Package/Application/Installation/@RebootOnComplete='False'">
                    <b> No</b>
                  </xsl:if>
                </li>
                <xsl:if test="count(Package/Application/Installation/Constraints/Process)>0">
                  <li>Programs which must be closed: </li>
                  <xsl:for-each select="Package/Application/Installation/Constraints/Process">
                    <b>
                      <xsl:value-of select="@Description"/>
                    </b>
                    <xsl:if test="position()!=last()">, </xsl:if>
                  </xsl:for-each>
                </xsl:if>
              </xsl:if>
              <xsl:if test="contains($url,'uninstall')">
                <xsl:copy-of select="Package/Popup/RequirementsUninstall" />
                <li>
                  Reboot Required:
                  <xsl:if test="Package/Application/Uninstallation/@RebootOnComplete='True'">
                    <b> Yes</b>
                  </xsl:if>
                  <xsl:if test="Package/Application/Uninstallation/@RebootOnComplete='False'">
                    <b> No</b>
                  </xsl:if>
                </li>

                <xsl:if test="count(Package/Application/Uninstallation/Constraints/Process)>0">
                  <li>Programs which must be closed: </li>
                  <xsl:for-each select="Package/Application/Uninstallation/Constraints/Process">
                    <b>
                      <xsl:value-of select="@Description"/>
                    </b>
                    <xsl:if test="position()!=last()">, </xsl:if>
                  </xsl:for-each>
                </xsl:if>

              </xsl:if>

              <li>Models supported: </li>
              <xsl:for-each select="Package/Compatibility/Model">
                <b>
                  <xsl:value-of select="."/>
                </b>
                <xsl:if test="position()!=last()">, </xsl:if>
              </xsl:for-each>

            </ul>
          </div>
          <xsl:if test="not(contains($url,'uninstall'))">
            <h2>
              Installation
            </h2>
            <div id="install">
              <ul>
                <xsl:copy-of select="Package/Popup/Installation" />
              </ul>
            </div>
          </xsl:if>
          <xsl:if test="contains($url,'uninstall')">
            <h2>
              Uninstallation
            </h2>
            <div id="install">
              <ul>
                <xsl:copy-of select="Package/Popup/Uninstallation" />
              </ul>
            </div>
          </xsl:if>

          <h2>
            Help and Contact
          </h2>
          <div id="help">
            <ul>
              <xsl:copy-of select="Package/Popup/Help" />
            </ul>
          </div>
        </div>

      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>