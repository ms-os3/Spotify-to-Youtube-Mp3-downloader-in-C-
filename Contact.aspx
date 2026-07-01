<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="Offline_Streamer.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
   

        <div class="row g-4">
            <div class="col-md-6">
                 <main aria-labelledby="title">
                    <h2 id="title" style="padding-top:30px">Contact</h2>
                    <p class="text-muted">Questions, feedback or issues? Use the form below or reach out to the team.</p>
                
                    <div class="mb-3">
                        <label class="form-label">Name</label>
                        <asp:TextBox ID="NameText" runat="server" CssClass="form-control" Placeholder="Your name" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Email</label>
                        <asp:TextBox ID="EmailText" runat="server" CssClass="form-control" Placeholder="you@example.com" />
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Message</label>
                        <asp:TextBox ID="MessageText" runat="server" TextMode="MultiLine" Rows="5" CssClass="form-control" Placeholder="Your message" />
                    </div>
                    <div>
                        <asp:Button ID="SendButton" runat="server" CssClass="btn btn-red" Text="Send message" OnClick="SendButton_Click" />
                        <span class="ms-2 text-muted" />
                    </div>
                </main>
            </div>
            <div class="col-md-6">
                    <asp:Image ImageUrl="phone.png" runat="server" style="padding-left:90px;"/>
                    <div class="mt-3">
                        <h5>Other ways to reach us</h5>
                        <p class="text-muted mb-1">Mobile: <a class="link-light">+91 9619436961</a></p>
                        <p class="text-muted mb-1">Email: <a class="link-light" href="mailto:manavshirali@gmail.com">manavshirali@gmail.com</a></p>
    
                    </div>
            </div>

                
                
            
        </div>
    

    
</asp:Content>
