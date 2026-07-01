<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="Offline_Streamer.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="row g-4 align-items-center" style="padding-top:30px">
            <div class="col-md-8">
                <h2>About the Offline Streamer</h2>
                <p class="text-muted">Offline Streamer helps you export your Spotify liked songs to local files for offline use. It connects to Spotify using OAuth 2.0, reads your saved tracks, and provides a download flow. This project is focused on simple, secure authentication and a clean download experience.</p>

                <h4 class="mt-4">Key principles</h4>
                <ul class="text-muted">
                    <li>Privacy-first: tokens stored securely (session/secure storage) and only used as needed.</li>
                    <li>Clear UX: minimal steps between authentication and download.</li>
                    <li>Respect licensing: this tool is intended for personal use; follow Spotify's terms.</li>
                </ul>
            </div>

            <div class="col-md-4">
                <div class="placeholder-img">
                    <asp:Image ImageUrl="sound.png" runat="server" CssClass="fit-img" />
                </div>
            </div>
        </div>

        <section class="mt-5">
            <h3>Future Enhancements</h3>
            <ol class="text-muted">
                <li>Improve downloader resilience and enable partial-download.</li>
                <li>Add bulk selection and queue management.</li>
                <li>Local file tagging and metadata preservation.</li>
            </ol>
        </section>
    </main>
</asp:Content>
