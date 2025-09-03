<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="homepage.aspx.cs" Inherits="homepage" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron text-center">
        <h1>Welcome to Librythm</h1>
        <p class="lead">Your complete solution for Library Management.</p>
    </div>

    <asp:Panel ID="pnlGuestZone" runat="server">
        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fas fa-user-circle fa-4x text-info mb-3"></i>
                        <h5 class="card-title">Member Zone</h5>
                        <p class="card-text">Access your personal dashboard, browse books, and view your account history.</p>
                        <a href="member_login.aspx" class="btn btn-info mt-2">Member Login</a>
                        <a href="member_signup.aspx" class="btn btn-outline-info mt-2 ml-2">Sign Up</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fas fa-user-shield fa-4x text-primary mb-3"></i>
                        <h5 class="card-title">Admin Zone</h5>
                        <p class="card-text">Manage the entire library system including books, members, and more.</p>
                        <a href="adminlogin.aspx" class="btn btn-primary mt-2">Admin Login</a>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row mt-5">
        <div class="col-12 text-center">
            <h3>Explore Our Collection</h3>
            <p>Search for your favorite books by name, author, or genre.</p>
            <div class="input-group mb-3">
                <asp:TextBox ID="txtSearchBook" runat="server" CssClass="form-control" placeholder="Search books..."></asp:TextBox>
                <div class="input-group-append">
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-secondary" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-3">
        <div class="col-12">
            <asp:GridView ID="GridViewBooks" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" EmptyDataText="No books found.">
                <Columns>
                    <asp:BoundField DataField="book_name" HeaderText="Book Name" />
                    <asp:BoundField DataField="author_name" HeaderText="Author" />
                    <asp:BoundField DataField="publisher_name" HeaderText="Publisher" />
                    <asp:BoundField DataField="language" HeaderText="Language" />
                    <asp:BoundField DataField="current_stock" HeaderText="Available" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>