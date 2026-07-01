<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Offline_Streamer._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main class="app-hero">
        <div class="row align-items-center">
        <div class="col-md-7">
                <h1>Presenting Scrapify an</h1>
                <h1 class="display-4" style="font-size:70px; font-family:Arial; ">Offline Streamer</h1>
                <p class="lead text-muted">Authenticate with Spotify, view your liked songs, and download them locally for offline playback.</p>

                <ul class="list-unstyled">
                    <li class="mb-2">• Secure OAuth 2.0 authentication with Spotify</li>
                    <li class="mb-2">• Browse and select liked tracks</li>
                    <li class="mb-2">• Download MP3 files to your machine</li>
                    <li class="mb-2">• Stream on this application or your built in audio player</li>
                </ul>

                <p class="mt-3">
                    <a class="btn btn-red btn-lg" runat="server" href="~/Downloads">Login with Spotify</a>
                    <a class="btn btn-outline-light btn-lg ms-2" runat="server" href="~/About">Learn More</a>
                    
                </p>
            </div>

            <div class="col-md-5" style="padding-top:50px">
                <image class="img-fluid" src="laga.png" alt="App preview (placeholder)" />
        </div>
        </div>
    </main>

    <section class="mt-5">
        <h2 class="section-title">How it works</h2>
        <div class="row g-4">
            <div class="col-md-4">
                <div class="card-dark p-3 h-100">
                    <div class="d-flex align-items-start">
                        <div class="icon-placeholder me-3">🔐</div>
                        <div>
                            <h5>Authenticate</h5>
                            <p class="text-muted mb-0">Sign in with Spotify to grant access to your saved tracks securely via OAuth 2.0.</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card-dark p-3 h-100">
                    <div class="d-flex align-items-start">
                        <div class="icon-placeholder me-3">📜</div>
                        <div>
                            <h5>View Liked Songs</h5>
                            <p class="text-muted mb-0">Browse your liked songs, preview metadata, and choose which tracks to download.</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-4">
                <div class="card-dark p-3 h-100">
                    <div class="d-flex align-items-start">
                        <div class="icon-placeholder me-3">⬇️</div>
                        <div>
                            <h5>Download</h5>
                            <p class="text-muted mb-0">Download selected tracks to your machine. (Downloader will save files locally.)</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="mt-5">
        <h2 class="section-title">Preview</h2>
        <div class="placeholder-img">
            <asp:Image ImageUrl="preview.png" runat="server" />
        </div>
    </section>

</asp:Content>
