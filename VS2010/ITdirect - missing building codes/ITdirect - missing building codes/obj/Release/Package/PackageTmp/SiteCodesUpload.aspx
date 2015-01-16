<%@ Page Title="ITdirect Missing Building Tool" Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="false"
    CodeBehind="SiteCodesUpload.aspx.vb" Inherits="ITdirect___missing_building_codes._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    </asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">

<h2><asp:Label ID="DebugTXT" runat="Server"></asp:Label></h2>
    <p>&nbsp;</p>

<asp:Button id="toggle" Text="Toggle Forms" runat="server" />

<asp:Panel ID="uploadForm" runat="server">
<h1>Upload Form:</h1>
    <p>
        This is a test</p>
    <p>
        &nbsp;</p>
</asp:Panel>

<asp:Panel ID="DataForm" runat="server">
<h1>Data Form:</h1>
    <p>
        This is also a test</p>
    <p>
        &nbsp;</p>
</asp:Panel>

</asp:Content>



