<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logout.aspx.cs" Inherits="Offline_Streamer.Streaming" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="logout-wrapper">
        <asp:Button ID="BtnLogout" runat="server" Text="Logout" OnClick="BtnLogout_Click" CssClass="btn-logout" BackColor="Black" BorderColor="White" BorderStyle="Dotted" BorderWidth="2px" ForeColor="White" />
    </div>

</asp:Content>
