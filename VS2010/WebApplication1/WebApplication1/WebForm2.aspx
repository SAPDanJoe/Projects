<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="WebForm2.aspx.vb" Inherits="WebApplication1.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <p>
       <asp:Button ID="LoadData" runat="server" Text="Load Current Data" Visible="false"/>
    </p>
   <h1>
   <asp:Label ID="debugText" runat="server" Text="This page has been loaded" Visible="false"></asp:Label>
   </h1>

   <asp:Panel ID="Panel1" runat="server">
   <h2>
       PLease Locate <strong>&#39;Tickets.xlsx&#39;</strong> to upload</h2>

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
           <asp:Button ID="Button2" runat="server" Text="Hide Panel 1" visible="false"/>

    <br />

        <asp:Button ID="RunProcedure" runat="server" Text="Process xlsx" />

        <asp:Label ID="ProcLabel" runat="server" Visible="False"></asp:Label>

    <br />
       <asp:Button ID="Button3" runat="server" Text="Show Results" />

   <asp:Panel ID="Panel2" runat="server" Visible="false">
   <h2>
       Tickets that need to be updated with building information</h2>

       <p>
           *Tickets listed more than once indicate that more than one user was found in the directory.</p>

       <p>
           <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
               AllowSorting="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" 
               EmptyDataText="There are no data records to display." BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Horizontal">
               <AlternatingRowStyle BackColor="#F7F7F7" />
               <Columns>
                   <asp:BoundField DataField="ticketNo" HeaderText="Ticket Number" 
                       SortExpression="ticketNo" />
                   <asp:BoundField DataField="firstName" HeaderText="First Name" 
                       SortExpression="firstName" />
                   <asp:BoundField DataField="lastName" HeaderText="LastName" 
                       SortExpression="lastName" />
                   <asp:BoundField DataField="userID" HeaderText="I/C/D Number" 
                       SortExpression="userID" />
                   <asp:BoundField DataField="location" HeaderText="Location" 
                       SortExpression="location" />
                   <asp:BoundField DataField="building" HeaderText="Building" 
                       SortExpression="building" />
               </Columns>
               <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
               <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
               <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
               <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
               <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
               <SortedAscendingCellStyle BackColor="#F4F4FD" />
               <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
               <SortedDescendingCellStyle BackColor="#D8D8F0" />
               <SortedDescendingHeaderStyle BackColor="#3E3277" />
           </asp:GridView>
           <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
               ConnectionString="Persist Security Info=False;Integrated Security=true;Server=OAKD00465226A;Database=Import;User Id=user;Password=kPdnpZn0603;" 
               ProviderName="<%$ ConnectionStrings:ImportConnectionString1.ProviderName %>" 
               SelectCommand="select ticketsmissinglocations.[Ticket Number] as ticketNo, ticketResults.building , ticketsmissinglocations.[Affected User/Req#], firstName, lastName, userID,location from ticketsmissinglocations left join ticketResults on TicketsMissingLocations.[Ticket Number] = ticketResults.ticketNo">
           </asp:SqlDataSource>
       </p>

   </asp:Panel>
       
</asp:Content>
