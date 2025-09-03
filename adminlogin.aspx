<%@ Page Title="Admin Login" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="adminlogin.aspx.cs" Inherits="adminlogin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-4">
                <div class="card shadow-lg fade-in">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-user-shield fa-3x mb-2"></i>
                        <h4 class="mb-0 mt-2">Admin Login</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtAdminID">Admin ID</label>
                            <asp:TextBox ID="txtAdminID" runat="server" CssClass="form-control" placeholder="Enter Admin ID"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>
                        </div>
                        <div class="form-group text-center mt-4">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger mt-3 d-block text-center"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
