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
                // Initial load: View all non-returned books
                ViewState["FilterMemberId"] = null;
                bindGridView();
            }
        }
    }

    // Helper function to set dates (called after verifying member/book details)
    private void SetIssueAndDueDates()
    {
        DateTime issueDate = DateTime.Today;
        DateTime dueDate = issueDate.AddDays(7); // Automatically set to 7 days later

        txtIssueDate.Text = issueDate.ToString("yyyy-MM-dd");
        txtDueDate.Text = dueDate.ToString("yyyy-MM-dd");
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
                dr.Read();
                txtMemberName.Text = dr.GetValue(0).ToString();
                SetIssueAndDueDates();

                // --- NEW: Store filter in ViewState ---
                ViewState["FilterMemberId"] = txtMemberID.Text.Trim();
                lblMessage.Text = $"Showing transactions for member: {txtMemberID.Text.Trim()}";
            }
            else
            {
                lblMessage.Text = "Member ID does not exist! Showing all active transactions.";
                txtMemberName.Text = string.Empty;
                ViewState["FilterMemberId"] = null; // Clear filter if ID is invalid
            }
            dr.Close();
            con.Close();

            // Refresh the GridView based on the new filter state
            bindGridView();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    // Go Button (Book)
    protected void btnGoBook_Click(object sender, EventArgs e)
    {
        // Implementation remains the same (book validation)
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
                dr.Read();
                if (Convert.ToInt32(dr["current_stock"]) > 0)
                {
                    txtBookName.Text = dr["book_name"].ToString();
                    lblMessage.Text = string.Empty;
                    SetIssueAndDueDates();
                }
                else
                {
                    lblMessage.Text = "Book is out of stock!";
                    txtBookName.Text = string.Empty;
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
        // Validate that member and book names are present (meaning lookup succeeded)
        if (string.IsNullOrEmpty(txtMemberName.Text) || string.IsNullOrEmpty(txtBookName.Text))
        {
            lblMessage.Text = "Please enter and validate both Member ID and Book ID.";
            return;
        }

        try
        {
            // --- NEW: Calculate Dates Internally ---
            DateTime issueDate = DateTime.Today;
            DateTime dueDate = issueDate.AddDays(7);

            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Check if book is already issued to this member
            SqlCommand checkCmd = new SqlCommand("SELECT * FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
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
            cmd.Parameters.AddWithValue("@issue_date", issueDate); // Use internal calculation
            cmd.Parameters.AddWithValue("@due_date", dueDate);   // Use internal calculation

            cmd.ExecuteNonQuery();

            // Update book stock
            SqlCommand updateCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock - 1 WHERE book_id=@book_id", con);
            updateCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
            updateCmd.ExecuteNonQuery();

            con.Close();
            lblMessage.Text = $"Book issued successfully! Due Date: {dueDate.ToShortDateString()}";
            bindGridView();
            clearFields();
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

            // Fine calculation logic
            SqlCommand fineCmd = new SqlCommand("SELECT due_date FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
            fineCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            fineCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));

            SqlDataReader dr = fineCmd.ExecuteReader();
            DateTime dueDate = DateTime.Today;

            if (dr.HasRows)
            {
                dr.Read();
                dueDate = Convert.ToDateTime(dr["due_date"]);
            }
            else
            {
                // Book not found as actively issued (or already returned)
                dr.Close();
                con.Close();
                lblMessage.Text = "Error: Book is not currently issued to this member or has already been returned.";
                return;
            }
            dr.Close();

            decimal fine = 0;
            if (DateTime.Today > dueDate)
            {
                TimeSpan ts = DateTime.Today - dueDate;
                int overdueDays = (int)ts.TotalDays;
                // Fine rate: Rs. 2 per day
                fine = overdueDays * 2;
            }

            // Update book_issue_tbl with return date and fine amount
            SqlCommand updateIssueCmd = new SqlCommand("UPDATE book_issue_tbl SET return_date=@return_date, fine_amount=@fine_amount WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
            updateIssueCmd.Parameters.AddWithValue("@return_date", DateTime.Today);
            updateIssueCmd.Parameters.AddWithValue("@fine_amount", fine);
            updateIssueCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            updateIssueCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));

            int rowsAffected = updateIssueCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                // Update book stock
                SqlCommand updateBookCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con);
                updateBookCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));
                updateBookCmd.ExecuteNonQuery();

                lblMessage.Text = "Book returned successfully!" + (fine > 0 ? $" Fine: Rs. {fine:0.00}" : "");
                clearFields();
                bindGridView();
            }
            else
            {
                lblMessage.Text = "Error: Book transaction record not found.";
            }

            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error returning book: " + ex.Message;
        }   
    }

    // GridView Select event(Fine Logic)
    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtMemberID.Text = GridView1.SelectedRow.Cells[1].Text;
        txtMemberName.Text = GridView1.SelectedRow.Cells[2].Text;
        txtBookID.Text = GridView1.SelectedRow.Cells[3].Text;
        txtBookName.Text = GridView1.SelectedRow.Cells[4].Text;

        // Calculate and display fine upon selection
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            SqlCommand fineCmd = new SqlCommand("SELECT due_date FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
            fineCmd.Parameters.AddWithValue("@member_id", txtMemberID.Text.Trim());
            fineCmd.Parameters.AddWithValue("@book_id", Convert.ToInt32(txtBookID.Text.Trim()));

            SqlDataReader dr = fineCmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                DateTime dueDate = Convert.ToDateTime(dr["due_date"]);
                txtDueDate.Text = dueDate.ToString("yyyy-MM-dd"); // Populate Due Date
                txtIssueDate.Text = GridView1.SelectedRow.Cells[5].Text; // Populate Issue Date

                if (DateTime.Today > dueDate)
                {
                    TimeSpan ts = DateTime.Today - dueDate;
                    int overdueDays = (int)ts.TotalDays;
                    decimal fine = overdueDays * 2;
                    lblMessage.Text = $"Overdue by {overdueDays} days. Fine: Rs. {fine:0.00}";
                }
                else
                {
                    lblMessage.Text = "Book is not overdue.";
                }
            }
            con.Close();
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error calculating fine: " + ex.Message;
        }
    }

    // GridView Delete event(Update With Fine Logic)
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        // Existing implementation for Row Deletion/Return remains here
        try
        {
            // Fine calculation logic (same as in btnReturn_Click)
            string memberId = GridView1.DataKeys[e.RowIndex].Values["member_id"].ToString();
            int bookId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["book_id"]);

            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            SqlCommand fineCmd = new SqlCommand("SELECT due_date FROM book_issue_tbl WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
            fineCmd.Parameters.AddWithValue("@member_id", memberId);
            fineCmd.Parameters.AddWithValue("@book_id", bookId);

            SqlDataReader dr = fineCmd.ExecuteReader();
            DateTime dueDate = DateTime.Today;
            if (dr.HasRows)
            {
                dr.Read();
                dueDate = Convert.ToDateTime(dr["due_date"]);
            }
            dr.Close();

            decimal fine = 0;
            if (DateTime.Today > dueDate)
            {
                TimeSpan ts = DateTime.Today - dueDate;
                int overdueDays = (int)ts.TotalDays;
                fine = overdueDays * 2;
            }

            // Update book_issue_tbl with return date and fine amount
            SqlCommand updateIssueCmd = new SqlCommand("UPDATE book_issue_tbl SET return_date=@return_date, fine_amount=@fine_amount WHERE member_id=@member_id AND book_id=@book_id AND return_date IS NULL", con);
            updateIssueCmd.Parameters.AddWithValue("@return_date", DateTime.Today);
            updateIssueCmd.Parameters.AddWithValue("@fine_amount", fine);
            updateIssueCmd.Parameters.AddWithValue("@member_id", memberId);
            updateIssueCmd.Parameters.AddWithValue("@book_id", bookId);

            int rowsAffected = updateIssueCmd.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                SqlCommand updateBookCmd = new SqlCommand("UPDATE book_master_tbl SET current_stock = current_stock + 1 WHERE book_id=@book_id", con);
                updateBookCmd.Parameters.AddWithValue("@book_id", bookId);
                updateBookCmd.ExecuteNonQuery();

                lblMessage.Text = "Book returned successfully!" + (fine > 0 ? $" Fine: Rs. {fine:0.00}" : "");
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
            // Only show books that are currently issued (return_date IS NULL)
            SqlCommand cmd = new SqlCommand("SELECT member_id, member_name, book_id, book_name, issue_date, due_date, fine_amount FROM book_issue_tbl WHERE return_date IS NULL", con);
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