<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Downloads.aspx.cs" Inherits="Offline_Streamer.Downloads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div style="background-image:url('output.jpeg'); background-size:cover; background-position:center; background-repeat:no-repeat; min-height:100vh; display:flex; align-items:center; justify-content:center; padding:2rem;">
        <div style="display:flex; align-items:center; justify-content:center;">
            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/images.png" Style="max-width:320px; width:40vw; height:auto; padding:150px 120px 110px 140px" OnClick="ImageButton1_Click1" />
        </div>
    </div>
      <asp:GridView ID="SongsGrid" runat="server" AutoGenerateColumns="false" CssClass="songs-grid" DataKeyNames="id"
      OnRowDeleting="SongsGrid_RowDeleting" EmptyDataText="No songs found.">
      <Columns>
          <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="true" />
          <asp:BoundField DataField="SongName" HeaderText="Song" />
          <asp:BoundField DataField="ArtistName" HeaderText="Artist" />
          <asp:CommandField ShowDeleteButton="True" />
      </Columns>
  </asp:GridView>

    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click1" Text="Download given Songs" />

</asp:Content>
