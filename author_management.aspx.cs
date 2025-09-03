using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class author_management : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        // GridView ko populate karne ke liye function call
        if (!IsPostBack)
        {
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

            SqlCommand cmd = new SqlCommand("INSERT INTO author_master_tbl(author_name) VALUES(@author_name)", con);
            cmd.Parameters.AddWithValue("@author_name", txtAuthorName.Text.Trim());

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Author added successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding author: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("UPDATE author_master_tbl SET author_name=@author_name WHERE author_id=@author_id", con);
            cmd.Parameters.AddWithValue("@author_name", txtAuthorName.Text.Trim());
            cmd.Parameters.AddWithValue("@author_id", txtAuthorID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Author updated successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Author not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating author: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("DELETE FROM author_master_tbl WHERE author_id=@author_id", con);
            cmd.Parameters.AddWithValue("@author_id", txtAuthorID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Author deleted successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Author not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting author: " + ex.Message;
        }
    }

    // GridView Row Select
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Selected row se data form mein fill karo
        txtAuthorID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtAuthorName.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", ""); // &nbsp; remove karo
    }

    // GridView Row Delete
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int authorId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM author_master_tbl WHERE author_id=@author_id", con);
            cmd.Parameters.AddWithValue("@author_id", authorId);

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Author deleted successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting author: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("SELECT * FROM author_master_tbl", con);
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
        txtAuthorID.Text = "";
        txtAuthorName.Text = "";
    }
}