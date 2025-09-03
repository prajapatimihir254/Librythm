using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class adminlogin : System.Web.UI.Page
{
    string strcon = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            // SQL Connection object banayenge
            SqlConnection con = new SqlConnection(strcon);

            // Connection open karenge agar band hai
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }

            // SQL Query likhenge user ko validate karne ke liye
            SqlCommand cmd = new SqlCommand("SELECT * FROM admin_login_tbl WHERE admin_id='" + txtAdminID.Text.Trim() + "' AND password='" + txtPassword.Text.Trim() + "'", con);

            // Data ko SQL DataAdapter mein load karenge
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Agar record mil gaya, to login successful
            if (dt.Rows.Count > 0)
            {
                // Session variables set karenge
                Session["admin_id"] = txtAdminID.Text.Trim();
                Session["role"] = "admin";

                // User ko homepage ya dashboard par redirect karenge
                Response.Redirect("homepage.aspx"); // Temporarily, we will redirect to homepage
            }
            else
            {
                // Agar record nahi mila, to error message display karenge
                lblMessage.Text = "Invalid credentials!";
            }

            // Connection ko band kar denge
            con.Close();
        }
        catch (Exception ex)
        {
            // Error handling
            lblMessage.Text = "An error occurred: " + ex.Message;
        }
    }
}