<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="publisher_management.aspx.cs" Inherits="publisher_management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-5">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-building"></i>
                        <h4 class="mb-0 mt-2">Add New Publisher</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtPublisherID">Publisher ID</label>
                            <asp:TextBox ID="txtPublisherID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPublisherName">Publisher Name</label>
                            <asp:TextBox ID="txtPublisherName" runat="server" CssClass="form-control" placeholder="Enter Publisher Name"></asp:TextBox>
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
                        <h4 class="mb-0 mt-2">Publishers List</h4>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="publisher_id" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting">
                            <Columns>
                                <asp:BoundField DataField="publisher_id" HeaderText="ID" ReadOnly="true" />
                                <asp:BoundField DataField="publisher_name" HeaderText="Publisher Name" />
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
