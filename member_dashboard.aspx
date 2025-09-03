<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="member_dashboard.aspx.cs" Inherits="member_dashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="jumbotron text-center bg-light shadow-sm">
            <h1 class="display-4">Member Dashboard</h1>
            <p class="lead">Welcome, <asp:Label ID="lblMemberName" runat="server" Font-Bold="true"></asp:Label>!</p>
            <hr class="my-4">
            <p>Here you can manage your account and view your issued books.</p>
        </div>

        <div class="row text-center mb-5">
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm fade-in">
                    <div class="card-body">
                        <i class="fas fa-book-open fa-4x text-info mb-3"></i>
                        <h5 class="card-title">My Issued Books</h5>
                        <p class="card-text">View the list of books you have issued from the library.</p>
                        <a href="#issued-books" class="btn btn-info mt-2">View My Books</a>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="card h-100 shadow-sm fade-in" style="animation-delay: 0.2s;">
                    <div class="card-body">
                        <i class="fas fa-book-reader fa-4x text-primary mb-3"></i>
                        <h5 class="card-title">Browse All Books</h5>
                        <p class="card-text">Search and explore the entire collection of books available in the library.</p>
                        <a href="book_list_for_members.aspx" class="btn btn-primary mt-2">Browse Books</a>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" id="issued-books">
            <div class="col-12">
                <div class="card shadow-sm">
                    <div class="card-header bg-secondary text-white text-center">
                        <h4 class="mb-0">Books Issued to Me</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="GridViewIssuedBooks" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" EmptyDataText="No books issued yet.">
                            <Columns>
                                <asp:BoundField DataField="book_name" HeaderText="Book Name" />
                                <asp:BoundField DataField="issue_date" HeaderText="Issue Date" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:d}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>