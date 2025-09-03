using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class issue_book : System.Web.UI.Page
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
                bindGridView();
            }
        }
    }
    // Go Button (Member)
    protected void btnGoMember_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT full_name FROM member_master_tbl WHERE member_id=@member_id", con);
            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    txtMemberName.Text = dr.GetValue(0).ToString();
                }
            }
            else
            {
                lblMessage.Text = "Member ID does not exist!";
                txtMemberName.Text = string.Empty;
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    // Go Button (Book)
    protected void btnGoBook_Click(object sender, EventArgs e)
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT book_name, current_stock FROM book_master_tbl WHERE book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@book_id", txtBookID.Text.Trim());
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    if (Convert.ToInt32(dr["current_stock"]) > 0)
                    {
                        txtBookName.Text = dr["book_name"].ToString();
                        lblMessage.Text = string.Empty;
                    }
                    else
                    {
                        lblMessage.Text = "Book is out of stock!";
                        txtBookName.Text = string.Empty;
                    }
                }
            }
            else
            {
                lblMessage.Text = "Book ID does not exist!";
                txtBookName.Text = string.Empty;
            }
            dr.Close();
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    // Issue Button Click
    protected void btnIssue_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMemberName.Text) || string.IsNullOrEmpty(txtBookName.Text))
        {
            lblMessage.Text = "Please fill in all details and click 'Go' to validate.";
            return;
        }
        if (string.IsNullOrEmpty(txtIssueDate.Text) || string.IsNullOrEmpty(txtDueDate.Text))
        {
            lblMessage.Text = "Please select issue and due dates.";
            return;
        }

        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Check if book is already issued to this member
            SqlCommand checkCmd = new SqlCommand("SELECT * FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con);
            checkCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            checkCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
            SqlDataAdapter checkDa = new SqlDataAdapter(checkCmd);
            DataTable checkDt = new DataTable();
            checkDa.Fill(checkDt);

            if (checkDt.Rows.Count > 0)
            {
                lblMessage.Text = "This book is already issued to this member.";
                con.Close();
                return;
            }

            // Issue the book
            SqlCommand cmd = new SqlCommand("INSERT INTO book_issue_tbl(member_id, member_name, book_id, book_name, issue_date, due_date) VALUES(@member_id, @member_name, @book_id, @book_name, @issue_date, @due_date)", con);
            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@member_name", txtMemberName.Text.Trim());
            cmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
            cmd.Parameters.AddWithValue("@book_name", txtBookName.Text.Trim());
            cmd.Parameters.AddWithValue("@issue_date", txtIssueDate.Text.Trim());
            cmd.Parameters.AddWithValue("@due_date", txtDueDate.Text.Trim());
            cmd.ExecuteNonQuery();

            // Update book stock
            SqlCommand updateCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock - 1 WHERE book_id=@book_id", con);
            updateCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
            updateCmd.ExecuteNonQuery();

            con.Close();
            lblMessage.Text = "Book issued successfully!";
            clearFields();
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error issuing book: " + ex.Message;
        }
    }

    // Return Button Click
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtMemberID.Text) || string.IsNullOrEmpty(txtBookID.Text))
        {
            lblMessage.Text = "Please select a record from the list to return.";
            return;
        }

        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            cmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                SqlCommand updateCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con);
                updateCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
                updateCmd.ExecuteNonQuery();

                lblMessage.Text = "Book returned successfully!";
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Book not found in the issued list.";
            }
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error returning book: " + ex.Message;
        }
    }

    // GridView Select event
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMemberID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtMemberName.Text = GridView1.SelectedRow.Cells[2].Text;
        txtBookID.Text = GridView1.SelectedRow.Cells[3].Text;
        txtBookName.Text = GridView1.SelectedRow.Cells[4].Text;
    }

    // GridView Delete event
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            string memberId = GridView1.DataKeys[e.RowIndex].Values["member_id"].ToString();
            int bookId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["book_id"]);

            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand cmd = new SqlCommand("DELETE FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con);
            cmd.Parameters.AddWithValue("@member_id", memberId);
            cmd.Parameters.AddWithValue("@book_id", bookId);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                SqlCommand updateCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con);
                updateCmd.Parameters.AddWithValue("@book_id", bookId);
                updateCmd.ExecuteNonQuery();

                lblMessage.Text = "Book returned successfully!";
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Error: Record not found.";
            }
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error returning book: " + ex.Message;
        }
    }

    private void bindGridView()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM book_issue_tbl", con);
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

    private void clearFields()
    {
        txtMemberID.Text = "";
        txtMemberName.Text = "";
        txtBookID.Text = "";
        txtBookName.Text = "";
        txtIssueDate.Text = "";
        txtDueDate.Text = "";
    }

    //protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        // Selected row se Member ID aur Book ID get karo
    //        string memberId = GridView1.DataKeys[e.RowIndex].Values["member_id"].ToString();
    //        int bookId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["book_id"]);

    //        SqlConnection con = new SqlConnection(strcon);
    //        if (con.State == ConnectionState.Closed)
    //        {
    //            con.Open();
    //        }

    //        // Delete record from book_issue_tbl
    //        SqlCommand cmd = new SqlCommand("DELETE FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id", con);
    //        cmd.Parameters.AddWithValue("@member_id", memberId);
    //        cmd.Parameters.AddWithValue("@book_id", bookId);

    //        int rowsAffected = cmd.ExecuteNonQuery();

    //        if (rowsAffected > 0)
    //        {
    //            // Update book stock
    //            SqlCommand updateCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con);
    //            updateCmd.Parameters.AddWithValue("@book_id", bookId);
    //            updateCmd.ExecuteNonQuery();

    //            lblMessage.Text = "Book returned successfully!";
    //            bindGridView();
    //        }
    //        else
    //        {
    //            lblMessage.Text = "Error: Record not found.";
    //        }

    //        con.Close();
    //    }
    //    catch (Exception ex)
    //    {
    //        lblMessage.Text = "Error returning book: " + ex.Message;
    //    }
    //}
}