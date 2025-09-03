<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="book_management.aspx.cs" Inherits="book_management" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-5">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-book"></i>
                        <h4 class="mb-0 mt-2">Book Operations</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtBookID">Book ID</label>
                            <asp:TextBox ID="txtBookID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtBookName">Book Name</label>
                            <asp:TextBox ID="txtBookName" runat="server" CssClass="form-control" placeholder="Enter Book Name"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="ddlLanguage">Language</label>
                            <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control">
                                <asp:ListItem Text="English" Value="English"></asp:ListItem>
                                <asp:ListItem Text="Hindi" Value="Hindi"></asp:ListItem>
                                <asp:ListItem Text="Gujarati" Value="Gujarati"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="ddlAuthor">Author</label>
                            <asp:DropDownList ID="ddlAuthor" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="ddlPublisher">Publisher</label>
                            <asp:DropDownList ID="ddlPublisher" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <label for="txtPublishDate">Publish Date</label>
                            <asp:TextBox ID="txtPublishDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtEdition">Edition</label>
                            <asp:TextBox ID="txtEdition" runat="server" CssClass="form-control" placeholder="Enter Edition"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtCost">Book Cost</label>
                            <asp:TextBox ID="txtCost" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter Cost"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPages">No. of Pages</label>
                            <asp:TextBox ID="txtPages" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter Pages"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtDescription">Description</label>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter Description"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtActualStock">Actual Stock</label>
                            <asp:TextBox ID="txtActualStock" runat="server" CssClass="form-control" TextMode="Number" placeholder="Enter Actual Stock"></asp:TextBox>
                        </div>
                        <div class="d-flex justify-content-between mt-4">
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
                        <h4 class="mb-0 mt-2">Books List</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="book_id" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting" PagerSettings-Mode="NumericFirstLast">
                                <Columns>
                                    <asp:BoundField DataField="book_id" HeaderText="ID" ReadOnly="true" />
                                    <asp:BoundField DataField="book_name" HeaderText="Book Name" />
                                    <asp:BoundField DataField="author_name" HeaderText="Author" />
                                    <asp:BoundField DataField="publisher_name" HeaderText="Publisher" />
                                    <asp:BoundField DataField="actual_stock" HeaderText="Actual Stock" />
                                    <asp:BoundField DataField="current_stock" HeaderText="Current Stock" />
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

