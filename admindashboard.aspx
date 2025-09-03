<%@ Page Title="Admin Dashboard" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="admindashboard.aspx.cs" Inherits="admindashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="jumbotron text-center bg-light shadow-sm">
            <h1 class="display-4">Admin Dashboard</h1>
            <p class="lead">Welcome, <asp:Label ID="lblAdminID" runat="server" Font-Bold="true"></asp:Label>!</p>
            <hr class="my-4">
            <p>Manage all aspects of the library system from here.</p>
        </div>

        <div class="row text-center mb-5">
            <div class="col-md-4 mb-4">
                <div class="card bg-primary text-white h-100 shadow-sm fade-in">
                    <div class="card-body">
                        <i class="fas fa-book fa-3x"></i>
                        <h5 class="card-title mt-2">Total Books</h5>
                        <asp:Label ID="lblTotalBooks" runat="server" CssClass="display-4" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card bg-success text-white h-100 shadow-sm fade-in" style="animation-delay: 0.2s;">
                    <div class="card-body">
                        <i class="fas fa-handshake fa-3x"></i>
                        <h5 class="card-title mt-2">Books Issued</h5>
                        <asp:Label ID="lblIssuedBooks" runat="server" CssClass="display-4" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card bg-info text-white h-100 shadow-sm fade-in" style="animation-delay: 0.4s;">
                    <div class="card-body">
                        <i class="fas fa-users fa-3x"></i>
                        <h5 class="card-title mt-2">Total Members</h5>
                        <asp:Label ID="lblTotalMembers" runat="server" CssClass="display-4" Font-Bold="true"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row text-center">
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body">
                        <i class="fas fa-book-reader fa-4x text-primary mb-3"></i>
                        <h5 class="card-title">Book Operations</h5>
                        <p class="card-text">Add, update, or delete books from the library inventory.</p>
                        <a href="book_management.aspx" class="btn btn-outline-primary mt-2">Manage Books</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body">
                        <i class="fas fa-user-friends fa-4x text-success mb-3"></i>
                        <h5 class="card-title">Member Operations</h5>
                        <p class="card-text">Manage member details and their accounts.</p>
                        <a href="member_management.aspx" class="btn btn-outline-success mt-2">Manage Members</a>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body">
                        <i class="fas fa-handshake fa-4x text-info mb-3"></i>
                        <h5 class="card-title">Issue/Return Books</h5>
                        <p class="card-text">Issue and return books to/from members.</p>
                        <a href="issue_book.aspx" class="btn btn-outline-info mt-2">Issue/Return</a>
                    </div>
                </div>
            </div>
        </div>
        <div class="row text-center mt-4">
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body">
                        <i class="fas fa-pen-nib fa-4x text-danger mb-3"></i>
                        <h5 class="card-title">Author Management</h5>
                        <p class="card-text">Add, update, or delete authors.</p>
                        <a href="author_management.aspx" class="btn btn-outline-danger mt-2">Manage Authors</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body">
                        <i class="fas fa-building fa-4x text-warning mb-3"></i>
                        <h5 class="card-title">Publisher Management</h5>
                        <p class="card-text">Add, update, or delete publishers.</p>
                        <a href="publisher_management.aspx" class="btn btn-outline-warning mt-2">Manage Publishers</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>