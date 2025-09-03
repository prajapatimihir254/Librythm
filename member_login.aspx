<%@ Page Title="Member Login" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="member_login.aspx.cs" Inherits="member_login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6 col-lg-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-info text-white text-center">
                        <i class="fas fa-user-circle fa-2x"></i>
                        <h4 class="mb-0 mt-2">Member Login</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtMemberID">Member ID</label>
                            <asp:TextBox ID="txtMemberID" runat="server" CssClass="form-control" placeholder="Enter Member ID"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter Password"></asp:TextBox>
                        </div>
                        <div class="form-group text-center">
                            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-info btn-block" OnClick="btnLogin_Click" />
                        </div>
                        <div class="text-center mt-3">
                            <a href="member_signup.aspx">Don't have an account? Sign up here.</a>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>