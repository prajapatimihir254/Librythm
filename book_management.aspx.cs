using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class book_management : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["role"] == null || Session["role"].ToString() != "admin")
        {
            Response.Redirect("adminlogin.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                fillAuthorPublisherDropdowns();
                bindGridView();
            }
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

            SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl(book_name, language, author_name, publisher_name, publish_date, edition, book_cost, no_of_pages, book_description, actual_stock, current_stock) VALUES(@book_name, @language, @author_name, @publisher_name, @publish_date, @edition, @book_cost, @no_of_pages, @book_description, @actual_stock, @current_stock)", con);

            cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
            cmd.Parameters.AddWithValue("@language", ddlLanguage.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@author_name", ddlAuthor.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@publisher_name", ddlPublisher.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
            cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
            cmd.Parameters.AddWithValue("@book_cost", Convert.ToDecimal(txtCost.Text.Trim()));
            cmd.Parameters.AddWithValue("@no_of_pages", Convert.ToInt32(txtPages.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@actual_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@current_stock", Convert.ToInt32(txtActualStock.Text.Trim()));

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Book added successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding book: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl SET book_name=@book_name, language=@language, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, edition=@edition, book_cost=@book_cost, no_of_pages=@no_of_pages, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock WHERE book_id=@book_id", con);

            cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
            cmd.Parameters.AddWithValue("@language", ddlLanguage.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@author_name", ddlAuthor.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@publisher_name", ddlPublisher.SelectedItem.Text);
            cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
            cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
            cmd.Parameters.AddWithValue("@book_cost", Convert.ToDecimal(txtCost.Text.Trim()));
            cmd.Parameters.AddWithValue("@no_of_pages", Convert.ToInt32(txtPages.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@actual_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@current_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Book updated successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Book not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error updating book: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("DELETE FROM book_master_tbl WHERE book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = "Book deleted successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Book not found!";
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting book: " + ex.Message;
        }
    }

    // GridView Row Select
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtBookName.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
        ddlAuthor.SelectedItem.Text = GridView1.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        ddlPublisher.SelectedItem.Text = GridView1.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
        txtActualStock.Text = GridView1.SelectedRow.Cells[5].Text.Replace("&nbsp;", "");

        // To be fully functional, you may need to fetch the remaining details (publish_date, etc.) from the DB
        // and populate them based on the selected book ID.

        // Example:
        // string selectedBookId = txtBookID.Text.Trim();
        // ... (SQL logic to get all book details from DB and populate fields)
    }

    // GridView Row Delete
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            int bookId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value);
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM book_master_tbl WHERE book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", bookId);

            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = "Book deleted successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error deleting book: " + ex.Message;
        }
    }

    // Helper function to bind data to DropDownLists
    private void fillAuthorPublisherDropdowns()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Authors
            SqlCommand cmdAuthor = new SqlCommand("SELECT author_name FROM author_master_tbl", con);
            SqlDataAdapter daAuthor = new SqlDataAdapter(cmdAuthor);
            DataTable dtAuthor = new DataTable();
            daAuthor.Fill(dtAuthor);
            ddlAuthor.DataSource = dtAuthor;
            ddlAuthor.DataTextField = "author_name";
            ddlAuthor.DataValueField = "author_name";
            ddlAuthor.DataBind();

            // Publishers
            SqlCommand cmdPublisher = new SqlCommand("SELECT publisher_name FROM publisher_master_tbl", con);
            SqlDataAdapter daPublisher = new SqlDataAdapter(cmdPublisher);
            DataTable dtPublisher = new DataTable();
            daPublisher.Fill(dtPublisher);
            ddlPublisher.DataSource = dtPublisher;
            ddlPublisher.DataTextField = "publisher_name";
            ddlPublisher.DataValueField = "publisher_name";
            ddlPublisher.DataBind();

            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error loading authors/publishers: " + ex.Message;
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

            SqlCommand cmd = new SqlCommand("SELECT * FROM book_master_tbl", con);
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
        txtBookID.Text = "";
        txtBookName.Text = "";
        txtPublishDate.Text = "";
        txtEdition.Text = "";
        txtCost.Text = "";
        txtPages.Text = "";
        txtDescription.Text = "";
        txtActualStock.Text = "";
    }
}