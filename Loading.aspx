<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Loading.aspx.cs" Inherits="Offline_Streamer.Loading" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="refresh" content="2" />
    <title>Redirecting</title>
    <link href="Loading.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="body">
            <span>
                <span></span>
                <span></span>
                <span></span>
                <span></span>
            </span>
            <div class="base">
                <span></span>
                <div class="face"></div>
            </div>
        </div>

        <div class="longfazers">
            <span></span>
            <span></span>
            <span></span>
            <span></span>
        </div>
        <h1>Downloading Songs</h1>
        <asp:Label runat="server" ID="StatusLabel" ></asp:Label>
    </form>
</body>
</html>
