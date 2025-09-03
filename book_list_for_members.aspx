<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="book_list_for_members.aspx.cs" Inherits="book_list_for_members" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-12 text-center">
                <h3>Explore Our Library Collection</h3>
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
                        <asp:BoundField DataField="current_stock" HeaderText="Available Stock" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>