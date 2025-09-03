<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="issue_book.aspx.cs" Inherits="issue_book" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-handshake"></i>
                        <h4 class="mb-0 mt-2">Issue Book</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group row">
                            <label for="txtMemberID" class="col-sm-4 col-form-label">Member ID</label>
                            <div class="col-sm-8 d-flex">
                                <asp:TextBox ID="txtMemberID" runat="server" CssClass="form-control" placeholder="Enter Member ID"></asp:TextBox>
                                <asp:LinkButton ID="btnGoMember" runat="server" CssClass="btn btn-outline-info ml-2" OnClick="btnGoMember_Click"><i class="fas fa-search"></i> Go</asp:LinkButton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="txtMemberName" class="col-sm-4 col-form-label">Member Name</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMemberName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="txtBookID" class="col-sm-4 col-form-label">Book ID</label>
                            <div class="col-sm-8 d-flex">
                                <asp:TextBox ID="txtBookID" runat="server" CssClass="form-control" placeholder="Enter Book ID"></asp:TextBox>
                                <asp:LinkButton ID="btnGoBook" runat="server" CssClass="btn btn-outline-info ml-2" OnClick="btnGoBook_Click"><i class="fas fa-search"></i> Go</asp:LinkButton>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="txtBookName" class="col-sm-4 col-form-label">Book Name</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBookName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="txtIssueDate" class="col-sm-4 col-form-label">Issue Date</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtIssueDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label for="txtDueDate" class="col-sm-4 col-form-label">Due Date</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtDueDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                            </div>
                        </div>
                        <div class="d-flex justify-content-center mt-4">
                            <asp:Button ID="btnIssue" runat="server" Text="Issue" CssClass="btn btn-success" OnClick="btnIssue_Click" />
                            <asp:Button ID="btnReturn" runat="server" Text="Return" CssClass="btn btn-warning ml-2" OnClick="btnReturn_Click" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-info text-white text-center">
                        <i class="fas fa-list"></i>
                        <h4 class="mb-0 mt-2">Issued Books List</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="member_id, book_id" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting">
                                <Columns>
                                    <asp:BoundField DataField="member_id" HeaderText="Member ID" />
                                    <asp:BoundField DataField="member_name" HeaderText="Member Name" />
                                    <asp:BoundField DataField="book_id" HeaderText="Book ID" />
                                    <asp:BoundField DataField="book_name" HeaderText="Book Name" />
                                    <asp:BoundField DataField="issue_date" HeaderText="Issue Date" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="due_date" HeaderText="Due Date" DataFormatString="{0:d}" />
                                    <asp:CommandField ShowSelectButton="true" HeaderText="Select" />
                                    <asp:CommandField ShowDeleteButton="true" HeaderText="Delete" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>