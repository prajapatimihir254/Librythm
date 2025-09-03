using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class publisher_management : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindGridView();
        }
    }
    // Add Button Click
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("INSERT INTO publisher_master_tbl(publisher_name) VALUES(@publisher_name)", con);
            cmd.Parameters.AddWithValue("@publisher_name", txtPublisherName.Text.Trim());

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Publisher added successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding publisher: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("UPDATE publisher_master_tbl SET publisher_name=@publisher_name WHERE publisher_id=@publisher_id", con);
            cmd.Parameters.AddWithValue("@publisher_name", txtPublisherName.Text.Trim());
            cmd.Parameters.AddWithValue("@publisher_id", txtPublisherID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Publisher updated successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Publisher not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating publisher: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("DELETE FROM publisher_master_tbl WHERE publisher_id=@publisher_id", con);
            cmd.Parameters.AddWithValue("@publisher_id", txtPublisherID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Publisher deleted successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Publisher not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting publisher: " + ex.Message;
        }
    }

    // GridView Row Select
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPublisherID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtPublisherName.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
    }

    // GridView Row Delete
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int publisherId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM publisher_master_tbl WHERE publisher_id=@publisher_id", con);
            cmd.Parameters.AddWithValue("@publisher_id", publisherId);

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Publisher deleted successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting publisher: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("SELECT * FROM publisher_master_tbl", con);
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
        txtPublisherID.Text = "";
        txtPublisherName.Text = "";
    }
}