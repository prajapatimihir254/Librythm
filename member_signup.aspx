<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="member_signup.aspx.cs" Inherits="member_signup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card shadow-sm">
                    <div class="card-header bg-info text-white text-center">
                        <i class="fas fa-user-plus fa-2x"></i>
                        <h4 class="mb-0 mt-2">Member Sign Up</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtMemberID">Your Member ID</label>
                            <asp:TextBox ID="txtMemberID" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtFullName">Full Name</label>
                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Enter Full Name"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtContact">Contact No</label>
                            <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" placeholder="Enter Contact No"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtEmail">Email ID</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter Email ID"></asp:TextBox>
                        </div>
                         <div class="form-group">
                            <label for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Create Password"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPincode">Pincode</label>
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="Enter Pincode"></asp:TextBox>
                        </div>
                         <div class="form-group">
                            <label for="txtAddress">Full Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter Address"></asp:TextBox>
                        </div>
                        <div class="form-group text-center">
                            <asp:Button ID="btnSignUp" runat="server" Text="Sign Up" CssClass="btn btn-info btn-block" OnClick="btnSignUp_Click" />
                        </div>
                        <div class="text-center mt-3">
                            <a href="member_login.aspx">Already have an account? Login here.</a>
                        </div>
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>