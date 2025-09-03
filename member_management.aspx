<%@ Page Title="" Language="C#" MasterPageFile="~/site.master" AutoEventWireup="true" CodeFile="member_management.aspx.cs" Inherits="member_management" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-5">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <i class="fas fa-user-plus"></i>
                        <h4 class="mb-0 mt-2">Add New Member</h4>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="txtMemberID">Member ID</label>
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
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter Password"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtPincode">Pincode</label>
                            <asp:TextBox ID="txtPincode" runat="server" CssClass="form-control" placeholder="Enter Pincode"></asp:TextBox>
                        </div>
                         <div class="form-group">
                            <label for="txtAddress">Full Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter Address"></asp:TextBox>
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
                        <h4 class="mb-0 mt-2">Members List</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="GridView1" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" DataKeyNames="member_id" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" OnRowDeleting="GridView1_RowDeleting" PagerSettings-Mode="NumericFirstLast">
                                <Columns>
                                    <asp:BoundField DataField="member_id" HeaderText="ID" ReadOnly="true" />
                                    <asp:BoundField DataField="full_name" HeaderText="Name" />
                                    <asp:BoundField DataField="contact_no" HeaderText="Contact" />
                                    <asp:BoundField DataField="email" HeaderText="Email" />
                                    <asp:BoundField DataField="account_status" HeaderText="Status" />
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