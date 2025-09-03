<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="author_management.aspx.cs" Inherits="author_management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-5">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-plus-square"></i>
                        <h4 class="mb-0 mt-2">Add New Author</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtAuthorID">Author ID</label>
                            <asp:TextBox ID="txtAuthorID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtAuthorName">Author Name</label>
                            <asp:TextBox ID="txtAuthorName" runat="server" CssClass="form-control" placeholder="Enter Author Name"></asp:TextBox>
                        </div>
                        <div class="d-flex justify-content-between">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success" OnClick="btnAdd_Click" />
                            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-warning" OnClick="btnUpdate_Click" />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
                        </div>
                    </div>
                </div>
                <div class="mt-3">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                </div>
            </div>
            <div class="col-md-7">
                <div class="card shadow-sm">
                    <div class="card-header bg-info text-white text-center">
                        <i class="fas fa-list"></i>
                        <h4 class="mb-0 mt-2">Authors List</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="author_id" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="author_id" HeaderText="ID" ReadOnly="true" />
                                <asp:BoundField DataField="author_name" HeaderText="Author Name" />
                                <asp:CommandField ShowSelectButton="true" HeaderText="Select" />
                                <asp:CommandField ShowDeleteButton="true" HeaderText="Delete" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

