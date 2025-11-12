using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public struct ForeignCosts
{
    public decimal USD;
    public decimal EUR;
    public decimal GBP;
}
public partial class book_management : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    private const decimal USDRate = 75.00m; // INR 75 = 1 USD
    private const decimal EURRate = 85.00m; // INR 85 = 1 EUR
    private const decimal GBPRate = 95.00m; // INR 95 = 1 GBP

    

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
                bindGridView();
            }
        }
    }

    // Function to Lookup/Insert Author/Publisher and return its ID
    private int GetOrCreateMasterDataID(string name, string tableName, string idColumn, string nameColumn, SqlConnection con)
    {
        int id = -1;

        // 1. Check if the name exists
        SqlCommand checkCmd = new SqlCommand($"SELECT {idColumn} FROM {tableName} WHERE {nameColumn} = @Name", con);
        checkCmd.Parameters.AddWithValue("@Name", name);

        object result = checkCmd.ExecuteScalar();

        if (result != null)
        {
            id = Convert.ToInt32(result); // Found existing ID
        }
        else
        {
            // 2. If not exists, insert new record
            SqlCommand insertCmd = new SqlCommand($"INSERT INTO {tableName} ({nameColumn}) VALUES (@Name); SELECT SCOPE_IDENTITY();", con);
            insertCmd.Parameters.AddWithValue("@Name", name);

            // Get the newly inserted ID
            id = Convert.ToInt32(insertCmd.ExecuteScalar());
        }
        return id;
    }

    private ForeignCosts CalculateForeignCosts(decimal inrCost)
    {
        ForeignCosts costs;
        costs.USD = Math.Round(inrCost / USDRate, 2);
        costs.EUR = Math.Round(inrCost / EURRate, 2);
        costs.GBP = Math.Round(inrCost / GBPRate, 2);
        return costs;
    }

    // Add Button Click (Updated for EUR and GBP)
    // Add Button Click (Updated for FKs)
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            decimal inrCost = Convert.ToDecimal(txtCost.Text.Trim());
            ForeignCosts costs = CalculateForeignCosts(inrCost);
            int authorId, publisherId;

            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // 1. Get/Create Author ID
            authorId = GetOrCreateMasterDataID(txtAuthorNameManual.Text.Trim(), "author_master_tbl", "author_id", "author_name", con);

            // 2. Get/Create Publisher ID
            publisherId = GetOrCreateMasterDataID(txtPublisherNameManual.Text.Trim(), "publisher_master_tbl", "publisher_id", "publisher_name", con);


            // 3. Insert Book Record (Updated Query with FKs)
            SqlCommand cmd = new SqlCommand("INSERT INTO book_master_tbl(book_name, language, author_name, publisher_name, publish_date, edition, book_cost, usd_book_cost, eur_book_cost, gbp_book_cost, no_of_pages, book_description, actual_stock, current_stock, author_fk_id, publisher_fk_id) VALUES(@book_name, @language, @author_name, @publisher_name, @publish_date, @edition, @book_cost, @usd_book_cost, @eur_book_cost, @gbp_book_cost, @no_of_pages, @book_description, @actual_stock, @current_stock, @author_fk_id, @publisher_fk_id)", con);

            cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
            cmd.Parameters.AddWithValue("@language", ddlLanguage.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@author_name", txtAuthorNameManual.Text.Trim());
            cmd.Parameters.AddWithValue("@publisher_name", txtPublisherNameManual.Text.Trim());
            cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
            cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
            cmd.Parameters.AddWithValue("@book_cost", inrCost);
            cmd.Parameters.AddWithValue("@usd_book_cost", costs.USD);
            cmd.Parameters.AddWithValue("@eur_book_cost", costs.EUR);
            cmd.Parameters.AddWithValue("@gbp_book_cost", costs.GBP);
            cmd.Parameters.AddWithValue("@no_of_pages", Convert.ToInt32(txtPages.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@actual_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@current_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@author_fk_id", authorId);
            cmd.Parameters.AddWithValue("@publisher_fk_id", publisherId);


            cmd.ExecuteNonQuery();
            con.Close();
            lblMessage.Text = $"Book added successfully! FKs: A:{authorId}, P:{publisherId}";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error adding book: " + ex.Message;
        }
    }

    // Update Button Click (Updated for FKs)
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            decimal inrCost = Convert.ToDecimal(txtCost.Text.Trim());
            ForeignCosts costs = CalculateForeignCosts(inrCost);
            int authorId, publisherId;

            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // 1. Get/Create Author ID
            authorId = GetOrCreateMasterDataID(txtAuthorNameManual.Text.Trim(), "author_master_tbl", "author_id", "author_name", con);

            // 2. Get/Create Publisher ID
            publisherId = GetOrCreateMasterDataID(txtPublisherNameManual.Text.Trim(), "publisher_master_tbl", "publisher_id", "publisher_name", con);

            // Update Query (Updated with FKs)
            SqlCommand cmd = new SqlCommand("UPDATE book_master_tbl SET book_name=@book_name, language=@language, author_name=@author_name, publisher_name=@publisher_name, publish_date=@publish_date, edition=@edition, book_cost=@book_cost, usd_book_cost=@usd_book_cost, eur_book_cost=@eur_book_cost, gbp_book_cost=@gbp_book_cost, no_of_pages=@no_of_pages, book_description=@book_description, actual_stock=@actual_stock, current_stock=@current_stock, author_fk_id=@author_fk_id, publisher_fk_id=@publisher_fk_id WHERE book_id=@book_id", con);

            cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
            cmd.Parameters.AddWithValue("@language", ddlLanguage.SelectedItem.Value);
            cmd.Parameters.AddWithValue("@author_name", txtAuthorNameManual.Text.Trim());
            cmd.Parameters.AddWithValue("@publisher_name", txtPublisherNameManual.Text.Trim());
            cmd.Parameters.AddWithValue("@publish_date", txtPublishDate.Text.Trim());
            cmd.Parameters.AddWithValue("@edition", txtEdition.Text.Trim());
            cmd.Parameters.AddWithValue("@book_cost", inrCost);
            cmd.Parameters.AddWithValue("@usd_book_cost", costs.USD);
            cmd.Parameters.AddWithValue("@eur_book_cost", costs.EUR);
            cmd.Parameters.AddWithValue("@gbp_book_cost", costs.GBP);
            cmd.Parameters.AddWithValue("@no_of_pages", Convert.ToInt32(txtPages.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_description", txtDescription.Text.Trim());
            cmd.Parameters.AddWithValue("@actual_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@current_stock", Convert.ToInt32(txtActualStock.Text.Trim()));
            cmd.Parameters.AddWithValue("@author_fk_id", authorId);
            cmd.Parameters.AddWithValue("@publisher_fk_id", publisherId);
            cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());

            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();

            if (rowsAffected > 0)
            {
                lblMessage.Text = $"Book updated successfully! FKs: A:{authorId}, P:{publisherId}";
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

    // Delete Button Click (Existing logic)
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

    // GridView Row Select (Updated)
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBookID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtBookName.Text = GridView1.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");

        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("SELECT book_cost, usd_book_cost, eur_book_cost, gbp_book_cost, author_name, publisher_name FROM book_master_tbl WHERE book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                txtCost.Text = dr["book_cost"].ToString();
                txtUSDCost.Text = dr["usd_book_cost"].ToString();
                txtEURCost.Text = dr["eur_book_cost"].ToString();
                txtGBPCost.Text = dr["gbp_book_cost"].ToString();

                txtAuthorNameManual.Text = dr["author_name"].ToString();
                txtPublisherNameManual.Text = dr["publisher_name"].ToString();
            }

            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error fetching book details: " + ex.Message;
        }
    }

    // GridView Row Delete (Existing logic)
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
            SqlCommand cmd = new SqlCommand("SELECT book_id, book_name, book_cost, usd_book_cost, eur_book_cost, gbp_book_cost, actual_stock, current_stock FROM book_master_tbl", con);
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
        txtAuthorNameManual.Text = "";
        txtPublisherNameManual.Text = "";
        txtPublishDate.Text = "";
        txtEdition.Text = "";
        txtCost.Text = "";
        txtUSDCost.Text = "";
        txtEURCost.Text = "";
        txtGBPCost.Text = "";
        txtPages.Text = "";
        txtDescription.Text = "";
        txtActualStock.Text = "";
    }
}