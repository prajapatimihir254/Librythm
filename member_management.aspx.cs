using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class member_management : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            getNewMemberID();
            bindGridView();
        }
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Check if member ID already exists before adding
            SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM member_master_tbl WHERE member_id = @member_id", con);
            checkCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            int count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count > 0)
            {
                lblMessage.Text = "Error: Member ID already exists. Please refresh.";
                con.Close();
                return;
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO member_master_tbl(member_id, full_name, contact_no, email, password, pincode, full_address, account_status) VALUES(@member_id, @full_name, @contact_no, @email, @password, @pincode, @full_address, 'active')", con);

            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@full_name", txtFullName.Text.Trim());
            cmd.Parameters.AddWithValue("@contact_no", txtContact.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@pincode", txtPincode.Text.Trim());
            cmd.Parameters.AddWithValue("@full_address", txtAddress.Text.Trim());

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Member added successfully!";
            clearFields();
            getNewMemberID();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding member: " + ex.Message;
        }
    }

    // Update Button Click
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("UPDATE member_master_tbl SET full_name=@full_name, contact_no=@contact_no, email=@email, password=@password, pincode=@pincode, full_address=@full_address WHERE member_id=@member_id", con);

            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@full_name", txtFullName.Text.Trim());
            cmd.Parameters.AddWithValue("@contact_no", txtContact.Text.Trim());
            cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@password", txtPassword.Text.Trim());
            cmd.Parameters.AddWithValue("@pincode", txtPincode.Text.Trim());
            cmd.Parameters.AddWithValue("@full_address", txtAddress.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Member updated successfully!";
                clearFields();
                getNewMemberID();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Member not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating member: " + ex.Message;
        }
    }

    // Delete Button Click
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM member_master_tbl WHERE member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Member deleted successfully!";
                clearFields();
                getNewMemberID();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Member not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting member: " + ex.Message;
        }
    }

    // GridView Row Select
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMemberID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtFullName.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        txtContact.Text = GridView1.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        txtEmail.Text = GridView1.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        // Password and address are not populated for security
    }

    // GridView Row Delete
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string memberId = GridView1.DataKeys[e.RowIndex].Value.ToString();
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM member_master_tbl WHERE member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberId);

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Member deleted successfully!";
            clearFields();
            getNewMemberID();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting member: " + ex.Message;
        }
    }

    // Helper function to bind data to GridView
    private void bindGridView()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT * FROM member_master_tbl", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridView1.DataSource = dt;
            GridView1.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading data: " + ex.Message;
        }
    }

    // Helper function to clear form fields
    private void clearFields()
    {
        txtFullName.Text = "";
        txtContact.Text = "";
        txtEmail.Text = "";
        txtPassword.Text = "";
        txtPincode.Text = "";
        txtAddress.Text = "";
    }

    // Function to generate and set the new Member ID
    private void getNewMemberID()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(CAST(SUBSTRING(member_id, 3, LEN(member_id)-2) AS INT)), 0) FROM member_master_tbl", con);
            int lastID = Convert.ToInt32(cmd.ExecuteScalar());
            int newID = lastID + 1;
            txtMemberID.Text = "M-" + newID.ToString("D4"); // "M-0001", "M-0002" format

            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error generating Member ID: " + ex.Message;
        }
    }
}