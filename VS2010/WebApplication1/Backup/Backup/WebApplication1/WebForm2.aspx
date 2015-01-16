<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebForm2.aspx.vb" Inherits="WebApplication1.WebForm2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
       <asp:Button ID="forceData" runat="server" Text="Load Current Data" />
    </p>
   <h1><asp:Label ID="debugText" runat="server" Text="This page has been loaded"></asp:Label></h1>

   <asp:Panel ID="Panel1" runat="server">
   <h2>
       This is Panel 1!</h2>

       <p>
           <asp:FileUpload ID="FileUpload1" runat="server" />
       </p>
        
       <asp:Button ID="Upload" runat="server" Text="Begin File Upload" />
       <br />
       <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
           ControlToValidate="FileUpload1" 
           ErrorMessage="Oops!  It looks like you selected the wrong file, Please locate 'tickets.xlsx'" 
           ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(tickets\.xlsx)$">
        </asp:RegularExpressionValidator>
       <br />
       <asp:Label ID="Label1" runat="server" Text="Please browse for a file to upload."></asp:Label>
   </asp:Panel>
           <asp:Button ID="Button2" runat="server" Text="Hide Panel 1" />

    <br />

   <asp:Panel ID="Panel2" runat="server" Visible="false">
   <h2>
       This is Panel 2!</h2>

       <p>
           <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
               AllowSorting="True" AutoGenerateColumns="False" BackColor="White" 
               BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" 
               DataSourceID="LocalSQL" GridLines="Vertical">
               <AlternatingRowStyle BackColor="#DCDCDC" />
               <Columns>
                   <asp:BoundField DataField="ticketNo" HeaderText="ticketNo" 
                       SortExpression="ticketNo" />
                   <asp:BoundField DataField="firstName" HeaderText="firstName" 
                       SortExpression="firstName" />
                   <asp:BoundField DataField="lastName" HeaderText="lastName" 
                       SortExpression="lastName" />
                   <asp:BoundField DataField="userID" HeaderText="userID" 
                       SortExpression="userID" />
                   <asp:BoundField DataField="location" HeaderText="location" 
                       SortExpression="location" />
                   <asp:BoundField DataField="building" HeaderText="building" 
                       SortExpression="building" />
               </Columns>
               <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
               <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
               <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
               <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
               <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
               <SortedAscendingCellStyle BackColor="#F1F1F1" />
               <SortedAscendingHeaderStyle BackColor="#0000A9" />
               <SortedDescendingCellStyle BackColor="#CAC9C9" />
               <SortedDescendingHeaderStyle BackColor="#000065" />
           </asp:GridView>
           <asp:SqlDataSource ID="LocalSQL" runat="server" 
               ConnectionString="Data Source=OAKD00469894A\SQL_EUS_OAK;Initial Catalog=Import;Persist Security Info=True;User ID=user;Password=kPdnpZn0603" 
               ProviderName="System.Data.SqlClient" 
               SelectCommand="SELECT * FROM [ticketResults]"></asp:SqlDataSource>
       </p>

   </asp:Panel>
       <asp:Button ID="Button3" runat="server" Text="Show Panel 2" />

</asp:Content>
