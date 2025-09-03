using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class admindashboard : System.Web.UI.Page
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
            lblAdminID.Text = Session["admin_id"].ToString();
            if (!IsPostBack)
            {
                loadDashboardData();
            }
        }
    }
    // Helper function to load dashboard data
    private void loadDashboardData()
    {
        try
        {
            SqlConnection con = new SqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // Get count of total books
            SqlCommand cmdBooks = new SqlCommand("SELECT COUNT(*) FROM book_master_tbl", con);
            lblTotalBooks.Text = cmdBooks.ExecuteScalar().ToString();

            // Get count of issued books
            SqlCommand cmdIssued = new SqlCommand("SELECT COUNT(*) FROM book_issue_tbl", con);
            lblIssuedBooks.Text = cmdIssued.ExecuteScalar().ToString();

            // Get count of total members
            SqlCommand cmdMembers = new SqlCommand("SELECT COUNT(*) FROM member_master_tbl", con);
            lblTotalMembers.Text = cmdMembers.ExecuteScalar().ToString();

            con.Close();
        }
        catch (Exception ex)
        {
            // In a real-world app, log this error instead of displaying it.
            lblAdminID.Text = "Error loading data: " + ex.Message;
        }
    }
}